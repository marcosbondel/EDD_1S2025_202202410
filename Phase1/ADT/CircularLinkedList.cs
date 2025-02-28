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
            SimpleNode<T>* newNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newNode = new SimpleNode<T> { value = data, next = null };

            if (first == null) {
                first = newNode;
                first->next = first; // Point to itself (circular)
            } else {
                SimpleNode<T>* current = first;
                while (current->next != first) {
                    current = current->next;
                }
                current->next = newNode;
                newNode->next = first; // Maintain circular structure
            }
            size++;
        }

        public bool deleteById(int id) {
            if (first == null) return false;

            SimpleNode<T>* current = first;
            SimpleNode<T>* previous = null;

            do {
                if (current->value.GetId() == id) {
                    if (previous == null) { // Deleting the first node
                        if (first->next == first) { // Only one node in the list
                            Marshal.FreeHGlobal((IntPtr)first);
                            first = null;
                        } else {
                            SimpleNode<T>* last = first;
                            while (last->next != first) {
                                last = last->next;
                            }
                            first = first->next;
                            last->next = first;
                            Marshal.FreeHGlobal((IntPtr)current);
                        }
                    } else { // Deleting a middle or last node
                        previous->next = current->next;
                        Marshal.FreeHGlobal((IntPtr)current);
                    }
                    size--;
                    return true;
                }
                previous = current;
                current = current->next;
            } while (current != first);

            return false; // ID not found
        }

        public void list() {
            if (first == null) return; // Prevent infinite loop

            SimpleNode<T>* current = first;
            Console.WriteLine("------------- Spare parts -------------");

            do {
                Console.WriteLine(current->value.ToString());
                current = current->next;
            } while (current != first);

            Console.WriteLine("--------------------------------");
        }

        public SimpleNode<T>* GetById(int id){
            if (first == null) return null;

            SimpleNode<T>* current = first;
            do {
                if (current->value.GetId() == id) {
                    return current;
                }
                current = current->next;
            } while (current != first);

            return null;
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
            graphviz += "        label = \"Lista Circular\";\n";

            SimpleNode<T>* current = first;
            int index = 0;

            do {
                graphviz += $"        n{index} [label = \"{{<data> ID: {current->value.GetId()} \\n Repuesto: {current->value.GetName()} \\n Detalle: {current->value.GetDetails()} \\n Costo: {current->value.GetCost()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            } while (current != first);

            // Connect nodes
            current = first;
            for (int i = 0; i < index - 1; i++) {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n";
                current = current->next;
            }
            graphviz += $"        n{index - 1}:next -> n0:data;\n"; // Circular link

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }

        public void FreeMemory()
        {
            if (first == null) return;

            SimpleNode<T>* current = first;
            SimpleNode<T>* temp;

            do {
                temp = current;
                current = current->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            } while (current != first);

            first = null;
            size = 0;
        }
    }
}
