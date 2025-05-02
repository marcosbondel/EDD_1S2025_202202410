using System;
using System.Collections.Generic;
using System.Text;

namespace Graphs
{
    
    public class UnDirectedGraph
    {
        
        private Dictionary<string, List<string>> listaAdyacencia;

        // crea un grafo vacío cuando lo iniciamos.
        public UnDirectedGraph()
        {
            listaAdyacencia = new Dictionary<string, List<string>>();
        }


        public void Insertar(string idVehiculo, string idRepuesto)
        {
            // Verificamos que los IDs no estén vacíos.
            if (string.IsNullOrEmpty(idVehiculo) || string.IsNullOrEmpty(idRepuesto))
            {
                throw new ArgumentException("Los IDs de vehículo y repuesto no pueden estar vacíos.");
            }

        
            // Si el vehículo no está en el diccionario, lo creamos con una lista vacía.
            if (!listaAdyacencia.ContainsKey(idVehiculo))
            {
                listaAdyacencia[idVehiculo] = new List<string>();
            }

            // Solo agregamos el repuesto si no está ya en la lista.
            if (!listaAdyacencia[idVehiculo].Contains(idRepuesto))
            {
                listaAdyacencia[idVehiculo].Add(idRepuesto);
            }

            // Agregamos la conexión de repuesto -> vehículo.
            // Como es no dirigido, hacemos lo mismo en la otra dirección.
            if (!listaAdyacencia.ContainsKey(idRepuesto))
            {
                listaAdyacencia[idRepuesto] = new List<string>();
            }
            if (!listaAdyacencia[idRepuesto].Contains(idVehiculo))
            {
                listaAdyacencia[idRepuesto].Add(idVehiculo);
            }

        }
        


        public string GenerarDot()
        {
            StringBuilder dot = new StringBuilder();

            dot.AppendLine("graph GrafoVehiculosRepuestos {");
            dot.AppendLine("    node [shape=ellipse];"); 
            dot.AppendLine("    graph [rankdir=LR];");  

            //  no repetir conexiones.
            HashSet<string> conexiones = new HashSet<string>();

            // Recorremos todas las claves del diccionario .
            foreach (var nodo in listaAdyacencia)
            {
                string idActual = nodo.Key; 
                List<string> conexionesNodo = nodo.Value; 

        
                foreach (var idConectado in conexionesNodo)
                {
                    // Creamos una clave única para la conexión
                    string claveConexion = idActual.CompareTo(idConectado) < 0
                        ? $"{idActual} -- {idConectado}"
                        : $"{idConectado} -- {idActual}";

                    // la agregamos al texto si no esta repetido
                    if (!conexiones.Contains(claveConexion))
                    {
                        dot.AppendLine($"    \"{idActual}\" -- \"{idConectado}\";");
                        conexiones.Add(claveConexion);
                    }
                }
            }

            dot.AppendLine("}");

            return dot.ToString();
        }
//         public string GenerarDot()
// {
//     StringBuilder dot = new StringBuilder();

//     dot.AppendLine("graph GrafoVehiculosRepuestos {");
//     dot.AppendLine("    node [shape=ellipse];");
//     dot.AppendLine("    graph [rankdir=LR];");
    
//     // Separate vehicles (V) and spare parts (R)
//     List<string> vehiculos = new List<string>();
//     List<string> repuestos = new List<string>();

//     // Classify nodes
//     foreach (var nodo in listaAdyacencia.Keys)
//     {
//         if (nodo.StartsWith("V"))
//         {
//             vehiculos.Add(nodo);
//         }
//         else if (nodo.StartsWith("R"))
//         {
//             repuestos.Add(nodo);
//         }
//     }

//     // Add all vehicles in one rank
//     if (vehiculos.Count > 0)
//     {
//         dot.Append("    { rank=same; ");
//         foreach (var v in vehiculos)
//         {
//             dot.Append($"\"{v}\"; ");
//         }
//         dot.AppendLine("}");
//     }

//     // Add all spare parts in another rank
//     if (repuestos.Count > 0)
//     {
//         dot.Append("    { rank=same; ");
//         foreach (var r in repuestos)
//         {
//             dot.Append($"\"{r}\"; ");
//         }
//         dot.AppendLine("}");
//     }

//     // Track connections to avoid duplicates
//     HashSet<string> conexiones = new HashSet<string>();

//     foreach (var nodo in listaAdyacencia)
//     {
//         string idActual = nodo.Key;
//         List<string> conexionesNodo = nodo.Value;

//         foreach (var idConectado in conexionesNodo)
//         {
//             // Create a unique key for the connection
//             string claveConexion = idActual.CompareTo(idConectado) < 0
//                 ? $"{idActual} -- {idConectado}"
//                 : $"{idConectado} -- {idActual}";

//             if (!conexiones.Contains(claveConexion))
//             {
//                 dot.AppendLine($"    \"{idActual}\" -- \"{idConectado}\";");
//                 conexiones.Add(claveConexion);
//             }
//         }
//     }

//     dot.AppendLine("}");

//     return dot.ToString();
// }
    }
}