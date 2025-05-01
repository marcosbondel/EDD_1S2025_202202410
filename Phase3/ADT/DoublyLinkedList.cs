using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public class DoublyLinkedList{

        private DoublePointerNode first;
        private DoublePointerNode last;
        private int size;

        public DoublyLinkedList(){
            first = null;
            last = null;
            size = 0;
        }

        public int GetSize(){
            return size;
        }

        public DoublePointerNode GetFirst(){
            return first;
        }

        public void insert(Automobile data){
            DoublePointerNode newNode = new DoublePointerNode(data);

            if (first == null) {
                first = last = newNode;
            } else {
                last.next = newNode;
                last = newNode;
            }
            size++;
        }
        
        public DoublePointerNode GetById(int id){
            DoublePointerNode current = first;
            while (current != null) {
                if (current.value.Id == id) 
                    return current;
                current = current.next;
            }
            return null;
        }

        public void List() {
            DoublePointerNode current = first;
            Console.WriteLine("------------- Automobiles -------------");
            while (current != null) {
                Console.WriteLine(current.value.ToString());
                current = current.next;
            }
            Console.WriteLine("--------------------------------");
        }

        public bool deleteById(int id) {
            if (first == null) return false;

            DoublePointerNode current = first;

            // Case: deleting first node
            if (first.value.Id == id) {
                first = first.next;
                if (first != null) first.previous = null;

                size--;
                return true;
            }

            // Traverse list to find the node to delete
            while (current != null && current.value.Id != id) {
                current = current.next;
            }

            // If not found, return false
            if (current == null) return false;

            // If deleting the last node
            if (current == last) {
                last = current.previous;
                last.next = null;
            } else {
                current.previous.next = current.next;
                if (current.next != null) {
                    current.next.previous = current.previous;
                }
            }

            size--;
            return true;
        }
        

        public string GenerateDotCode()
        {
            if (first == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            var graphviz = "digraph G {\n";
            graphviz += "    node [shape=record];\n";
            graphviz += "    rankdir=LR;\n";
            graphviz += "    subgraph cluster_0 {\n";
            graphviz += "        label = \"Lista Doblemente Enlazada\";\n";

            DoublePointerNode current = first;
            int index = 0;

            while (current != null)
            {
                graphviz += $"        n{index} [label = \"{{<prev> Anterior | <data> ID: {current.value.Id} \\n ID_Usuario: {current.value.Id} \\n Marca: {current.value.Brand} \\n Modelo: {current.value.Model} \\n Placa: {current.value.Plate} | <next> Siguiente }}\"];\n";
                current = current.next;
                index++;
            }

            current = first;
            for (int i = 0; current != null && current.next != null; i++)
            {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n";
                graphviz += $"        n{i + 1}:prev -> n{i}:data;\n";
                current = current.next;
            }

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }

        public List<int> ListarVehiculos_Usuario(int idUsuario)
        {
            List<int> listaVehiculos = new List<int>();
            DoublePointerNode actual = first;
            
            while (actual != null)
            {
                if (actual.value.Id == idUsuario)
                {
                    listaVehiculos.Add(actual.value.Id);
                }
                actual = actual.next;
            }
            
            return listaVehiculos;
        }

        public void GenerateFile()
        {
            string ruta = "./reports/automobiles.txt"; 
            try
            {
                string? directorio = Path.GetDirectoryName(ruta);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                using (StreamWriter writer = new StreamWriter(ruta, false))
                {
                    DoublePointerNode? actual = first;
                    while (actual != null)
                    {
                        writer.WriteLine($"{actual.value.Id},{actual.value.UserId},{actual.value.Brand},{actual.value.Model},{actual.value.Plate}");
                        actual = actual.next;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el archivo: {ex.Message}");
            }
        }
    }
}
