using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Entidades
{
    public class EstudianteEntidad
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;

        // Propiedades de navegación
        public virtual ICollection<EstudianteMateriaEntidad> MateriasInscritas { get; set; }
    }
}
