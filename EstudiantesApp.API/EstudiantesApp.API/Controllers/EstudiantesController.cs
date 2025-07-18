using EstudiantesApp.API.Responses;
using EstudiantesApp.Servicio.DTOs;
using EstudiantesApp.Servicio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstudiantesApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : BaseController
    {
        private readonly IEstudianteServicio _estudianteService;

        public EstudiantesController(IEstudianteServicio estudianteService,
            ILogger<EstudiantesController> logger) : base(logger)
        {
            _estudianteService = estudianteService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEstudiantes()
        {
            _logger.LogInformation("Obteniendo todos los estudiantes");
            try
            {
                var estudiantes = await _estudianteService.ObtenerTodosEstudiantesAsync();
                _logger.LogInformation("Se encontraron {Count} estudiantes", estudiantes.Count());
                return HandleResult(estudiantes);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEstudiantePorId(int id)
        {
            _logger.LogInformation("Buscando estudiante con ID {Id}", id);
            try
            {
                var estudiante = await _estudianteService.ObtenerEstudianteAsync(id);
                if (estudiante == null)
                {
                    _logger.LogWarning("Estudiante con ID {Id} no encontrado", id);
                }
                return HandleResult(estudiante);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<EstudianteDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdicionarEstudiante([FromBody] CrearEstudianteDTO crearEstudianteDTO)
        {
            _logger.LogInformation("Creando nuevo estudiante con email: {Email}", crearEstudianteDTO.Email);

            try
            {
                var estudianteDTO = new EstudianteDTO
                {
                    Nombre = crearEstudianteDTO.Nombre,
                    Email = crearEstudianteDTO.Email,
                    PasswordHash = crearEstudianteDTO.Clave 
                };

                var result = await _estudianteService.AdicionarEstudianteAsync(estudianteDTO);

                _logger.LogInformation("Estudiante creado con ID {Id}", result.Id);

                return CreatedAtAction(nameof(ObtenerEstudiantePorId), new { id = result.Id },
                    APIResponse<EstudianteDTO>.RespuestaExitosa(result, "Estudiante creado exitosamente"));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al crear estudiante");
            }
        }
        [Authorize(Roles = "Estudiante")]
        [HttpPut("modificarestudiante/{id}")]
        public async Task<IActionResult> ModificarEstudiante(int id, EstudianteDTO estudianteDTO)
        {
            _logger.LogInformation("Actualizando estudiante con ID {Id}: {@Estudiante}", id, estudianteDTO);
            try
            {
                if (id != estudianteDTO.Id)
                {
                    _logger.LogWarning("ID no coincide: {Id} vs {EstudianteId}", id, estudianteDTO.Id);
                    return BadRequest(APIResponse<object>.RespuestaError("ID no coincide"));
                }

                var result = await _estudianteService.ActualizarEstudianteAsync(estudianteDTO);
                if (!result)
                {
                    _logger.LogWarning("Estudiante con ID {Id} no encontrado para actualización", id);
                    return NotFound(APIResponse<object>.RespuestaError("Estudiante no encontrado"));
                }

                _logger.LogInformation("Estudiante con ID {Id} actualizado exitosamente", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al actualizar estudiante");
            }
        }
        [Authorize(Roles = "Estudiante")]
        [HttpPut("deshabilitarestudiante/{id}")]
        public async Task<IActionResult> DeshabilitarEstudiante(int id)
        {
            _logger.LogInformation("Deshabilitando estudiante con ID {Id}", id);
            try
            {
                var result = await _estudianteService.DeshabilitarEstudianteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Estudiante con ID {Id} no encontrado para Deshabilitación", id);
                    return NotFound(APIResponse<object>.RespuestaError("Estudiante no encontrado"));
                }

                _logger.LogInformation("Estudiante con ID {Id} eliminado exitosamente", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al eliminar estudiante");
            }
        }
        [Authorize(Roles = "Estudiante")]
        [HttpPost("inscribir")]
        public async Task<IActionResult> InscribirMateria(InscripcionDTO inscripcionDTO)
        {
            _logger.LogInformation("Inscribiendo materia: {@Inscripcion}", inscripcionDTO);
            try
            {
                var result = await _estudianteService.InscribirMateriaAsync(inscripcionDTO);
                if (!result)
                {
                    _logger.LogWarning("No se pudo inscribir la materia: {@Inscripcion}", inscripcionDTO);
                    return BadRequest(APIResponse<object>.RespuestaError("No se pudo inscribir la materia"));
                }

                _logger.LogInformation("Materia inscrita exitosamente: {@Inscripcion}", inscripcionDTO);
                return Ok(APIResponse<object>.RespuestaExitosa(null, "Materia inscrita exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada: {Message}", ex.Message);
                return BadRequest(APIResponse<object>.RespuestaError(ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al inscribir materia");
            }
        }
        [Authorize(Roles = "Estudiante")]
        [HttpGet("companeros/{id}")]
        public async Task<IActionResult> ObtenerCompaneros(int id)
        {
            _logger.LogInformation("Obteniendo compañeros de clase para estudiante ID {Id}", id);
            try
            {
                var companeros = await _estudianteService.ObtenerCompanerosClaseAsync(id);
                _logger.LogInformation("Encontrados {Count} compañeros para estudiante ID {Id}", companeros.Count(), id);
                return HandleResult(companeros);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
