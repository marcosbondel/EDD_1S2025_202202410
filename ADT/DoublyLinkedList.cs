using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class DoublyLinkedList<T> where T : unmanaged, AutomobileInterface {

        private DoublePointerNode<T>* first;
        private DoublePointerNode<T>* last;
        private int size;

        public DoublyLinkedList(){
            first = null;
            last = null;
            size = 0;
        }

        public int GetSize(){
            return size;
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
        
        public DoublePointerNode<T>* GetById(int id){
            if (first == null) return null;

            DoublePointerNode<T>* current = first;
            while (current!= null) {
                if (current->value.GetId() == id) {
                    return current;
                }
                current = current->next;
            }
            return null;
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

        public bool deleteById(int id) {
            if (first == null) return false;

            if ( first->value.GetId() == id ){
                DoublePointerNode<T>* temp = first;
                first = first->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                return true;
            }
            
            DoublePointerNode<T>* current = first;
            
            while (current->next != null && current->value.GetId() == id) {
                current = current->next;
            }
            
            if (current->next != null) {
                DoublePointerNode<T>* temp = current->next;
                current->next = current->next->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }

            return true;
        }
    }
}