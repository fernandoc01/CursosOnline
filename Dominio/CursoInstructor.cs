using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class CursoInstructor
    {
        [Key]
        public Guid CursoId{set;get;}

        public Curso Curso{set;get;}

        [Key]
        public Guid InstructorId{set;get;}

        public Instructor Instructor{set;get;}
    }
}