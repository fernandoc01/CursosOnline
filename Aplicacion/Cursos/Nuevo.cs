using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest {
            
            //public Guid CursoId{set;get;}

            //0[Required(ErrorMessage="Por favor, ingrese un título")]
            public string titulo{set;get;}

            public string descripcion{set;get;}

            public DateTime? FechaPublicacion{set;get;}

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor( x => x.titulo).NotEmpty();
                RuleFor( x => x.descripcion).NotEmpty();
                RuleFor( x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context){
                _context = context;

            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = new Curso{
                  titulo = request.titulo,
                  descripcion = request.descripcion,
                  FechaPublicacion = request.FechaPublicacion
                };

                _context.Curso.Add(curso);
                var valor = await _context.SaveChangesAsync();
                if(valor>0){
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");

            }
        }
    }
}