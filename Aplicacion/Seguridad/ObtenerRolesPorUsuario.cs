using System.Collections.Generic;
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
    public class ObtenerRolesPorUsuario
    {
        public class Ejecuta : IRequest<List<string>>{
            public string userName{set;get;}
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>{
            public ValidaEjecuta(){
                RuleFor(x => x.userName).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager){
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await  _userManager.FindByNameAsync(request.userName);

                if(usuario == null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No existe el usuario"});
                }

                var rolesDeEsteUsuario = await _userManager.GetRolesAsync(usuario);
                return new List<string>(rolesDeEsteUsuario);
            }
        }
    }
}