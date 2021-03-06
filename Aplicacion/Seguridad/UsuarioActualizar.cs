using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>{
        public string NombreCompleto{set;get;}

        public string Email{set;get;}

        public string Password{set;get;}
        
        public string UserName{set;get;}

        public ImagenGeneral ImagenPerfil{set;get;}
        
        }
        
        public class ValidaUsuarioActualizar : AbstractValidator<Ejecuta>{
        public ValidaUsuarioActualizar(){
            RuleFor(x => x.NombreCompleto).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();        
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _ijwtGenerador;

            private readonly IPasswordHasher<Usuario> _ipasswordHasher;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador ijwtGenerador, IPasswordHasher<Usuario> ipasswordHasher){
                _context = context;
                _userManager = userManager;
                _ijwtGenerador = ijwtGenerador;
                _ipasswordHasher = ipasswordHasher;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(request.UserName);

                if(usuario==null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro un usuario con ese UserName"});
                }

                var resultado = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync();

                if(resultado){
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, "Este email pertenece a otro usuario");
                }

                if(request.ImagenPerfil!=null){
                    Console.WriteLine("Le estoy enviando una imagen desde el front");
                    var resultadoImagen = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid( usuario.Id )).FirstOrDefaultAsync();
                    if(resultadoImagen==null){
                    var imagen = new Documento{
                        Contenido = System.Convert.FromBase64String( request.ImagenPerfil.Data ),
                        Nombre = request.ImagenPerfil.Nombre,
                        Extension = request.ImagenPerfil.Extension,
                        ObjetoReferencia = new Guid( usuario.Id ),
                        DocumentoId = Guid.NewGuid(),
                        FechaCreacion = DateTime.UtcNow
                    };
                    Console.WriteLine("Añadir nueva imagen de perfil");
                    _context.Documento.Add(imagen);
                    }else{
                        Console.WriteLine("Actualizar imagen de perfil");
                        resultadoImagen.Contenido = System.Convert.FromBase64String( request.ImagenPerfil.Data );
                        resultadoImagen.Nombre = request.ImagenPerfil.Nombre;
                        resultadoImagen.Extension = request.ImagenPerfil.Extension;    
                    }
                }

                usuario.NombreCompleto = request.NombreCompleto;
                usuario.PasswordHash = _ipasswordHasher.HashPassword(usuario, request.Password);
                usuario.Email = request.Email;

                var updateUsuario = await _userManager.UpdateAsync(usuario);
                var listaRolesIList = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(listaRolesIList);

                var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid( usuario.Id )).FirstOrDefaultAsync();

                ImagenGeneral imagenGeneral = null;
                if(imagenPerfil!=null){
                    imagenGeneral = new ImagenGeneral{
                        Data = Convert.ToBase64String( imagenPerfil.Contenido ),
                        Nombre = imagenPerfil.Nombre,
                        Extension = imagenPerfil.Extension
                    };
                }

                if(updateUsuario.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Token = _ijwtGenerador.CrearToken(usuario, listaRoles),
                        ImagenPerfil = imagenGeneral
                    };
                }

                throw new Exception("No se pudo actualizar los datos del usuario");

            }
        }
    }

    
}