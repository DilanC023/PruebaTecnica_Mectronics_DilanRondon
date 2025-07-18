using EstudiantesApp.API.Responses;
using EstudiantesApp.Servicio.DTOs;
using EstudiantesApp.Servicio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstudiantesApp.API.Controllers
{
    [Authorize(Roles = "Profesor, Estudiante")]
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasController : BaseController
    {
        private readonly IMateriaServicio _materiaService;

        public MateriasController(IMateriaServicio materiaService, ILogger<MateriasController> logger) : base(logger)
        {
            _materiaService = materiaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMaterias()
        {
            _logger.LogInformation("Obteniendo todas las materias");
            try
            {
                var materias = new List<MateriaDTO>(); ;
                if (RolUsuario.Equals("Estudiante"))
                    materias = await _materiaService.ObtenerTodasMateriasAsync();
                if (RolUsuario.Equals("Profesor"))
                    materias = await _materiaService.ObtenerMateriaPorProfesorAsync(UsuarioId);
                _logger.LogInformation("Se encontraron {Count} materias", materias.Count());
                return HandleResult(materias);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerMateriaPorId(int id)
        {
            _logger.LogInformation("Buscando materia con ID {Id}", id);
            try
            {
                var materia = await _materiaService.ObtenerMateriaAsync(id);
                if (materia == null)
                {
                    _logger.LogWarning("Materia con ID {Id} no encontrada", id);
                }
                return HandleResult(materia);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        [Authorize(Roles = "Profesor")]
        [HttpPost]
        public async Task<IActionResult> AdicionarMateria(MateriaDTO materiaDTO)
        {
            _logger.LogInformation("Creando nueva materia: {@Materia}", materiaDTO);
            try
            {
                var result = await _materiaService.CrearMateriaAsync(materiaDTO);
                _logger.LogInformation("Materia creada con ID {Id}", result.Id);
                return CreatedAtAction(nameof(ObtenerMateriaPorId), new { id = result.Id },
                    APIResponse<MateriaDTO>.RespuestaExitosa(result, "Materia creada exitosamente"));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al crear materia");
            }
        }
        [Authorize(Roles = "Profesor")]
        [HttpPut("modificarmateria/{id}")]
        public async Task<IActionResult> ModificarMateria(int id, MateriaDTO materiaDTO)
        {
            _logger.LogInformation("Actualizando materia con ID {Id}: {@Materia}", id, materiaDTO);
            try
            {
                if (id != materiaDTO.Id)
                {
                    _logger.LogWarning("ID no coincide: {Id} vs {MateriaId}", id, materiaDTO.Id);
                    return BadRequest(APIResponse<object>.RespuestaError("ID no coincide"));
                }

                var result = await _materiaService.ActualizarMateriaAsync(materiaDTO);
                if (!result)
                {
                    _logger.LogWarning("Materia con ID {Id} no encontrada para actualización", id);
                    return NotFound(APIResponse<object>.RespuestaError("Materia no encontrada"));
                }

                _logger.LogInformation("Materia con ID {Id} actualizada exitosamente", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al actualizar materia");
            }
        }
        [Authorize(Roles = "Profesor")]
        [HttpPut("deshabilitarmateria/{id}")]
        public async Task<IActionResult> DeshabilitarMateria(int id)
        {
            _logger.LogInformation("Deshabilitando materia con ID {Id}", id);
            try
            {
                var result = await _materiaService.EliminarMateriaAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Materia con ID {Id} no encontrada para Deshabilitación", id);
                    return NotFound(APIResponse<object>.RespuestaError("Materia no encontrada"));
                }

                _logger.LogInformation("Materia con ID {Id} deshabilitada exitosamente", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al deshabilitar materia");
            }
        }
    }
}
