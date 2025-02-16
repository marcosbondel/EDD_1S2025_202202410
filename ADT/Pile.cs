using System;
using System.Runtime.InteropServices;

namespace ADT {

    public unsafe class Pile<T> where T : unmanaged {

        private SimpleNode<T>* top;

        public Pile(){
            top = null;
        }

        public void push(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            newSimpleNode -> next = top;
            top = newSimpleNode;

        }

        public SimpleNode<T>* pop(T data){

            if (top == null) return null;

            SimpleNode<T>* temp = top;
            top = top -> next;
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
    }

}