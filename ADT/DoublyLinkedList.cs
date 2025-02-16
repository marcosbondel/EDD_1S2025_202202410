using System;
using System.Runtime.InteropServices;

namespace ADT {

    public unsafe class DoublyLinkedList<T> where T : unmanaged {

        private DoublePointerNode<T>* first;
        private DoublePointerNode<T>* last;

        public DoublyLinkedList(){
            first = null;
            last = null;
        }

        public void insert(T data){
            DoublePointerNode<T>* newDoublePointerNode = (DoublePointerNode<T>*)Marshal.AllocHGlobal(sizeof(DoublePointerNode<T>));
            *newDoublePointerNode = new DoublePointerNode<T> { value = data, next = null, previous = null };

            if (first == null) {
                first = last= newDoublePointerNode;
            } else {
                last -> next = newDoublePointerNode;
                newDoublePointerNode -> previous = last;
                last = newDoublePointerNode;
            }
        }

        public void list() {
            DoublePointerNode<T>* current = first;
            Console.WriteLine("------------- Users -------------");
            while (current != null) {
                Console.WriteLine(current->value.ToString());
                current = current->next;
            }
            Console.WriteLine("--------------------------------");
        }
    }
}