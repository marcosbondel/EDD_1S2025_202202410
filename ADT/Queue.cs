using System;
using System.Runtime.InteropServices;

namespace ADT {

    public unsafe class Queue<T> where T : unmanaged {

        private SimpleNode<T>* head;
        private SimpleNode<T>* tail;

        public Queue(){
            head = null;
            tail = null;
        }

        public void push(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            newSimpleNode -> next = head;
            head = newSimpleNode;

        }

        public SimpleNode<T>* pop(T data){

            if (head == null) return null;

            SimpleNode<T>* temp = head;
            head = head -> next;
            return head;
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
    }

}