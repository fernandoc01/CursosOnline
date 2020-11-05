using System;
using System.Linq;
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

                var instructoresDB = _context.CursoInstructor.Where(x => x.CursoId == request.Id);

                foreach(var instructor in instructoresDB){
                    _context.CursoInstructor.Remove(instructor);
                }

                var curso = await _context.Curso.FindAsync(request.Id);

                if(curso==null){
                    //throw new Exception("No se pudo encontrar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{curso = "No se encontró el curso"});
                }

                var listaComentarios = _context.Comentario.Where(x => x.CursoId == request.Id);

                foreach(var comen in listaComentarios){
                    _context.Comentario.Remove(comen);
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