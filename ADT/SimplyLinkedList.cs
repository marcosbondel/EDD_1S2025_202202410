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

        public int GetSize(){
            return size;
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

        public bool deleteById(int id) {
            if (head == null) return false;

            if ( head->value.GetId() == id ){
                SimpleNode<T>* temp = head;
                head = head->next;
                Marshal.FreeHGlobal((IntPtr)temp);
                return true;
            }
            
            SimpleNode<T>* current = head;
            
            while (current->next != null && current->value.GetId() == id) {
                current = current->next;
            }
            
            if (current->next != null) {
                SimpleNode<T>* temp = current->next;
                current->next = current->next->next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }

            return true;
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