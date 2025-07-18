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
    public class ProfesorMapper : Profile
    {
        public ProfesorMapper()
        {
            // Mapeo de Profesor
            CreateMap<ProfesorEntidad, ProfesorDTO>();
            CreateMap<ProfesorDTO, ProfesorEntidad>();
        }
    }
}
