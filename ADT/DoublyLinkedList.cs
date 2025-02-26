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

        public void insert(T data){
            DoublePointerNode<T>* newDoublePointerNode = (DoublePointerNode<T>*)Marshal.AllocHGlobal(sizeof(DoublePointerNode<T>));
            *newDoublePointerNode = new DoublePointerNode<T> { value = data, next = null, previous = null };

            if (first == null) {
                first = last = newDoublePointerNode;
            } else {
                last -> next = newDoublePointerNode;
                newDoublePointerNode -> previous = last;
                last = newDoublePointerNode;
            }
            size++;
        }
        
        public DoublePointerNode<T>* GetById(int id){

            if(first == null) return null;

            DoublePointerNode<T>* current = first;

            do {
                Console.WriteLine(current->value.ToString());
                if(current->value.GetId() == id) 
                    return current;

                current = current->next;

            } while (current != null);
            
            return null;
        }

        public void list() {
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

            if ( first->value.GetId() == id ){
                DoublePointerNode<T>* temp = first;
                first = first->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                return true;
            }
            
            DoublePointerNode<T>* current = first;
            
            while (current->next != null && current->value.GetId() == id) {
                current = current->next;
            }
            
            if (current->next != null) {
                DoublePointerNode<T>* temp = current->next;
                current->next = current->next->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }

            return true;
        }

        public unsafe string GenerateDotCode()
        {
            // Si la lista está vacía, generamos un solo nodo con "NULL"
            if (first == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            // Iniciamos el código Graphviz
            var graphviz = "digraph G {\n";
            graphviz += "    node [shape=record];\n";
            graphviz += "    rankdir=LR;\n";
            graphviz += "    subgraph cluster_0 {\n";
            graphviz += "        label = \"Lista Doblemente Enlazada\";\n";

            // Iterar sobre los nodos de la lista y construir la representación Graphviz
            DoublePointerNode<T>* current = first;
            int index = 0;

            while (current != null)
            {
                graphviz += $"        n{index} [label = \"{{<prev> Anterior | <data> ID: {current->value.GetId()} \\n ID_Usuario: {current->value.GetUserId()} \\n Marca: {current->value.GetBrand()} \\n Modelo: {current->value.GetModel()} \\n Placa: {current->value.GetPlate()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            }

            // Conectar los nodos hacia adelante (next)
            current = first;
            for (int i = 0; current != null && current->next != null; i++)
            {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n"; // Enlace hacia adelante
                graphviz += $"        n{i + 1}:prev -> n{i}:data;\n"; // Enlace hacia atrás
                current = current->next;
            }

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }

    }
}