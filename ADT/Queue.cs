using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class Queue<T> where T : unmanaged, ServiceInterface {

        private SimpleNode<T>* head;
        private SimpleNode<T>* tail;
        private int size;

        public Queue(){
            head = null;
            tail = null;
            size = 0;
        }

        public int GetSize(){
            return size;
        }

        public void push(T data){
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            newSimpleNode -> next = head;
            head = newSimpleNode;
            size++;
        }

        public SimpleNode<T>* pop(T data){

            if (head == null) return null;

            SimpleNode<T>* temp = head;
            head = head -> next;
            size--;

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

    }

}