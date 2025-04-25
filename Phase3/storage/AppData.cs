using System;
using ADT;
using Model;
using Trees.AVL;
using Trees.Binary;
using View;
using Blocks;

namespace Storage {
    public static class AppData {
        public static Blockchain blockchain = new Blockchain();
        public static DoublyLinkedList automobiles_data = new DoublyLinkedList();

        // Phase2
        public static AVLTree spare_parts_data_avl_tree = new AVLTree();
        public static BinaryTree services_data_binary_tree = new BinaryTree();

        // Counters
        public static int bill_id_counter = 0;
        public static List<LogModel> session_logs_data = new List<LogModel>();
        public static SimpleNode current_user_node = null;
    }

    public static class AppViews {
        public static DashboardView dashboard = new DashboardView();
    }
}