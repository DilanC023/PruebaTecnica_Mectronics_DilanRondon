using EstudiantesApp.Repositorio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Interfaces
{
    public interface IEstudianteMateriaRepositorio
    {
        Task<bool> InscribirMateria(int estudianteId, int materiaId);
        Task<bool> EliminarInscripcion(int estudianteId, int materiaId);
        Task<IEnumerable<MateriaEntidad>> ObtenerMateriasInscritas(int estudianteId);
        Task<IEnumerable<EstudianteEntidad>> ObtenerCompanerosClase(int estudianteId, int materiaId);
    }
}
