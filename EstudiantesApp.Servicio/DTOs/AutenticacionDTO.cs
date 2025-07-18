using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.DTOs
{
    public class AutenticacionDTO
    {

        public string? Token { get; set; }
        public string? Rol { get; set; }
        public int UsuarioId { get; set; }
        public string? Nombre { get; set; }
    }
}
