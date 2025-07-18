using AutoMapper;
using EstudiantesApp.Repositorio.Entidades;
using EstudiantesApp.Servicio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Mapeos
{
    public class EstudianteMateriaMapper : Profile
    {
        public EstudianteMateriaMapper() 
        {
            // Mapeo de Inscripción
            CreateMap<InscripcionDTO, EstudianteMateriaEntidad>()
                .ForMember(dest => dest.EstudianteId, opt => opt.MapFrom(src => src.EstudianteId))
                .ForMember(dest => dest.MateriaId, opt => opt.MapFrom(src => src.MateriaId))
                .ForMember(dest => dest.FechaInscripcion, opt => opt.MapFrom(_ => DateTime.Now));

            // Mapeo para compañeros de clase
            CreateMap<MateriaEntidad, CompanerosClaseDTO>()
                .ForMember(dest => dest.MateriaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MateriaNombre, opt => opt.MapFrom(src => src.Nombre));
        }
    }
}
