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
    }

}