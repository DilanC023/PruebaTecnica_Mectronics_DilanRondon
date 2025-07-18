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
    public class EstudianteRepositorio : RepositorioBase, IEstudianteRepositorio
    {
        public EstudianteRepositorio(IConfiguration configuration) : base(configuration) { }
        public async Task<int> AdicionarEstudianteAsync(EstudianteEntidad estudiante)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "A"),
                CrearParametro("@Nombre", SqlDbType.NVarChar, estudiante.Nombre),
                CrearParametro("@Email", SqlDbType.NVarChar, estudiante.Email),
                CrearParametro("@PasswordHash", SqlDbType.NVarChar, estudiante.PasswordHash)
            };

            var resultado = await EjecutarScalarAsync("sp_TEstudiantes", parametros);
            return Convert.ToInt32(resultado);
        }
        public async Task<EstudianteEntidad> ObtenerEstudiantePorIdAsync(int id)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "C"),
                CrearParametro("@EstudianteId", SqlDbType.Int, id)
            };

            using (var reader = await EjecutarReaderAsync("sp_TEstudiantes", parametros))
            {
                if (await reader.ReadAsync())
                {
                    return new EstudianteEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("EstudianteId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    };
                }
            }
            return null;
        }
        public async Task<IEnumerable<EstudianteEntidad>> ObtenerTodosEstudiantesAsync()
        {
            var estudiantes = new List<EstudianteEntidad>();
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "C")
            };

            using (var reader = await EjecutarReaderAsync("sp_TEstudiantes", parametros))
            {
                while (await reader.ReadAsync())
                {
                    estudiantes.Add(new EstudianteEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("EstudianteId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    });
                }
            }
            return estudiantes;
        }
        public async Task<bool> ModificarEstudianteAsync(EstudianteEntidad estudiante)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "M"),
                CrearParametro("@EstudianteId", SqlDbType.Int, estudiante.Id),
                CrearParametro("@Nombre", SqlDbType.NVarChar, estudiante.Nombre),
                CrearParametro("@Email", SqlDbType.NVarChar, estudiante.Email)
            };

            var resultado = await EjecutarScalarAsync("sp_TEstudiantes", parametros);
            return resultado != null;
        }
        public async Task<bool> DeshabilitarEstudianteAsync(int id)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "E"),
                CrearParametro("@EstudianteId", SqlDbType.Int, id)
            };

            var resultado = await EjecutarScalarAsync("sp_TEstudiantes", parametros);
            return resultado != null;
        }
        public async Task<bool> InscribirMateriaPorEstudianteAsync(int estudianteId, int materiaId)
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
        public async Task<EstudianteEntidad> ObtenerPorEmailAsync(string email)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "CE"),
                CrearParametro("@Email", SqlDbType.NVarChar, email)
            };

            using (var reader = await EjecutarReaderAsync("sp_TEstudiantes", parametros))
            {
                if (await reader.ReadAsync())
                {
                    return new EstudianteEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("EstudianteId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    };
                }
            }
            return null;
        }
    }
}

