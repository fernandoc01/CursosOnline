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
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>{
            
            public string Nombre{set;get;}

            public string Apellido{set;get;}

            public string Email{set;get;}

            public string Password{set;get;}

            public string UserName{set;get;}

        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>{
            public EjecutaValidador(){
                RuleFor( x => x.Nombre).NotEmpty();
                RuleFor( x => x.Apellido).NotEmpty();
                RuleFor( x => x.Email).NotEmpty();
                RuleFor( x => x.Password).NotEmpty();
                RuleFor( x => x.UserName).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>{

            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _ijwtGenerador;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador ijWtGenerador){
                _context = context;
                _userManager = userManager;
                _ijwtGenerador = ijWtGenerador;
            }
        
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where( x => x.Email == request.Email).AnyAsync();

                if(existe){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "El email ingresado ya existe"});
                }

                var existeUserName = await _context.Users.Where( x => x.UserName == request.UserName).AnyAsync();

                if(existeUserName){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "El nombre de usuario ingresado ya existe"});
                }

                var usuario = new Usuario{
                    NombreCompleto = request.Nombre + request.Apellido,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var resultado = await _userManager.CreateAsync(usuario, request.Password);

                if(resultado.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _ijwtGenerador.CrearToken(usuario),
                        UserName = usuario.UserName,
                        Email = usuario.Email
                    };
                }

                throw new System.Exception("No se pudo agregar al nuevo usuario");

            }
        }
    }
}