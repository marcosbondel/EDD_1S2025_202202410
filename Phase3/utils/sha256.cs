using System;
using System.Security.Cryptography;
using System.Text;


namespace Utils {
    public static class SHA256Utils {
        // Función para generar hash SHA256
        public static string GenerarHashSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir el texto a bytes
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                // Calcular el hash
                byte[] hashBytes = sha256.ComputeHash(bytes);
                // Convertir el hash a string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                Console.WriteLine($"=============================");
                Console.WriteLine($"GENERAR...");
                Console.WriteLine($"Texto: {texto}");
                Console.WriteLine($"Hash: {builder.ToString()}");
                Console.WriteLine($"=============================");
                return builder.ToString();
            }
        }

        // Función para verificar si un texto coincide con un hash
        public static bool VerificarHashSHA256(string texto, string hash)
        {
            // Generar el hash del texto proporcionado
            string hashGenerado = GenerarHashSHA256(texto);
            // Comparar con el hash proporcionado

            Console.WriteLine($"=============================");
            Console.WriteLine($"VALIDANDO...");
            Console.WriteLine($"Texto: {texto}");
            Console.WriteLine($"Hash: {hash}");
            Console.WriteLine($"Hash Generado: {hashGenerado}");
            Console.WriteLine($"=============================");
            return hashGenerado.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}