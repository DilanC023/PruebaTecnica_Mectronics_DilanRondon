using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.DTOs
{
    public class MateriaDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Creditos { get; set; }
        public ProfesorDTO Profesor { get; set; }
    }
}
