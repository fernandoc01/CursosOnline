using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class Instructor
    {
        [Key]
        public Guid InstructorId{set;get;}

        public string Nombre{set;get;}

        public string Apellido{set;get;}

        public string Grado{set;get;}

        public byte[] FotoPerfil{set;get;}

        public ICollection<CursoInstructor> CursoLink{set;get;}


    }
}