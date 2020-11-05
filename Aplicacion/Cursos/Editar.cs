using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest{

            public Guid CursoId{set;get;}

            public string titulo{set;get;}

            public string descripcion{set;get;}

            public DateTime? fechaPublicacion{set;get;}

            public List<Guid> ListaInstructor{set;get;}

        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context){
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.CursoId);
                
                if(curso==null){
                    //throw new Exception("No se pudo encontrar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{curso = "No se encontró el curso"});
                }

                curso.titulo = request.titulo ?? curso.titulo;
                curso.descripcion = request.descripcion ?? curso.descripcion;
                curso.FechaPublicacion = request.fechaPublicacion ?? curso.FechaPublicacion;

                if(request.ListaInstructor!=null){
                    if(request.ListaInstructor.Count>0){
                        var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        
                        //Para eliminar los instructores de este curso
                        foreach(var id in instructoresBD){
                            _context.CursoInstructor.Remove(id);
                        }

                        //Para agregar los instructores que esta ingresando el usuario
                        foreach(var id in request.ListaInstructor){
                            var nuevoInstructor = new CursoInstructor{
                              CursoId = request.CursoId,
                              InstructorId = id
                            };

                            _context.CursoInstructor.Add(nuevoInstructor);

                        }
                    }
                }

                var resultado = await _context.SaveChangesAsync();

                if(resultado>0){
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el curso");

            }
        }
    }
}