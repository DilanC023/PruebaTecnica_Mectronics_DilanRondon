using EstudiantesApp.Repositorio.ConexionBD;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio
{
    public abstract class RepositorioBase : IDisposable
    {
        protected SqlConnection _conexion;
        private bool _disposed = false;

        protected RepositorioBase(IConfiguration configuration)
        {
            Conexion.Inicializar(configuration);
            _conexion = Conexion.ObtenerConexion();
        }

        protected async Task<SqlDataReader> EjecutarReaderAsync(string storedProcedure, params SqlParameter[] parameters)
        {
            await AbrirConexionSiNoEstaAbierta();

            var command = new SqlCommand(storedProcedure, _conexion)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }

            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        protected async Task<int> EjecutarNonQueryAsync(string storedProcedure, params SqlParameter[] parameters)
        {
            await AbrirConexionSiNoEstaAbierta();

            using (var command = new SqlCommand(storedProcedure, _conexion))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                return await command.ExecuteNonQueryAsync();
            }
        }

        protected async Task<object> EjecutarScalarAsync(string storedProcedure, params SqlParameter[] parameters)
        {
            await AbrirConexionSiNoEstaAbierta();

            using (var command = new SqlCommand(storedProcedure, _conexion))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                return await command.ExecuteScalarAsync();
            }
        }

        private async Task AbrirConexionSiNoEstaAbierta()
        {
            if (_conexion.State != ConnectionState.Open)
            {
                await _conexion.OpenAsync();
            }
        }

        protected SqlParameter CrearParametro(string nombre, SqlDbType tipo, object valor)
        {
            return new SqlParameter
            {
                ParameterName = nombre,
                SqlDbType = tipo,
                Value = valor ?? DBNull.Value
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_conexion != null)
                    {
                        _conexion.Dispose();
                        _conexion = null;
                    }
                }
                _disposed = true;
            }
        }

        ~RepositorioBase()
        {
            Dispose(false);
        }
    }
}
