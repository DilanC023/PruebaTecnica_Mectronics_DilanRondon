using EstudiantesApp.API.Responses;
using EstudiantesApp.Servicio.DTOs;
using EstudiantesApp.Servicio.Interfaces;
using EstudiantesApp.Servicio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EstudiantesApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAutenticacionServicio _autenticacionServicio;
        public AuthController(IAutenticacionServicio autenticacionServicio, ILogger<AuthController> logger) : base(logger)
        {
            _autenticacionServicio = autenticacionServicio;
        }

        [HttpPost("iniciarsesion")]
        public async Task<IActionResult> IniciarSesion(LoginDTO loginDTO)
        {
            try
            {
                var authResponse = await _autenticacionServicio.Autenticar(loginDTO.Email, loginDTO.Password);

                if (authResponse == null)
                {
                    _logger.LogWarning("Intento de login fallido para {Email}", loginDTO.Email);
                    return Unauthorized(APIResponse<object>.RespuestaError("Credenciales inválidas"));
                }

                _logger.LogInformation("Login exitoso para {Email} como {Rol}", loginDTO.Email, authResponse.Rol);

                return Ok(APIResponse<AutenticacionDTO>.RespuestaExitosa(authResponse, "Login exitoso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login para {Email}", loginDTO.Email);
                return StatusCode(500, APIResponse<object>.RespuestaError("Error interno del servidor"));
            }
        }
    }

}
