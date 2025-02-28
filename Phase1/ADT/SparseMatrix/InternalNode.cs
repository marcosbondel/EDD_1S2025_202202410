using System;

namespace ADT {
    namespace Matrix
    {
        // Definition of a generic structure InternalNode
        // This structure uses the data type T, which must be an unmanaged type.
        public unsafe struct InternalNode<T> where T : unmanaged
        {
            public T id;               // Identifier of type T. Being generic, it can be any unmanaged type.
            public string name;        // Name of the node, a string value.
            public int coordinateX;     
            public int coordinateY;     
            
            // Pointers to other nodes that form the structure of the matrix
            public InternalNode<T>* up;        // Pointer to the upper node
            public InternalNode<T>* down;      // Pointer to the lower node
            public InternalNode<T>* right;     // Pointer to the next node
            public InternalNode<T>* left;      // Pointer to the previous node
        }
    }
}
