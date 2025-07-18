using EstudiantesApp.Servicio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Interfaces
{
    public interface IMateriaServicio
    {
        Task<MateriaDTO> CrearMateriaAsync(MateriaDTO materiaDTO);
        Task<MateriaDTO> ObtenerMateriaAsync(int id);
        Task<List<MateriaDTO>> ObtenerTodasMateriasAsync();
        Task<bool> ActualizarMateriaAsync(MateriaDTO materiaDTO);
        Task<bool> EliminarMateriaAsync(int id);
        Task<List<MateriaDTO>> ObtenerMateriaPorProfesorAsync(int idProfesor);


    }
}
