using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest {
            
            public Guid Id{set;get;}

        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context){
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);

                if(curso==null){
                    //throw new Exception("No se pudo encontrar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{curso = "No se encontró el curso"});
                }

                _context.Remove(curso);
                var resultado = await _context.SaveChangesAsync();

                if(resultado>0){
                    return Unit.Value;
                }

                throw new Exception("Ocurrió un error al intentar eliminar el curso");
            }
        }
    }
}