using System;
using ADT;
using Model;
using Trees.AVL;

namespace Storage {
    public static class AppData {
        public static SimplyLinkedList<User> users_data = new SimplyLinkedList<User>();
        public static CircularLinkedList<SparePart> spare_parts_data = new CircularLinkedList<SparePart>();
        public static DoublyLinkedList<Automobile> automobiles_data = new DoublyLinkedList<Automobile>();
        public static ADT.Queue<Service> services_data = new ADT.Queue<Service>();
        public static ADT.Pile<Bill> bills_data = new ADT.Pile<Bill>();
        public static ADT.Matrix.SparseMatrix<int> logs_data = new ADT.Matrix.SparseMatrix<int>(0);

        // Phase2
        public static AVLTree spare_parts_data_avl_tree = new AVLTree();
    }
}