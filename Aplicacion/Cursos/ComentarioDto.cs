using System;

namespace Aplicacion.Cursos
{
    public class ComentarioDto
    {
        public Guid ComentarioId{set;get;}

        public string Alumno{set;get;}

        public int Puntaje{set;get;}

        public string ComentarioTexto{set;get;}
    }
}