using EstudiantesApp.Repositorio.Entidades;
using EstudiantesApp.Repositorio.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.Repositorios
{
    public class EstudianteMateriaRepositorio : RepositorioBase, IEstudianteMateriaRepositorio
    {
        public EstudianteMateriaRepositorio(IConfiguration configuration) : base(configuration) { }
        public async Task<bool> InscribirMateria(int estudianteId, int materiaId)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "A"),
                CrearParametro("@EstudianteId", SqlDbType.Int, estudianteId),
                CrearParametro("@MateriaId", SqlDbType.Int, materiaId),
                CrearParametro("@ValidarRestricciones", SqlDbType.Bit, 1)
            };

            using (var reader = await EjecutarReaderAsync("sp_TEstudiantesMaterias", parametros))
            {
                if (await reader.ReadAsync())
                {
                    return reader.GetInt32(0) == 1;
                }
            }
            return false;
        }
        public async Task<bool> EliminarInscripcion(int estudianteId, int materiaId)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "E"),
                CrearParametro("@EstudianteId", SqlDbType.Int, estudianteId),
                CrearParametro("@MateriaId", SqlDbType.Int, materiaId)
            };

            var resultado = await EjecutarNonQueryAsync("sp_TEstudiantesMaterias", parametros);
            return resultado > 0;
        }
        public async Task<IEnumerable<MateriaEntidad>> ObtenerMateriasInscritas(int estudianteId)
        {
            var materias = new List<MateriaEntidad>();
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "C"),
                CrearParametro("@EstudianteId", SqlDbType.Int, estudianteId)
            };

            using (var reader = await EjecutarReaderAsync("sp_TEstudiantesMaterias", parametros))
            {
                while (await reader.ReadAsync())
                {
                    materias.Add(new MateriaEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("MateriaId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Creditos = reader.GetInt32(reader.GetOrdinal("Creditos")),
                        ProfesorId = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                        Profesor = new ProfesorEntidad
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                            Nombre = reader.GetString(reader.GetOrdinal("ProfesorNombre"))
                        }
                    });
                }
            }
            return materias;
        }
        public async Task<IEnumerable<EstudianteEntidad>> ObtenerCompanerosClase(int estudianteId, int materiaId)
        {
            var companeros = new List<EstudianteEntidad>();
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "CC"),
                CrearParametro("@EstudianteId", SqlDbType.Int, estudianteId),
                CrearParametro("@MateriaId", SqlDbType.Int, materiaId)
            };

            using (var reader = await EjecutarReaderAsync("sp_TEstudiantesMaterias", parametros))
            {
                while (await reader.ReadAsync())
                {
                    companeros.Add(new EstudianteEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("EstudianteId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
                    });
                }
            }
            return companeros;
        }
    }
}
