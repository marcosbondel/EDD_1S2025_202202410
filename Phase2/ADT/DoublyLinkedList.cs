using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class DoublyLinkedList<T> where T : unmanaged, AutomobileInterface {

        private DoublePointerNode<T>* first;
        private DoublePointerNode<T>* last;
        private int size;

        public DoublyLinkedList(){
            first = null;
            last = null;
            size = 0;
        }

        public int GetSize(){
            return size;
        }

        public DoublePointerNode<T>* GetFirst(){
            return first;
        }

        public void insert(T data){
            DoublePointerNode<T>* newNode = (DoublePointerNode<T>*)Marshal.AllocHGlobal(sizeof(DoublePointerNode<T>));
            *newNode = new DoublePointerNode<T> { value = data, next = null, previous = last };

            if (first == null) {
                first = last = newNode;
            } else {
                last->next = newNode;
                last = newNode;
            }
            size++;
        }
        
        public DoublePointerNode<T>* GetById(int id){
            DoublePointerNode<T>* current = first;
            while (current != null) {
                if (current->value.GetId() == id) 
                    return current;
                current = current->next;
            }
            return null;
        }

        public void List() {
            DoublePointerNode<T>* current = first;
            Console.WriteLine("------------- Automobiles -------------");
            while (current != null) {
                Console.WriteLine(current->value.ToString());
                current = current->next;
            }
            Console.WriteLine("--------------------------------");
        }

        public bool deleteById(int id) {
            if (first == null) return false;

            DoublePointerNode<T>* current = first;

            // Case: deleting first node
            if (first->value.GetId() == id) {
                first = first->next;
                if (first != null) first->previous = null;
                Marshal.FreeHGlobal((IntPtr)current);
                size--;
                return true;
            }

            // Traverse list to find the node to delete
            while (current != null && current->value.GetId() != id) {
                current = current->next;
            }

            // If not found, return false
            if (current == null) return false;

            // If deleting the last node
            if (current == last) {
                last = current->previous;
                last->next = null;
            } else {
                current->previous->next = current->next;
                if (current->next != null) {
                    current->next->previous = current->previous;
                }
            }

            Marshal.FreeHGlobal((IntPtr)current);
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

            DoublePointerNode<T>* current = first;
            int index = 0;

            while (current != null)
            {
                graphviz += $"        n{index} [label = \"{{<prev> Anterior | <data> ID: {current->value.GetId()} \\n ID_Usuario: {current->value.GetUserId()} \\n Marca: {current->value.GetBrand()} \\n Modelo: {current->value.GetModel()} \\n Placa: {current->value.GetPlate()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            }

            current = first;
            for (int i = 0; current != null && current->next != null; i++)
            {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n";
                graphviz += $"        n{i + 1}:prev -> n{i}:data;\n";
                current = current->next;
            }

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }

        public void FreeMemory()
        {
            DoublePointerNode<T>* current = first;
            while (current != null)
            {
                DoublePointerNode<T>* temp = current;
                current = current->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }
            first = last = null;
            size = 0;
        }

        public List<int> ListarVehiculos_Usuario(int idUsuario)
        {
            List<int> listaVehiculos = new List<int>();
            DoublePointerNode<T>* actual = first;
            
            while (actual != null)
            {
                if (actual->value.GetId() == idUsuario)
                {
                    listaVehiculos.Add(actual -> value.GetId());
                }
                actual = actual->next;
            }
            
            return listaVehiculos;
        }
    }
}
