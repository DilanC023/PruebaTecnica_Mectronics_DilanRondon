using AutoMapper;
using EstudiantesApp.Repositorio.Entidades;
using EstudiantesApp.Repositorio.Interfaces;
using EstudiantesApp.Servicio.DTOs;
using EstudiantesApp.Servicio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Servicios
{
    public class MateriaServicio : IMateriaServicio
    {
        private readonly IMateriaRepositorio _materiaRepositorio;
        private readonly IMapper _mapper;

        public MateriaServicio(IMateriaRepositorio materiaRepositorio, IMapper mapper)
        {
            _materiaRepositorio = materiaRepositorio;
            _mapper = mapper;
        }
        public async Task<MateriaDTO> CrearMateriaAsync(MateriaDTO materiaDTO)
        {
            var materia = _mapper.Map<MateriaEntidad>(materiaDTO);
            var id = await _materiaRepositorio.AdicionarMateria(materia);
            materiaDTO.Id = id;
            return materiaDTO;
        }
        public async Task<MateriaDTO> ObtenerMateriaAsync(int id)
        {
            var materia = await _materiaRepositorio.ObtenerMateriaPorId(id);
            return _mapper.Map<MateriaDTO>(materia);
        }
        public async Task<List<MateriaDTO>> ObtenerTodasMateriasAsync()
        {
            var materias = await _materiaRepositorio.ObtenerTodasMaterias();
            return _mapper.Map<List<MateriaDTO>>(materias);
        }
        public async Task<bool> ActualizarMateriaAsync(MateriaDTO materiaDTO)
        {
            var materia = _mapper.Map<MateriaEntidad>(materiaDTO);
            return await _materiaRepositorio.ModificarMateria(materia);
        }
        public async Task<bool> EliminarMateriaAsync(int id)
        {
            return await _materiaRepositorio.EliminarMateria(id);
        }
        public async Task<List<MateriaDTO>> ObtenerMateriaPorProfesorAsync(int idProfesor)
        {
            var materia = await _materiaRepositorio.ObtenerMateriasPorProfesor(idProfesor);
            return _mapper.Map<List<MateriaDTO>>(materia);
        }
    }
}
