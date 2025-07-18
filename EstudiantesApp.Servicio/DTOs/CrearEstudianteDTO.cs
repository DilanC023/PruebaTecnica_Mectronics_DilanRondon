using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.DTOs
{
    public class CrearEstudianteDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "La clave es requerida")]
        [MinLength(8, ErrorMessage = "La clave debe tener al menos 8 caracteres")]
        public string? Clave { get; set; }
    }
}
