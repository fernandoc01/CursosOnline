using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class Curso
    {
        [Key]
        public Guid CursoId{set;get;}

        public string titulo{set;get;}

        public string descripcion{set;get;}

        public DateTime? FechaPublicacion{set;get;}

        public byte[] FotoPortada{set;get;}

        public Precio precioPromocion{set;get;}

        public ICollection<Comentario> ComentarioLista{set;get;}

        public ICollection<CursoInstructor> InstructoresLink{set;get;}

    }
}