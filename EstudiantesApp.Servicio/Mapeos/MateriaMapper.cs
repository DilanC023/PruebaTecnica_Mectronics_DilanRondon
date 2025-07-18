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
    public class MateriaMapper : Profile
    {
        public MateriaMapper() 
        {
            // Mapeo de Materia
            CreateMap<MateriaEntidad, MateriaDTO>();
            CreateMap<MateriaDTO, MateriaEntidad>();
        }  
    }
}
