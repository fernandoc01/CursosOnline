using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class Comentario
    {
        [Key]
        public Guid ComentarioId{set;get;}

        public string Alumno{set;get;}

        public int Puntaje{set;get;}

        public string ComentarioTexto{set;get;}

        public Guid CUrsoId{set;get;}

        public Curso Curso{set;get;}
    }
}