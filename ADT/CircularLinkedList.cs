using System;
using System.Runtime.InteropServices;

namespace ADT {

    public unsafe class CircularLinkedList<T> where T : unmanaged {

        private SimpleNode<T>* first;
        private SimpleNode<T>* last;

        public CircularLinkedList(){
            first = null;
            last = null;
        }

        public void insert(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            if (first == null) {
                first = last = newSimpleNode;
            } else {
                last -> next = newSimpleNode;
                last = newSimpleNode;
            }
        }

        public void list() {
            SimpleNode<T>* current = first;
            Console.WriteLine("------------- Users -------------");
            while (current != null) {
                Console.WriteLine(current->value.ToString());
                current = current->next;
            }
            Console.WriteLine("--------------------------------");
        }
    }

}