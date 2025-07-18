using EstudiantesApp.Repositorio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Interfaces
{
    public interface IMateriaRepositorio
    {
        Task<int> AdicionarMateria(MateriaEntidad materia);
        Task<MateriaEntidad> ObtenerMateriaPorId(int id);
        Task<IEnumerable<MateriaEntidad>> ObtenerTodasMaterias();
        Task<bool> ModificarMateria(MateriaEntidad materia);
        Task<bool> EliminarMateria(int id);
        Task<IEnumerable<MateriaEntidad>> ObtenerMateriasPorProfesor(int profesorId);

    }
}
