using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Precio
    {
        [Key]
        public Guid codprecio{set;get;}

        [Column(TypeName = "decimal(18,2)")]        
        public decimal precioActual{set;get;}

        [Column(TypeName = "decimal(18,2)")]
        public decimal promocion{set;get;}

        public Guid codcurso{set;get;}

        public Curso Curso{set;get;}
    }
}