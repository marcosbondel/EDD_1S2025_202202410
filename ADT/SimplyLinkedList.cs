using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class SimplyLinkedList<T> where T : unmanaged, UserInterface {

        private SimpleNode<T>* head;
        private int size;

        public SimplyLinkedList(){
            head = null;
            size = 0;
        }

        public int GetSize(){
            return size;
        }

        public void insert(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            if (head == null) {
                head = newSimpleNode;
                size++;
            } else {
                SimpleNode<T>* current = head;
                while (current -> next != null) {
                    current = current -> next;
                }
                current -> next = newSimpleNode;
                size++;
            }
        }

        public bool deleteById(int id) {
            if (head == null) return false;

            if ( head->value.GetId() == id ){
                SimpleNode<T>* temp = head;
                head = head->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                return true;
            }
            
            SimpleNode<T>* current = head;
            
            while (current->next != null && current->value.GetId() == id) {
                current = current->next;
            }
            
            if (current->next != null) {
                SimpleNode<T>* temp = current->next;
                current->next = current->next->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }

            return true;
        }

        public SimpleNode<T>* GetById(int id){
            if (head == null) return null;

            SimpleNode<T>* current = head;
            while (current!= null) {
                if (current->value.GetId() == id) {
                    return current;
                }
                current = current->next;
            }
            return null;
        }

        public void list() {
            SimpleNode<T>* current = head;
            Console.WriteLine("------------- Users -------------");
            while (current != null) {
                Console.WriteLine(current->value.ToString());
                current = current->next;
            }
            Console.WriteLine("--------------------------------");
        }

        public unsafe string GenerarGraphviz()
        {
            // Si la lista está vacía, generamos un solo nodo con "NULL"
            if (head == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            // Iniciamos el código Graphviz
            var graphviz = "digraph G {\n";
            graphviz += "    node [shape=record];\n";
            graphviz += "    rankdir=LR;\n";
            graphviz += "    subgraph cluster_0 {\n";
            graphviz += "        label = \"Lista Simple\";\n";

            // Iterar sobre los nodos de la lista y construir la representación Graphviz
            SimpleNode<T>* current = head;
            int index = 0;

            while (current != null)
            {
                graphviz += $"        n{index} [label = \"{{<data> ID: {current->value.GetId()} \\n Name: {current->value.GetFullname()} \\n Email: {current->value.GetEmail()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            }

            // Conectar los nodos
            current = head;
            for (int i = 0; current != null && current->next != null; i++)
            {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n";
                current = current->next;
            }

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }
    }

}