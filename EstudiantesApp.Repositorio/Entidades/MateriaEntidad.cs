using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Entidades
{
    public class MateriaEntidad
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Creditos { get; set; } = 3;
        public int ProfesorId { get; set; }
        public bool Activo { get; set; } = true;

        // Propiedades de navegación
        public virtual ProfesorEntidad Profesor { get; set; }
        public virtual ICollection<EstudianteMateriaEntidad> EstudiantesInscritos { get; set; }
    }
}
