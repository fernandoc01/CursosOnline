using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicacion.Cursos
{
    public class PrecioDto
    {
        public Guid codprecio{set;get;}

        [Column(TypeName = "decimal(18,2)")]        
        public decimal precioActual{set;get;}

        [Column(TypeName = "decimal(18,2)")]
        public decimal promocion{set;get;}

        public Guid CursoId{set;get;}
    }
}