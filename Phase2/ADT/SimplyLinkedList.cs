using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class SimplyLinkedList<T> : IDisposable where T : unmanaged, UserInterface {

        private SimpleNode<T>* head;
        private int size;

        public SimplyLinkedList() {
            head = null;
            size = 0;
        }

        public int GetSize() {
            return size;
        }

        public void insert(T data) {
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            if (head == null) {
                head = newSimpleNode;
            } else {
                SimpleNode<T>* current = head;
                while (current->next != null) {
                    current = current->next;
                }
                current->next = newSimpleNode;
            }
            size++;
        }

        public bool deleteById(int id) {
            if (head == null) return false;

            if (head->value.GetId() == id) {
                SimpleNode<T>* temp = head;
                head = head->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                size--;
                return true;
            }

            SimpleNode<T>* current = head;
            while (current->next != null && current->next->value.GetId() != id) {
                current = current->next;
            }

            if (current->next != null) {
                SimpleNode<T>* temp = current->next;
                current->next = current->next->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                size--;
                return true;
            }
            return false;
        }

        public SimpleNode<T>* GetById(int id) {
            SimpleNode<T>* current = head;
            while (current != null) {
                if (current->value.GetId() == id) {
                    return current;
                }
                current = current->next;
            }
            return null;
        }
        
        public SimpleNode<T>* GetByEmail(string email) {
            SimpleNode<T>* current = head;
            while (current != null) {
                if (current->value.GetEmail() == email) {
                    return current;
                }
                current = current->next;
            }
            return null;
        }

        public bool CheckUserCredentials(string name, string password){
            SimpleNode<T>* current = head;
            while (current != null) {
                if (current->value.GetName() == name && current->value.GetPassword() == password) {
                    return true;
                }
                current = current->next;
            }
            return false;
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

        public string GenerateDotCode() {
            if (head == null) {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            var graphviz = "digraph G {\n    node [shape=record];\n    rankdir=LR;\n    subgraph cluster_0 {\n        label = \"Lista Simple\";\n";

            SimpleNode<T>* current = head;
            int index = 0;
            while (current != null) {
                graphviz += $"        n{index} [label = \"{{<data> ID: {current->value.GetId()} \\\n Name: {current->value.GetFullname()} \\\n Email: {current->value.GetEmail()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            }

            current = head;
            for (int i = 0; current != null && current->next != null; i++) {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n";
                current = current->next;
            }

            graphviz += "    }\n}\n";
            return graphviz;
        }

        public void Dispose() {
            while (head != null) {
                SimpleNode<T>* temp = head;
                head = head->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }
            size = 0;
        }
    }
}
