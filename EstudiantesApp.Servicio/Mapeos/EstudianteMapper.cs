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
    public class EstudianteMapper : Profile
    {
        public EstudianteMapper()
        {
            // Mapeo de Estudiante
            CreateMap<EstudianteEntidad, EstudianteDTO>()
                .ForMember(dest => dest.MateriasInscritas, opt => opt.MapFrom(src => src.MateriasInscritas.Select(m => m.Materia)));

            CreateMap<EstudianteDTO, EstudianteEntidad>();
            CreateMap<EstudianteEntidad, EstudianteSimpleDTO>();
        }
    }
}
