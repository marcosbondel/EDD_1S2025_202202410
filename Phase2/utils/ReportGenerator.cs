using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Utils
{
    public static class ReportGenerator
    {
        // Función que genera un archivo .dot en la carpeta "reports" con el nombre y contenido especificado
        public static void GenerateDotFile(string nombre, string contenido)
        {
            try
            {
                Console.WriteLine("Generando archivo.dot...");
                Console.WriteLine($"Dotcode: {contenido}");

                // Use Path.Combine to ensure cross-platform compatibility for directory paths
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "reports");

                Console.WriteLine($"carpeta: {carpeta}");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                // Validate the file name
                if (string.IsNullOrEmpty(nombre))
                {
                    Console.WriteLine("El nombre del archivo no puede ser nulo o vacío.");
                    return;
                }

                // Ensure the file name ends with .dot
                if (!nombre.EndsWith(".dot", StringComparison.OrdinalIgnoreCase))
                {
                    nombre += ".dot";
                }

                // Combine the directory and file name using Path.Combine
                string rutaArchivo = Path.Combine(carpeta, nombre);

                Console.WriteLine($"rutaArchivo: {rutaArchivo}");
                // Write the content to the file
                File.WriteAllText(rutaArchivo, contenido);

                Console.WriteLine($"Archivo generado con éxito en: {rutaArchivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el archivo: {ex.Message}");
            }
        }

        // Función que convierte un archivo .dot en una imagen jpg
        public static void ParseDotToImage(string nombreReporte)
        {
            try
            {
                if (string.IsNullOrEmpty(nombreReporte))
                {
                    Console.WriteLine("El nombre del archivo no es válido.");
                    return;
                }

                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "reports");
                string archivoDot = Path.Combine(carpeta, nombreReporte);

                if (!File.Exists(archivoDot))
                {
                    Console.WriteLine($"El archivo {nombreReporte} no existe en la carpeta 'reports'.");
                    return;
                }

                string archivoImagen = Path.ChangeExtension(archivoDot, ".jpg");
                string archivoDotFullPath = Path.GetFullPath(archivoDot);
                string archivoImagenFullPath = Path.GetFullPath(archivoImagen);

                // Detectar sistema operativo y usar la ruta correcta de Graphviz
                string graphvizPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? @"C:\Program Files\Graphviz\bin\dot.exe" // Ruta en Windows
                    : "dot"; // Linux usa "dot" directamente

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = graphvizPath,
                    Arguments = $"-Tjpg \"{archivoDotFullPath}\" -o \"{archivoImagenFullPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process proceso = Process.Start(processStartInfo))
                {
                    if (proceso == null)
                    {
                        Console.WriteLine("No se pudo iniciar el proceso para convertir el archivo .dot.");
                        return;
                    }

                    proceso.WaitForExit();

                    string error = proceso.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Error de Graphviz: {error}");
                    }

                    if (proceso.ExitCode == 0)
                    {
                        Console.WriteLine($"Conversión exitosa. La imagen se guardó en: {archivoImagenFullPath}");
                    }
                    else
                    {
                        Console.WriteLine("Hubo un error al intentar convertir el archivo .dot a imagen.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir el archivo .dot a imagen: {ex.Message}");
            }
        }

    }
}