using System;


namespace ADT {

    public unsafe class SimpleNode<T> {
        public T value;
        public SimpleNode<T>* next;
    }

    public unsafe struct DoublePointerNode<T> {
        public T Data;
        public DoublePointerNode<T>* Next;
        public DoublePointerNode<T>* Previous;
    }
}