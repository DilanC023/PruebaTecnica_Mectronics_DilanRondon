using AutoMapper;
using EstudiantesApp.Repositorio.Entidades;
using EstudiantesApp.Repositorio.Interfaces;
using EstudiantesApp.Repositorio.Repositorios;
using EstudiantesApp.Servicio.DTOs;
using EstudiantesApp.Servicio.Interfaces;
using EstudiantesApp.Servicio.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Servicios
{
    public class ProfesorServicio : IProfesorServicio
    {
        private readonly IProfesorRepositorio _profesorRepositorio;
        private readonly IMateriaServicio _materiaServicio;
        private readonly IMapper _mapper;

        public ProfesorServicio(IProfesorRepositorio profesorRepositorio, IMateriaServicio materiaServicio, IMapper mapper)
        {
            _profesorRepositorio = profesorRepositorio;
            _materiaServicio = materiaServicio; 
            _mapper = mapper;
        }
        public async Task<ProfesorDTO> AdicionarProfesorAsync(ProfesorDTO profesorDTO)
        {
            // Validar que el email no exista
            var ExisteEstudiante = await _profesorRepositorio.ObtenerPorEmailAsync(profesorDTO.Email);
            if (ExisteEstudiante != null)
                throw new InvalidOperationException("El email ya está registrado");
            var profesor = _mapper.Map<ProfesorEntidad>(profesorDTO);

            // Generar hash SHA-256 de la clave
            profesor.PasswordHash = CifradoSHA256.GenerarHashSHA256(profesorDTO.PasswordHash);

            var id = await _profesorRepositorio.AdicionarProfesorAsync(profesor);
            profesorDTO.Id = id;

           
            return profesorDTO;
        }
        public async Task<ProfesorDTO> ObtenerProfesorAsync(int id)
        {
            var profesor = await _profesorRepositorio.ObtenerProfesorPorIdAsync(id);
            return _mapper.Map<ProfesorDTO>(profesor);
        }
        public async Task<IEnumerable<ProfesorDTO>> ObtenerTodosProfesoresAsync()
        {
            var profesores = await _profesorRepositorio.ObtenerProfesoresAsync();
            return _mapper.Map<IEnumerable<ProfesorDTO>>(profesores);
        }
        public async Task<bool> ActualizarProfesorAsync(ProfesorDTO profesorDTO)
        {
            var profesor = _mapper.Map<ProfesorEntidad>(profesorDTO);
            return await _profesorRepositorio.ModificarProfesorAsync(profesor);
        }
        public async Task<bool> EliminarProfesorAsync(int id)
        {
            return await _profesorRepositorio.EliminarProfesorAsync(id);
        }
        public async Task<ProfesorDTO> ObtenerProfesorPorEmailAsync(string email)
        {
            var profesor = await _profesorRepositorio.ObtenerPorEmailAsync(email);
            if (profesor == null) return null;

            var materias = await _materiaServicio.ObtenerTodasMateriasAsync();
            materias = materias.Where(x => x.Profesor.Id == profesor.Id)
                               .ToList();
            var profesorDTO = _mapper.Map<ProfesorDTO>(profesor);
            profesorDTO.MateriasImpartidas = _mapper.Map<List<MateriaDTO>>(materias);

            return profesorDTO;
        }
    }
}
