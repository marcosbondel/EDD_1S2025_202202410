using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class Pile<T> where T : unmanaged, BillInterface {

        private SimpleNode<T>* top;
        private int size;

        public Pile(){
            top = null;
            size = 0;
        }

        public int GetSize(){
            return size;
        }

        public SimpleNode<T>* GetTop(){
            return top;
        }

        public void push(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            newSimpleNode -> next = top;
            top = newSimpleNode;
            size++;
        }

        public SimpleNode<T>* pop(){

            if (top == null) return null;

            SimpleNode<T>* temp = top;
            top = top -> next;
            size--;

            return top;
        }

        public void list() {
            SimpleNode<T>* current = top;
            Console.WriteLine("------------- Users -------------");
            while (current != null) {
                Console.WriteLine(current->value.ToString());
                current = current->next;
            }
            Console.WriteLine("--------------------------------");
        }

        public unsafe string GenerateDotFile()
        {
            // Si la pila está vacía, generamos un solo nodo con "NULL"
            if (top == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            // Iniciamos el código Graphviz
            var graphviz = "digraph G {\n";
            graphviz += "    node [shape=record];\n";
            graphviz += "    rankdir=TB;\n"; // De arriba hacia abajo
            graphviz += "    subgraph cluster_0 {\n";
            graphviz += "        label = \"Pila\";\n";

            // Iterar sobre los nodos de la pila y construir la representación Graphviz
            SimpleNode<T>* current = top;
            int index = 0;

            while (current != null)
            {
                // graphviz += $"        n{index} [label = \"{{<data> ID: {current->value.GetId()} \\n Name: {current->value.GetFullname()} \\n Email: {current->value.GetEmail()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            }

            // Conectar los nodos de la pila (de arriba hacia abajo)
            current = top;
            for (int i = 0; current != null && current->next != null; i++)
            {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n"; // Conexión hacia abajo
                current = current->next;
            }

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }

    }

}