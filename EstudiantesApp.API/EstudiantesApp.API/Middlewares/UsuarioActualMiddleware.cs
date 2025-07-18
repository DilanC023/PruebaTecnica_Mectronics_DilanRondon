using EstudiantesApp.Repositorio.Interfaces;
using System.Security.Claims;

namespace EstudiantesApp.API.Middlewares
{
    public class UsuarioActualMiddleware
    {
        private readonly RequestDelegate _next;

        public UsuarioActualMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IEstudianteRepositorio estudianteRepositorio,IProfesorRepositorio profesorRepositorio)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var rol = context.User.FindFirstValue(ClaimTypes.Role);

                if (rol == "Estudiante" && int.TryParse(userId, out var estudianteId))
                {
                    var estudiante = await estudianteRepositorio.ObtenerEstudiantePorIdAsync(estudianteId);
                    context.Items["UsuarioActual"] = estudiante;
                    context.Items["RolUsuario"] = "Estudiante";
                }
                else if (rol == "Profesor" && int.TryParse(userId, out var profesorId))
                {
                    var profesor = await profesorRepositorio.ObtenerProfesorPorIdAsync(profesorId);
                    context.Items["UsuarioActual"] = profesor;
                    context.Items["RolUsuario"] = "Profesor";
                }
            }

            await _next(context);
        }
    }
}
