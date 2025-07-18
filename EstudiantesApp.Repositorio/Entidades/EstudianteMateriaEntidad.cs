using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Entidades
{
    public class EstudianteMateriaEntidad
    {
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        // Propiedades de navegación
        public virtual EstudianteEntidad Estudiante { get; set; }
        public virtual MateriaEntidad Materia { get; set; }
    }
}
