using System;

namespace ADT {

    namespace Matrix
    {
        // Generic structure HeaderNode for handling links in a list or matrix
        public unsafe struct HeaderNode<T> where T : unmanaged
        {
            public T id; // Node identifier, of generic type T.
            
            // Pointers to link nodes in both directions (next and previous)
            public HeaderNode<T>* next;
            public HeaderNode<T>* previous; 
            
            // Pointer to an InternalNode, used to access a specific location in the matrix
            public InternalNode<T>* access; // Pointer to the internal node of the matrix.
        }
    }
}
