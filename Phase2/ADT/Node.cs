using System;


namespace ADT {

    public unsafe class SimpleNode<T> {
        public T value;
        public SimpleNode<T>* next;
    }

    public unsafe struct DoublePointerNode<T> {
        public T value;
        public DoublePointerNode<T>* next;
        public DoublePointerNode<T>* previous;
    }
}