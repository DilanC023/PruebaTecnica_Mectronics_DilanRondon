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
    public class ProfesorRepositorio : RepositorioBase, IProfesorRepositorio
    {
        public ProfesorRepositorio(IConfiguration configuration) : base(configuration) { }
        public async Task<int> AdicionarProfesorAsync(ProfesorEntidad profesor)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "A"),
                CrearParametro("@Nombre", SqlDbType.NVarChar, profesor.Nombre),
                CrearParametro("@Email", SqlDbType.NVarChar, profesor.Email),
                CrearParametro("@PasswordHash", SqlDbType.NVarChar, profesor.PasswordHash),
            };

            var resultado = await EjecutarScalarAsync("sp_TProfesores", parametros);
            return Convert.ToInt32(resultado);
        }
        public async Task<ProfesorEntidad> ObtenerProfesorPorIdAsync(int id)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "C"),
                CrearParametro("@ProfesorId", SqlDbType.Int, id)
            };

            using (var reader = await EjecutarReaderAsync("sp_TProfesores", parametros))
            {
                if (await reader.ReadAsync())
                {
                    return new ProfesorEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    };
                }
            }
            return null;
        }
        public async Task<IEnumerable<ProfesorEntidad>> ObtenerProfesoresAsync()
        {
            var profesores = new List<ProfesorEntidad>();
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "C")
            };

            using (var reader = await EjecutarReaderAsync("sp_TProfesores", parametros))
            {
                while (await reader.ReadAsync())
                {
                    profesores.Add(new ProfesorEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    });
                }
            }
            return profesores;
        }
        public async Task<bool> ModificarProfesorAsync(ProfesorEntidad profesor)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "M"),
                CrearParametro("@ProfesorId", SqlDbType.Int, profesor.Id),
                CrearParametro("@Nombre", SqlDbType.NVarChar, profesor.Nombre)
            };

            var resultado = await EjecutarScalarAsync("sp_TProfesores", parametros);
            return resultado != null;
        }
        public async Task<bool> EliminarProfesorAsync(int id)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "E"),
                CrearParametro("@ProfesorId", SqlDbType.Int, id)
            };

            var resultado = await EjecutarScalarAsync("sp_TProfesores", parametros);
            return resultado != null;
        }
        public async Task<ProfesorEntidad> ObtenerPorEmailAsync(string email)
        {
            var parametros = new[]
            {
                CrearParametro("@Opcion", SqlDbType.VarChar, "CE"),
                CrearParametro("@Email", SqlDbType.NVarChar, email)
            };

            using (var reader = await EjecutarReaderAsync("sp_TProfesores", parametros))
            {
                if (await reader.ReadAsync())
                {
                    return new ProfesorEntidad
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    };
                }
            }
            return null;
        }
    }
}
