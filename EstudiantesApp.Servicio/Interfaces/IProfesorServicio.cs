using EstudiantesApp.Servicio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Interfaces
{
    public interface IProfesorServicio
    {
        Task<ProfesorDTO> AdicionarProfesorAsync(ProfesorDTO profesorDTO);
        Task<ProfesorDTO> ObtenerProfesorAsync(int id);
        Task<IEnumerable<ProfesorDTO>> ObtenerTodosProfesoresAsync();
        Task<bool> ActualizarProfesorAsync(ProfesorDTO profesorDTO);
        Task<bool> EliminarProfesorAsync(int id);
        Task<ProfesorDTO> ObtenerProfesorPorEmailAsync(string email);

    }
}
