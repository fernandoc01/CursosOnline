using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData> {

            public string Email{set;get;}

            public string Password{set;get;}
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{

            public EjecutaValidacion(){
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
            
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _singInManager;
            private readonly IJwtGenerador _ijwtgenerador;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> singInManager, IJwtGenerador ijwtgenerador){
                _userManager = userManager;
                _singInManager = singInManager;
                _ijwtgenerador = ijwtgenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);

                if(usuario==null){
                   throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var resultado = await _singInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                if(resultado.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _ijwtgenerador.CrearToken(usuario),
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Imagen = null
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}