using EstudiantesApp.API.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
namespace EstudiantesApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected string RolUsuario => HttpContext.Items["RolUsuario"] as string;

        protected object UsuarioActual => HttpContext.Items["UsuarioActual"];

        protected int UsuarioId
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (int.TryParse(userId, out var id))
                        return id;
                }
                return 0;
            }
        }

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected IActionResult HandleResult<T>(T result, string successMessage = "Operación exitosa")
        {
            if (result == null)
            {
                _logger.LogWarning("Resultado no encontrado");
                return NotFound(APIResponse<T>.RespuestaError("Recurso no encontrado"));
            }

            _logger.LogInformation(successMessage);
            return Ok(APIResponse<T>.RespuestaExitosa(result, successMessage));
        }

        protected IActionResult HandleError(Exception ex, string customMessage = null)
        {
            _logger.LogError(ex, customMessage ?? ex.Message);
            return StatusCode(500, APIResponse<object>.RespuestaError(
                customMessage ?? "Ocurrió un error en el servidor",
                new List<string> { ex.Message }));
        }
    }
}
