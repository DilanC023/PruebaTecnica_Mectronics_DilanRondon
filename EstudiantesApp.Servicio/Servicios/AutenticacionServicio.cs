using EstudiantesApp.Repositorio.Interfaces;
using EstudiantesApp.Servicio.DTOs;
using EstudiantesApp.Servicio.Interfaces;
using EstudiantesApp.Servicio.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Servicios
{

    public class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IEstudianteRepositorio _estudianteRepositorio;
        private readonly IProfesorRepositorio _profesorRepositorio;
        private readonly IConfiguration _configuration;
        public AutenticacionServicio(IEstudianteRepositorio estudianteRepo, IProfesorRepositorio profesorRepo, IConfiguration configuration)
        {
            _estudianteRepositorio = estudianteRepo;
            _profesorRepositorio = profesorRepo;
            _configuration = configuration;
        }

        public async Task<AutenticacionDTO> Autenticar(string email, string password)
        {
            // Primero buscamos como estudiante
            var estudiante = await _estudianteRepositorio.ObtenerPorEmailAsync(email);
            if (estudiante != null && estudiante.Activo &&
                CifradoSHA256.VerificarHashSHA256(password, estudiante.PasswordHash))
            {
                return new AutenticacionDTO
                {
                    Token = GenerateJwtToken(estudiante, "Estudiante"),
                    Rol = "Estudiante",
                    UsuarioId = estudiante.Id,
                    Nombre = estudiante.Nombre
                };
            }

            // Si no es estudiante, buscamos como profesor
            var profesor = await _profesorRepositorio.ObtenerPorEmailAsync(email);
            if (profesor != null && profesor.Activo &&
                CifradoSHA256.VerificarHashSHA256(password, profesor.PasswordHash))
            {
                return new AutenticacionDTO
                {
                    Token = GenerateJwtToken(profesor, "Profesor"),
                    Rol = "Profesor",
                    UsuarioId = profesor.Id,
                    Nombre = profesor.Nombre
                };
            }

            // Si no encontró ninguno, credenciales inválidas
            return null;
        }

        private string GenerateJwtToken(object usuario, string rol)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.GetType().GetProperty("Id").GetValue(usuario).ToString()),
                new Claim(ClaimTypes.Name, usuario.GetType().GetProperty("Nombre").GetValue(usuario).ToString()),
                new Claim(ClaimTypes.Role, rol),
                new Claim("rol", rol)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
