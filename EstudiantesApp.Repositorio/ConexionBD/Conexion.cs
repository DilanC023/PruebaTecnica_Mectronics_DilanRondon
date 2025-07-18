using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Repositorio.ConexionBD
{
    public static class Conexion
    {
        private static string? _instancia;
        private static readonly object _lock = new object();
        private static bool _inicializar = false;

        /// <summary>
        /// Inicializa la conexión con la cadena de conexión desde IConfiguration
        /// </summary>
        /// <param name="configuration">Instancia de IConfiguration</param>
        public static void Inicializar(IConfiguration configuration)
        {
            if (!_inicializar)
            {
                lock (_lock)
                {
                    if (!_inicializar)
                    {
                        _instancia = configuration.GetConnectionString("DefaultConnection");
                        _inicializar = true;
                    }
                }
            }
        }
        public static SqlConnection ObtenerConexion()
        {
            if (!_inicializar)
                throw new InvalidOperationException("La conexión no ha sido inicializada. Llame a Initialize() primero.");

            var connection = new SqlConnection(_instancia);
            connection.Open();
            return connection;
        }
    }
}
