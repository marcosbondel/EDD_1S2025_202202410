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
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            if (first == null) {
                first = newSimpleNode;
                first -> next = first;
            } else {
                SimpleNode<T>* current = first;
                while (current -> next != first) {
                    current = current -> next;
                }
                current -> next = newSimpleNode;
                newSimpleNode -> next = first;
            }

            size++;
        }

        public bool deleteById(int id) {
            if (first == null) return false;

            if ( first->value.GetId() == id ){
                SimpleNode<T>* temp = first;
                first = first->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                return true;
            }
            
            SimpleNode<T>* current = first;
            
            while (current->next != null && current->value.GetId() == id && current->next != first) {
                current = current->next;
            }
            
            if (current->next != null) {
                SimpleNode<T>* temp = current->next;
                current->next = current->next->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }

            return true;
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

        public SimpleNode<T>* GetById(int id){
            if (first == null) return null;

            SimpleNode<T>* current = first;
            while (current!= null && current->next != first) {
                if (current->value.GetId() == id) {
                    return current;
                }
                current = current->next;
            }
            return null;
        }
    }

}