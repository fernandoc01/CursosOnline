namespace Aplicacion.Cursos
{
    public class InstructorDto
    {
        public System.Guid InstructorId {set;get;}

        public string Nombre{set;get;}

        public string Apellido{set;get;}

        public string Grado{set;get;}

        public byte[] FotoPerfil{set;get;}
    }
}