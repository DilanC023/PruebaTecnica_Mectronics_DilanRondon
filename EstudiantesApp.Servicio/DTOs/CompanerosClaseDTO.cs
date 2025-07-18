using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.DTOs
{
    public class CompanerosClaseDTO
    {
        public int MateriaId { get; set; }
        public string MateriaNombre { get; set; }
        public List<EstudianteSimpleDTO> Compañeros { get; set; } = new List<EstudianteSimpleDTO>();
    }
}
