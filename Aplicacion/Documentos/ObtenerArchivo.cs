using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Documentos
{
    public class ObtenerArchivo
    {
        public class Ejecuta : IRequest<ArchivoGenerico>{
            public Guid Id{set;get;}
        }

        public class Manejador : IRequestHandler<Ejecuta, ArchivoGenerico>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context){
                _context = context;
            }
            public async Task<ArchivoGenerico> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var archivo = await _context.Documento.Where(x => x.ObjetoReferencia == request.Id).FirstAsync();

                if(archivo==null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, "No se encontro el archivo");
                }

                var archivoGenerico = new ArchivoGenerico{
                    Data = Convert.ToBase64String( archivo.Contenido ),
                    Nombre = archivo.Nombre,
                    Extension = archivo.Extension

                };

                return archivoGenerico;
            }
        }
    }
}