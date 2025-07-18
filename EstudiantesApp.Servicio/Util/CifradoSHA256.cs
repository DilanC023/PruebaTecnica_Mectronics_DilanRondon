using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Util
{
    public static class CifradoSHA256
    {
        /// <summary>
        /// Genera un hash SHA-256 de una cadena de texto
        /// </summary>
        /// <param name="input">Texto a hashear</param>
        /// <returns>Hash SHA-256 en formato hexadecimal</returns>
        public static string GenerarHashSHA256(string? input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir el string a bytes
                byte[] bytes = Encoding.UTF8.GetBytes(input);

                // Generar el hash
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convertir el hash a string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    builder.Append(hashBytes[i].ToString("x2"));

                return builder.ToString();
            }
        }

        /// <summary>
        /// Verifica si un texto coincide con un hash SHA-256
        /// </summary>
        /// <param name="input">Texto a verificar</param>
        /// <param name="hash">Hash almacenado para comparación</param>
        /// <returns>True si coinciden, False si no</returns>
        public static bool VerificarHashSHA256(string input, string hash)
        {
            // Generar hash del input
            string hashOfInput = GenerarHashSHA256(input);

            // Comparar los hashes (comparación segura contra ataques de tiempo)
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        /// <summary>
        /// Genera un hash SHA-256 con salt
        /// </summary>
        /// <param name="input">Texto a hashear</param>
        /// <param name="salt">Salt para agregar seguridad</param>
        /// <returns>Hash con salt en formato hexadecimal</returns>
        public static string GenerarHashSHA256ConSalt(string input, string salt)
        {
            // Combinar input y salt
            string saltedInput = string.Concat(input, salt);
            return GenerarHashSHA256(saltedInput);
        }
    }
}
