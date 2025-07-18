using AutoMapper;
using EstudiantesApp.Repositorio.Entidades;
using EstudiantesApp.Repositorio.Interfaces;
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
    public class EstudianteServicio : IEstudianteServicio
    {
        private readonly IEstudianteRepositorio _estudianteRepositorio;
        private readonly IEstudianteMateriaRepositorio _estudianteMateriaRepositorio;
        private readonly IMapper _mapper;

        public EstudianteServicio(IEstudianteRepositorio estudianteRepositorio, IEstudianteMateriaRepositorio estudianteMateriaRepositorio, IMapper mapper)
        {
            _estudianteRepositorio = estudianteRepositorio;
            _estudianteMateriaRepositorio = estudianteMateriaRepositorio;
            _mapper = mapper;
        }
        public async Task<EstudianteDTO> AdicionarEstudianteAsync(EstudianteDTO estudianteDTO)
        {
            // Validar que el email no exista
            var ExisteEstudiante = await _estudianteRepositorio.ObtenerPorEmailAsync(estudianteDTO.Email);
            if (ExisteEstudiante != null)
                throw new InvalidOperationException("El email ya está registrado");
            var estudiante = _mapper.Map<EstudianteEntidad>(estudianteDTO);

            // Generar hash SHA-256 de la contraseña
            estudiante.PasswordHash = CifradoSHA256.GenerarHashSHA256(estudianteDTO.PasswordHash);

            var id = await _estudianteRepositorio.AdicionarEstudianteAsync(estudiante);
            estudianteDTO.Id = id;

            return estudianteDTO;
        }
        public async Task<bool> ValidarCredencialesAsync(string email, string password)
        {
            var estudiante = await _estudianteRepositorio.ObtenerPorEmailAsync(email);
            if (estudiante == null) return false;

            // Verificar el hash SHA-256
            return CifradoSHA256.VerificarHashSHA256(password, estudiante.PasswordHash);
        }
        public async Task<EstudianteDTO> ObtenerEstudianteAsync(int id)
        {
            var estudiante = await _estudianteRepositorio.ObtenerEstudiantePorIdAsync(id);
            if (estudiante == null) return null;

            var materias = await _estudianteMateriaRepositorio.ObtenerMateriasInscritas(id);

            var estudianteDTO = _mapper.Map<EstudianteDTO>(estudiante);
            estudianteDTO.MateriasInscritas = _mapper.Map<List<MateriaDTO>>(materias);

            return estudianteDTO;
        }
        public async Task<IEnumerable<EstudianteDTO>> ObtenerTodosEstudiantesAsync()
        {
            var estudiantes = await _estudianteRepositorio.ObtenerTodosEstudiantesAsync();
            return _mapper.Map<IEnumerable<EstudianteDTO>>(estudiantes);
        }
        public async Task<bool> ActualizarEstudianteAsync(EstudianteDTO estudianteDTO)
        {
            var estudiante = _mapper.Map<EstudianteEntidad>(estudianteDTO);
            return await _estudianteRepositorio.ModificarEstudianteAsync(estudiante);
        }
        public async Task<bool> DeshabilitarEstudianteAsync(int id)
        {
            return await _estudianteRepositorio.DeshabilitarEstudianteAsync(id);
        }
        public async Task<bool> InscribirMateriaAsync(InscripcionDTO inscripcionDTO)
        {
            // Validar que el estudiante no exceda el límite de materias
            var materiasInscritas = await _estudianteMateriaRepositorio.ObtenerMateriasInscritas(inscripcionDTO.EstudianteId);
            if (materiasInscritas.Count() >= 3)
            {
                throw new InvalidOperationException("El estudiante ya tiene el máximo de 3 materias inscritas");
            }

            return await _estudianteMateriaRepositorio.InscribirMateria(
            inscripcionDTO.EstudianteId,
            inscripcionDTO.MateriaId);
        }
        public async Task<IEnumerable<CompanerosClaseDTO>> ObtenerCompanerosClaseAsync(int estudianteId)
        {
            var materiasInscritas = await _estudianteMateriaRepositorio.ObtenerMateriasInscritas(estudianteId);
            var resultado = new List<CompanerosClaseDTO>();

            foreach (var materia in materiasInscritas)
            {
                var companeros = await _estudianteMateriaRepositorio.ObtenerCompanerosClase(estudianteId, materia.Id);

                var dto = _mapper.Map<CompanerosClaseDTO>(materia);
                dto.Compañeros = _mapper.Map<List<EstudianteSimpleDTO>>(companeros);

                resultado.Add(dto);
            }

            return resultado;
        }
        public async Task<EstudianteDTO> ObtenerEstudiantePorEmailAsync(string email)
        {
            var estudiante = await _estudianteRepositorio.ObtenerPorEmailAsync(email);
            if (estudiante == null) return null;

            var materias = await _estudianteMateriaRepositorio.ObtenerMateriasInscritas(estudiante.Id);

            var estudianteDTO = _mapper.Map<EstudianteDTO>(estudiante);
            estudianteDTO.MateriasInscritas = _mapper.Map<List<MateriaDTO>>(materias);

            return estudianteDTO;
        }
        
    }
}
