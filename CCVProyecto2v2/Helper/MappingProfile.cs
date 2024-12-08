using AutoMapper;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;

namespace CCVProyecto2v2.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Estudiante, EstudianteDto>();
            CreateMap<EstudianteDto, Estudiante>();
            CreateMap<Profesor, ProfesorDto>();
            CreateMap<ProfesorDto, Profesor>();
            CreateMap<Clase, ClaseDto>();
        }
    }
}
