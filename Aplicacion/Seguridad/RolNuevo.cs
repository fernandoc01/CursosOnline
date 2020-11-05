using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class Ejecuta : IRequest 
        {
            public string nombre{set;get;}
        }

        public class ValidaEjecuta: AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta(){
                RuleFor(x => x.nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public RoleManager<IdentityRole> _roleManager;
            public Manejador(RoleManager<IdentityRole> roleManager){
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.nombre);

                if(role!=null){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, "Ya existe el Rol");
                }
                
                var resultado = await _roleManager.CreateAsync(new IdentityRole(request.nombre));

                if(resultado.Succeeded){
                    return Unit.Value;
                }

                throw new Exception("No se pudo guardar el rol");
            }
        }
    }
}