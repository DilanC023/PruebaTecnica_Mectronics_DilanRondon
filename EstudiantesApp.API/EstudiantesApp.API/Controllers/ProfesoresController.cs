using EstudiantesApp.API.Responses;
using EstudiantesApp.Servicio.DTOs;
using EstudiantesApp.Servicio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstudiantesApp.API.Controllers
{
    [Authorize(Roles = "Profesor")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesoresController : BaseController
    {
        private readonly IProfesorServicio _profesorServicio;

        public ProfesoresController(
            IProfesorServicio profesorServicio,
            ILogger<ProfesoresController> logger) : base(logger)
        {
            _profesorServicio = profesorServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProfesores()
        {
            _logger.LogInformation("Obteniendo todos los profesores");
            try
            {
                var profesores = await _profesorServicio.ObtenerTodosProfesoresAsync();
                _logger.LogInformation("Se encontraron {Count} profesores", profesores.Count());
                return HandleResult(profesores);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> ObtenerProfesorPorId(int id)
        {
            _logger.LogInformation("Buscando profesor con ID {Id}", id);
            try
            {
                var profesor = await _profesorServicio.ObtenerProfesorAsync(id);
                if (profesor == null)
                {
                    _logger.LogWarning("Profesor con ID {Id} no encontrado", id);
                }
                return HandleResult(profesor);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<ProfesorDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdicionarProfesor(CrearProfesorDTO crearProfesorDTO)
        {
            _logger.LogInformation("Creando nuevo profesor: {@Profesor}", crearProfesorDTO);
            try
            {
                var profesorDTO = new ProfesorDTO
                {
                    Nombre = crearProfesorDTO.Nombre,
                    Email = crearProfesorDTO.Email,
                    PasswordHash = crearProfesorDTO.Clave,
                };
                var result = await _profesorServicio.AdicionarProfesorAsync(profesorDTO);
                _logger.LogInformation("Profesor creado con ID {Id}", result.Id);
                return CreatedAtAction(nameof(ObtenerProfesorPorId), new { id = result.Id },
                    APIResponse<ProfesorDTO>.RespuestaExitosa(result, "Profesor creado exitosamente"));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al crear profesor");
            }
        }

        [HttpPut("modificarprofesor/{id}")]
        [ProducesResponseType(typeof(APIResponse<ProfesorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarProfesor(int id, ProfesorDTO profesorDTO)
        {
            _logger.LogInformation("Actualizando profesor con ID {Id}: {@Profesor}", id, profesorDTO);
            try
            {
                if (id != profesorDTO.Id)
                {
                    _logger.LogWarning("ID no coincide: {Id} vs {ProfesorId}", id, profesorDTO.Id);
                    return BadRequest(APIResponse<object>.RespuestaError("ID no coincide"));
                }

                var result = await _profesorServicio.ActualizarProfesorAsync(profesorDTO);
                if (!result)
                {
                    _logger.LogWarning("Profesor con ID {Id} no encontrado para actualización", id);
                    return NotFound(APIResponse<object>.RespuestaError("Profesor no encontrado"));
                }

                _logger.LogInformation("Profesor con ID {Id} actualizado exitosamente", id);
                return Ok(APIResponse<ProfesorDTO>.RespuestaExitosa(profesorDTO, "Profesor actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al actualizar profesor");
            }
        }

        [HttpPut("deshabilitarprofesor/{id}")]
        [ProducesResponseType(typeof(APIResponse<ProfesorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeshabilitarProfesor(int id)
        {
            _logger.LogInformation("Deshabilitando profesor con ID {Id}", id);
            try
            {
                var result = await _profesorServicio.EliminarProfesorAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Profesor con ID {Id} no encontrado para deshabilitación", id);
                    return NotFound(APIResponse<object>.RespuestaError("Profesor no encontrado"));
                }
                var profesorDTO = await _profesorServicio.ObtenerProfesorAsync(id);
                _logger.LogInformation("Profesor con ID {Id} deshabilitado exitosamente", id);
                return Ok(APIResponse<ProfesorDTO>.RespuestaExitosa(profesorDTO, "Profesor deshabilitado exitosamente"));

            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al deshabilitar profesor");
            }
        }
    }
}
