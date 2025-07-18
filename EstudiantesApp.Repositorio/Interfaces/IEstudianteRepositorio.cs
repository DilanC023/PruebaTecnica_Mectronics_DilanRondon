using EstudiantesApp.Repositorio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Interfaces
{
    public interface IEstudianteRepositorio
    {
        Task<int> AdicionarEstudianteAsync(EstudianteEntidad estudiante);
        Task<EstudianteEntidad> ObtenerEstudiantePorIdAsync(int id);
        Task<IEnumerable<EstudianteEntidad>> ObtenerTodosEstudiantesAsync();
        Task<bool> ModificarEstudianteAsync(EstudianteEntidad estudiante);
        Task<bool> DeshabilitarEstudianteAsync(int id);
        Task<bool> InscribirMateriaPorEstudianteAsync(int estudianteId, int materiaId);
        Task<EstudianteEntidad> ObtenerPorEmailAsync(string email);

    }
}
