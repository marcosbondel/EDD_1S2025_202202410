using System;
using ADT;
using Model;
using Trees.AVL;
using Trees.B;
using Trees.Binary;

namespace Storage {
    public static unsafe class AppData {
        public static SimplyLinkedList users_data = new SimplyLinkedList();
        public static DoublyLinkedList automobiles_data = new DoublyLinkedList();

        // Phase2
        public static AVLTree spare_parts_data_avl_tree = new AVLTree();
        public static BinaryTree services_data_binary_tree = new BinaryTree();
        public static BTree bills_data_b_tree = new BTree();

        // Counters
        public static int bill_id_counter = 0;
        public static List<LogModel> session_logs_data = new List<LogModel>();
        public static SimpleNode current_user_node = null;
    }
}