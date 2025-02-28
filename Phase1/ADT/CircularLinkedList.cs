using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class CircularLinkedList<T> where T : unmanaged, SparePartInterface {

        private SimpleNode<T>* first;
        private int size;

        public CircularLinkedList(){
            first = null;
            size = 0;
        }

        public int GetSize(){
            return size;
        }

        public void insert(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            if (first == null) {
                first = newSimpleNode;
                first -> next = first;
                size++;
            } else {
                SimpleNode<T>* current = first;
                while (current->next != first) {
                    current = current -> next;
                }
                current -> next = newSimpleNode;
                newSimpleNode -> next = first;
                size++;
            }

        }

        public bool deleteById(int id) {
            if (first == null) return false;

            if ( first->value.GetId() == id ){
                SimpleNode<T>* temp = first;
                first = first->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                return true;
            }
            
            SimpleNode<T>* current = first;
            
            while (current->next != null && current->value.GetId() == id && current->next != first) {
                current = current->next;
            }
            
            if (current->next != null) {
                SimpleNode<T>* temp = current->next;
                current->next = current->next->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }

            return true;
        }

        public void list() {
            SimpleNode<T>* current = first;

            Console.WriteLine("------------- Spare parts -------------");
            do{
              
              if(current == null) return;

              Console.WriteLine(current->value.ToString());
              current = current->next;
                
            } while (current != first);
            Console.WriteLine("--------------------------------");
        }

        public SimpleNode<T>* GetById(int id){
            if(first == null) return null;

            SimpleNode<T>* current = first;

            do{

                if(current->value.GetId() == id){
                    return current;
                }

                current = current->next;

            } while(current != first);

            return null;
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
            graphviz += "        label = \"Lista Circular\";\n";

            // Iterar sobre los nodos de la lista y construir la representación Graphviz
            SimpleNode<T>* current = first;
            // SimpleNode<T>* first = first; // Guardamos la referencia al primer nodo
            int index = 0;

            do
            {
                graphviz += $"        n{index} [label = \"{{<data> ID: {current->value.GetId()} \\n Repuesto: {current->value.GetName()} \\n Detalle: {current->value.GetDetails()} \\n Costo: {current->value.GetCost()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            } while (current != first); // Continuamos hasta completar el ciclo

            // Conectar los nodos en la lista circular
            current = first;
            for (int i = 0; i < index - 1; i++)
            {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n"; // Enlace al siguiente nodo
                current = current->next;
            }

            // Conectar el último nodo de vuelta al primero (circularidad)
            graphviz += $"        n{index - 1}:next -> n0:data;\n";

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }


    }

}