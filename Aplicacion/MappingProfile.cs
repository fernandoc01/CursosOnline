using System.Linq;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile(){
            CreateMap<Curso, CursoDto>()
            .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructoresLink.Select(a => a.Instructor).ToList()))
            .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
            .ForMember(x => x.PrecioDto, y => y.MapFrom(y => y.Precio));
            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<Precio, PrecioDto>();
        }
    }
}