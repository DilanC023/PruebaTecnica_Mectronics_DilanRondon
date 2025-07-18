using EstudiantesApp.Servicio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Interfaces
{
    public interface IEstudianteServicio
    {
        Task<EstudianteDTO> AdicionarEstudianteAsync(EstudianteDTO estudianteDTO);
        Task<bool> ValidarCredencialesAsync(string email, string password);
        Task<EstudianteDTO> ObtenerEstudianteAsync(int id);
        Task<IEnumerable<EstudianteDTO>> ObtenerTodosEstudiantesAsync();
        Task<bool> ActualizarEstudianteAsync(EstudianteDTO estudianteDTO);
        Task<bool> DeshabilitarEstudianteAsync(int id);
        Task<bool> InscribirMateriaAsync(InscripcionDTO inscripcionDTO);
        Task<IEnumerable<CompanerosClaseDTO>> ObtenerCompanerosClaseAsync(int estudianteId);
        Task<EstudianteDTO> ObtenerEstudiantePorEmailAsync(string email);
    }
}
