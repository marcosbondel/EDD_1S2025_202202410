using System;
using ADT;
using Model;
using Trees.AVL;
using Trees.B;
using Trees.Binary;

namespace Storage {
    public static unsafe class AppData {
        public static SimplyLinkedList<User> users_data = new SimplyLinkedList<User>();
        public static CircularLinkedList<SparePart> spare_parts_data = new CircularLinkedList<SparePart>();
        public static DoublyLinkedList<Automobile> automobiles_data = new DoublyLinkedList<Automobile>();
        public static ADT.Queue<Service> services_data = new ADT.Queue<Service>();
        public static ADT.Pile<Bill> bills_data = new ADT.Pile<Bill>();
        public static ADT.Matrix.SparseMatrix<int> logs_data = new ADT.Matrix.SparseMatrix<int>(0);

        // Phase2
        public static AVLTree spare_parts_data_avl_tree = new AVLTree();
        public static BinaryTree services_data_binary_tree = new BinaryTree();
        public static BTree bills_data_b_tree = new BTree();

        // Counters
        public static int bill_id_counter = 0;
        public static List<LogModel> session_logs_data = new List<LogModel>();
        public static SimpleNode<User>* current_user_node = null;
    }
}