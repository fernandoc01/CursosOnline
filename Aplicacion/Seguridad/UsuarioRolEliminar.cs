using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolEliminar
    {
        public class Ejecuta : IRequest {
            public string userName{set;get;}

            public string rolName{set;get;}
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>{
            public ValidaEjecuta(){
                RuleFor(x => x.userName).NotEmpty();
                RuleFor(x => x.rolName).NotEmpty();

            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager){
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.rolName);

                if(role == null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, "No se encontro el rol");
                }

                var usuario = await _userManager.FindByNameAsync(request.userName);

                if(usuario == null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, "No se encontro el usuario");
                }

                var resultado = await _userManager.RemoveFromRoleAsync(usuario, request.rolName);

                if(resultado.Succeeded){
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el rol");

            }
        }
    }
}