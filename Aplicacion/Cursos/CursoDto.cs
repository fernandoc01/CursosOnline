using System;
using System.Collections;
using System.Collections.Generic;

namespace Aplicacion.Cursos
{
    public class CursoDto
    {
        public Guid CursoId{set;get;}

        public string titulo{set;get;}

        public string descripcion{set;get;}

        public DateTime? FechaPublicacion{set;get;}

        public byte[] FotoPortada{set;get;}

        public ICollection<InstructorDto> Instructores{set;get;}

        public ICollection<ComentarioDto> Comentarios{set;get;}

        public PrecioDto PrecioDto{set;get;}
    }
}