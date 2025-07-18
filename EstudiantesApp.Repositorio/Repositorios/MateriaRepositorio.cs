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
    public class MateriaRepositorio : RepositorioBase, IMateriaRepositorio
    {
        public MateriaRepositorio(IConfiguration configuration) : base(configuration) { }
        public async Task<int> AdicionarMateria(MateriaEntidad materia)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "A"),
                CrearParametro("@Nombre", SqlDbType.NVarChar, materia.Nombre),
                CrearParametro("@Creditos", SqlDbType.Int, materia.Creditos),
                CrearParametro("@ProfesorId", SqlDbType.Int, materia.ProfesorId)
            };

            var resultado = await EjecutarScalarAsync("sp_TMaterias", parametros);
            return Convert.ToInt32(resultado);
        }
        public async Task<MateriaEntidad> ObtenerMateriaPorId(int id)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "C"),
                CrearParametro("@MateriaId", SqlDbType.Int, id)
            };

            using (var reader = await EjecutarReaderAsync("sp_TMaterias", parametros))
            {
                if (await reader.ReadAsync())
                {
                    return new MateriaEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("MateriaId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Creditos = reader.GetInt32(reader.GetOrdinal("Creditos")),
                        ProfesorId = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                        Profesor = new ProfesorEntidad
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                            Nombre = reader.GetString(reader.GetOrdinal("ProfesorNombre"))
                        },
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    };
                }
            }
            return null;
        }
        public async Task<IEnumerable<MateriaEntidad>> ObtenerTodasMaterias()
        {
            var materias = new List<MateriaEntidad>();
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "C")
            };

            using (var reader = await EjecutarReaderAsync("sp_TMaterias", parametros))
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
                        },
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    });
                }
            }
            return materias;
        }
        public async Task<bool> ModificarMateria(MateriaEntidad materia)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "M"),
                CrearParametro("@MateriaId", SqlDbType.Int, materia.Id),
                CrearParametro("@Nombre", SqlDbType.NVarChar, materia.Nombre),
                CrearParametro("@Creditos", SqlDbType.Int, materia.Creditos),
                CrearParametro("@ProfesorId", SqlDbType.Int, materia.ProfesorId)
            };

            var resultado = await EjecutarScalarAsync("sp_TMaterias", parametros);
            return resultado != null;
        }
        public async Task<bool> EliminarMateria(int id)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "E"),
                CrearParametro("@MateriaId", SqlDbType.Int, id)
            };

            var resultado = await EjecutarScalarAsync("sp_TMaterias", parametros);
            return resultado != null;
        }
        public async Task<IEnumerable<MateriaEntidad>> ObtenerMateriasPorProfesor(int profesorId)
        {
            var materias = new List<MateriaEntidad>();
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "CP"),
                CrearParametro("@ProfesorId", SqlDbType.Int, profesorId)
            };

            using (var reader = await EjecutarReaderAsync("sp_TMaterias", parametros))
            {
                while (await reader.ReadAsync())
                {
                    materias.Add(new MateriaEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("MateriaId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Creditos = reader.GetInt32(reader.GetOrdinal("Creditos")),
                        ProfesorId = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    });
                }
            }
            return materias;
        }
    }
}
