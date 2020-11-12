using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            UserManager<Usuario> _userManager;
            IJwtGenerador _ijwtGenerador;
            IUsuarioSesion _iusuarioSesion;
            private readonly CursosOnlineContext _context;

            public Manejador(UserManager<Usuario> userManager, IJwtGenerador ijwtGenerador, IUsuarioSesion iusuarioSesion, CursosOnlineContext context){
                _userManager = userManager;
                _ijwtGenerador = ijwtGenerador;
                _iusuarioSesion = iusuarioSesion;
                _context = context;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_iusuarioSesion.ObtenerUsuarioSesion());
                //Console.WriteLine("usuario: "+usuario.Email);
                
                if(usuario==null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No existe usuario o token no es valido"});
                }

                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);
                var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid( usuario.Id )).FirstOrDefaultAsync();

                if(imagenPerfil!=null){
                    var imagenCliente = new ImagenGeneral{
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };

                    return new UsuarioData{
                    NombreCompleto = usuario.NombreCompleto,
                    UserName = usuario.UserName,
                    Token = _ijwtGenerador.CrearToken(usuario, listaRoles),
                    Email = usuario.Email,
                    ImagenPerfil = imagenCliente
                };
                }else{
                    return new UsuarioData{
                    NombreCompleto = usuario.NombreCompleto,
                    UserName = usuario.UserName,
                    Token = _ijwtGenerador.CrearToken(usuario, listaRoles),
                    Email = usuario.Email
                };
                }

                
            }
        }
    }
}