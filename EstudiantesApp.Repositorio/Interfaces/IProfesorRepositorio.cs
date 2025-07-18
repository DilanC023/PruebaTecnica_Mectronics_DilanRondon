using EstudiantesApp.Repositorio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Interfaces
{
    public interface IProfesorRepositorio
    {
        Task<int> AdicionarProfesorAsync(ProfesorEntidad profesor);
        Task<ProfesorEntidad> ObtenerProfesorPorIdAsync(int id);
        Task<IEnumerable<ProfesorEntidad>> ObtenerProfesoresAsync();
        Task<bool> ModificarProfesorAsync(ProfesorEntidad profesor);
        Task<bool> EliminarProfesorAsync(int id);
        Task<ProfesorEntidad> ObtenerPorEmailAsync(string email);

    }
}
