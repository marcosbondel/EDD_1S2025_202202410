using System;
using Model;
using System.Text;

namespace Trees.Binary {
   public class BinaryTree
    {


        private BinaryNode raiz;



        public BinaryTree()
        {
            raiz = null;
        }





        public void Insertar(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
        {
            // Creamos un nuevo nodo con los parámetros recibidos
            BinaryNode nuevo = new BinaryNode(new ServiceModel(id, idRepuesto, idVehiculo, detalles, costo));

            // Si el árbol está vacío (raíz es null), el nuevo nodo se convierte en la raíz
            if (raiz == null)
            {
                raiz = nuevo;
            }
            else
            {
                // Si el árbol no está vacío, llamamos al método recursivo para insertar el nodo
                InsertarRecursivo(raiz, nuevo);
            }
        }

        // Método privado y recursivo que inserta el nodo en el lugar correcto del árbol
        private void InsertarRecursivo(BinaryNode actual, BinaryNode nuevo)
        {
            // Si el id del nuevo nodo es menor que el id del nodo actual, debe ir en el subárbol izquierdo
            if (nuevo.Value.Id < actual.Value.Id)
            {
                // Si no hay nodo en la izquierda, insertamos el nuevo nodo allí
                if (actual.Left == null)
                {
                    actual.Left = nuevo;
                }
                else
                {
                    // Si ya hay un nodo en la izquierda, llamamos recursivamente para insertar en el subárbol izquierdo
                    InsertarRecursivo(actual.Left, nuevo);
                }
            }
            else
            {
                // Si el id del nuevo nodo es mayor o igual al del nodo actual, debe ir en el subárbol derecho
                if (actual.Right == null)
                {
                    actual.Right = nuevo;
                }
                else
                {
                    // Si ya hay un nodo en la derecha, llamamos recursivamente para insertar en el subárbol derecho
                    InsertarRecursivo(actual.Right, nuevo);
                }
            }
        }







        // Método para generar el archivo .dot para Graphviz
        public string GraficarGraphviz()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph BST {");
            sb.AppendLine("    node [shape=rectangle];");
            if (raiz != null)
            {
                GenerarDotRecursivo(raiz, sb);
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarDotRecursivo(BinaryNode nodo, StringBuilder sw)
        {
            if (nodo != null)
            {
                // Formato del nodo con toda la información
                string nodoLabel = $"\"{nodo.Value.Id}\" [label=\"Id: {nodo.Value.Id}\\nRepuesto: {nodo.Value.SparePartId}\\nVehiculo: {nodo.Value.AutomobileId}\\nDetalles: {nodo.Value.Details}\\nCosto: {nodo.Value.Cost}\"]";
                sw.AppendLine($"    {nodoLabel};");

                // Conexiones con hijos
                if (nodo.Left != null)
                {
                    sw.AppendLine($"    \"{nodo.Value.Id}\" -> \"{nodo.Left.Value.Id}\";");
                    GenerarDotRecursivo(nodo.Left, sw);
                }
                if (nodo.Right != null)
                {
                    sw.AppendLine($"    \"{nodo.Value.Id}\" -> \"{nodo.Right.Value.Id}\";");
                    GenerarDotRecursivo(nodo.Right, sw);
                }
            }
        }




        // Recorridos 

        // Recorrido InOrden (izquierda, raíz, derecha)
        public List<BinaryNode> TablaInOrden()
        {
            List<BinaryNode> resultado = new List<BinaryNode>();
            InOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void InOrdenRecursivo(BinaryNode nodo, List<BinaryNode> resultado)
        {
            if (nodo != null)
            {
                InOrdenRecursivo(nodo.Left, resultado);
                resultado.Add(nodo);
                InOrdenRecursivo(nodo.Right, resultado);
            }
        }

        // Recorrido PreOrden (raíz, izquierda, derecha)
        public List<BinaryNode> TablaPreOrden()
        {
            List<BinaryNode> resultado = new List<BinaryNode>();
            PreOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void PreOrdenRecursivo(BinaryNode nodo, List<BinaryNode> resultado)
        {
            if (nodo != null)
            {
                resultado.Add(nodo);
                PreOrdenRecursivo(nodo.Left, resultado);
                PreOrdenRecursivo(nodo.Right, resultado);
            }
        }

        // Recorrido PostOrden (izquierda, derecha, raíz)
        public List<BinaryNode> TablaPostOrden()
        {
            List<BinaryNode> resultado = new List<BinaryNode>();
            PostOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void PostOrdenRecursivo(BinaryNode nodo, List<BinaryNode> resultado)
        {
            if (nodo != null)
            {
                PostOrdenRecursivo(nodo.Left, resultado);
                PostOrdenRecursivo(nodo.Right, resultado);
                resultado.Add(nodo);
            }
        }


        public ServiceModel BuscarPorId(int id)
        {
            BinaryNode nodoEncontrado = BuscarPorIdRecursivo(raiz, id);
            return nodoEncontrado?.Value;
        }

        // Método privado recursivo para buscar por ID
        private BinaryNode BuscarPorIdRecursivo(BinaryNode nodo, int id)
        {
            // Caso base: nodo es null o encontramos el ID
            if (nodo == null || nodo.Value.Id == id)
            {
                return nodo;
            }

            // Si el ID buscado es menor que el ID del nodo actual, buscamos en el subárbol izquierdo
            if (id < nodo.Value.Id)
            {
                return BuscarPorIdRecursivo(nodo.Left, id);
            }
            // Si el ID buscado es mayor, buscamos en el subárbol derecho
            else
            {
                return BuscarPorIdRecursivo(nodo.Right, id);
            }
        }



        // TablaInOrden_Vehiculos
        public List<BinaryNode> TablaInOrden_Vehiculos(List<int> idsVehiculos)
        {
            List<BinaryNode> resultado = new List<BinaryNode>();
            InOrdenVehiculosRecursivo(raiz, resultado, idsVehiculos);
            return resultado;
        }

        private void InOrdenVehiculosRecursivo(BinaryNode nodo, List<BinaryNode> resultado, List<int> idsVehiculos)
        {
            if (nodo != null)
            {
                InOrdenVehiculosRecursivo(nodo.Left, resultado, idsVehiculos);
                if (idsVehiculos.Contains(nodo.Value.AutomobileId))
                    resultado.Add(nodo);
                InOrdenVehiculosRecursivo(nodo.Right, resultado, idsVehiculos);
            }
        }




        // TablaPreOrden_Vehiculos
        public List<BinaryNode> TablaPreOrden_Vehiculos(List<int> idsVehiculos)
        {
            List<BinaryNode> resultado = new List<BinaryNode>();
            PreOrdenVehiculosRecursivo(raiz, resultado, idsVehiculos);
            return resultado;
        }

        private void PreOrdenVehiculosRecursivo(BinaryNode nodo, List<BinaryNode> resultado, List<int> idsVehiculos)
        {
            if (nodo != null)
            {
                if (idsVehiculos.Contains(nodo.Value.AutomobileId))
                    resultado.Add(nodo);
                PreOrdenVehiculosRecursivo(nodo.Left, resultado, idsVehiculos);
                PreOrdenVehiculosRecursivo(nodo.Right, resultado, idsVehiculos);
            }
        }



        // TablaPostOrden_Vehiculos
        public List<BinaryNode> TablaPostOrden_Vehiculos(List<int> idsVehiculos)
        {
            List<BinaryNode> resultado = new List<BinaryNode>();
            PostOrdenVehiculosRecursivo(raiz, resultado, idsVehiculos);
            return resultado;
        }

        private void PostOrdenVehiculosRecursivo(BinaryNode nodo, List<BinaryNode> resultado, List<int> idsVehiculos)
        {
            if (nodo != null)
            {
                PostOrdenVehiculosRecursivo(nodo.Left, resultado, idsVehiculos);
                PostOrdenVehiculosRecursivo(nodo.Right, resultado, idsVehiculos);
                if (idsVehiculos.Contains(nodo.Value.AutomobileId))
                    resultado.Add(nodo);
            }
        }



        // Servicios_Vehiculos
        public List<int> Servicios_Vehiculos(List<int> idsVehiculos)
        {
            List<int> resultado = new List<int>();
            InOrdenVehiculosRecursivoIds(raiz, resultado, idsVehiculos);
            return resultado;
        }

        private void InOrdenVehiculosRecursivoIds(BinaryNode nodo, List<int> resultado, List<int> idsVehiculos)
        {
            if (nodo != null)
            {
                InOrdenVehiculosRecursivoIds(nodo.Left, resultado, idsVehiculos);
                if (idsVehiculos.Contains(nodo.Value.AutomobileId))
                    resultado.Add(nodo.Value.Id);
                InOrdenVehiculosRecursivoIds(nodo.Right, resultado, idsVehiculos);
            }
        }



    }
}