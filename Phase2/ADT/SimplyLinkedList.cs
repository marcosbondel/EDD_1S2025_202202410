using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class SimplyLinkedList {

        private SimpleNode head;
        private int size;

        public SimplyLinkedList() {
            head = null;
            size = 0;
        }

        public int GetSize() {
            return size;
        }

        public void insert(UserModel data) {
            SimpleNode newSimpleNode = new SimpleNode(data);

            if (head == null) {
                head = newSimpleNode;
            } else {
                SimpleNode current = head;
                while (current.next != null) {
                    current = current.next;
                }
                current.next = newSimpleNode;
            }
            size++;
        }

        public bool deleteById(int id) {
            if (head == null) return false;

            if (head.value.Id == id) {
                SimpleNode temp = head;
                head = head.next;
                size--;
                return true;
            }

            SimpleNode current = head;
            while (current.next != null && current.next.value.Id != id) {
                current = current.next;
            }

            if (current.next != null) {
                SimpleNode temp = current.next;
                current.next = current.next.next;
                size--;
                return true;
            }
            return false;
        }

        public SimpleNode GetById(int id) {
            SimpleNode current = head;
            while (current != null) {
                if (current.value.Id == id) {
                    return current;
                }
                current = current.next;
            }
            return null;
        }
        
        public SimpleNode GetByEmail(string email) {
            SimpleNode current = head;
            while (current != null) {
                if (current.value.Email == email) {
                    return current;
                }
                current = current.next;
            }
            return null;
        }

        public bool CheckUserCredentials(string email, string password){
            SimpleNode current = head;
            while (current != null) {
                if (current.value.Email == email && current.value.Password == password) {
                    return true;
                }
                current = current.next;
            }
            return false;
        }

        public void list() {
            SimpleNode current = head;
            Console.WriteLine("------------- Users -------------");
            while (current != null) {
                Console.WriteLine(current.value.ToString());
                current = current.next;
            }
            Console.WriteLine("--------------------------------");
        }

        public string GenerateDotCode() {
            if (head == null) {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            var graphviz = "digraph G {\n    node [shape=record];\n    rankdir=LR;\n    subgraph cluster_0 {\n        label = \"Lista Simple\";\n";

            SimpleNode current = head;
            int index = 0;
            while (current != null) {
                graphviz += $"        n{index} [label = \"{{<data> ID: {current.value.Id} \\\n Name: {current.value.GetFullname()} \\\n Email: {current.value.Email} | <next> Siguiente }}\"];\n";
                current = current.next;
                index++;
            }

            current = head;
            for (int i = 0; current != null && current.next != null; i++) {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n";
                current = current.next;
            }

            graphviz += "    }\n}\n";
            return graphviz;
        }

    }
}
