using System;
using System.Runtime.InteropServices;

namespace ADT {

    public unsafe class SimplyLinkedList<T> where T : unmanaged {

        private SimpleNode<T>* head;

        public SimplyLinkedList(){
            head = null;
        }

        public void insert(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            if (head == null) {
                head = newSimpleNode;
            } else {
                SimpleNode<T>* current = head;
                while (current -> next != null) {
                    current = current -> next;
                }
                current -> next = newSimpleNode;
            }
        }
    }

}