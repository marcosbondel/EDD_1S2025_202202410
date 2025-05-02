using System;
using ADT;
using Model;
using Trees.AVL;
using Trees.Binary;
using View;
using Blocks;
using Merkle;
using Graphs;
using Structures;

namespace Storage {
    public static class AppData {
        public static Blockchain user_blockchain = new Blockchain();
        public static DoublyLinkedList automobiles_data = new DoublyLinkedList();

        // Phase2
        public static AVLTree spare_parts_data_avl_tree = new AVLTree();
        public static BinaryTree services_data_binary_tree = new BinaryTree();
        public static MerkleTree bills_data_merkle_tree = new MerkleTree();
        public static UnDirectedGraph automobile_spare_parts_graph = new UnDirectedGraph();

        // Counters
        public static int bill_id_counter = 0;
        public static List<LogModel> session_logs_data = new List<LogModel>();
        public static User current_user = null;

        // Compresors
        public static HuffmanCompressor compressor = new HuffmanCompressor();
    }

    public static class AppViews {
        public static DashboardView dashboard_view = null;
        public static AutomobilesView automobile_view = null;
        public static LoginView login_view = null;
        // public static ServicesUserVisualizationView services_user_visualization_view = null;
        public static ServicesView services_view = null;
        public static SparePartsOrderView spare_parts_order_view = null;
        public static SparePartsView spare_parts_view = null;
        // public static UserBillsView user_bills_view = null;
        public static UserDashboardView user_dashboard_view = null;
        public static UsersView users_view = null;
        public static UserAutomobilesView users_automobiles_view = null;

        public static void renderGivenView(string view_name) {
            switch (view_name) {
                case "dashboard":

                    if(dashboard_view == null){
                        dashboard_view = new DashboardView();
                    }

                    dashboard_view.ShowAll();
                    break;
                case "automobiles":
                    
                    if(automobile_view == null){
                        automobile_view = new AutomobilesView();
                    }

                    automobile_view.ShowAll();
                    break;
                case "login":

                    if(login_view == null){
                        login_view = new LoginView();
                    }

                    login_view.ShowAll();
                    break;
                case "services":

                    if(services_view == null){
                        services_view = new ServicesView();
                    }

                    services_view.ShowAll();
                    break;
                case "spare_parts":

                    if(spare_parts_view == null){
                        spare_parts_view = new SparePartsView();
                    }

                    spare_parts_view.ShowAll();
                    break;
                case "spare_parts_order":

                    if(spare_parts_order_view == null){
                        spare_parts_order_view = new SparePartsOrderView();
                    }

                    spare_parts_order_view.ShowAll();
                    break;
                case "user_dashboard":

                    if(user_dashboard_view == null){
                        user_dashboard_view = new UserDashboardView();
                    }

                    user_dashboard_view.ShowAll();
                    break;
                case "users":

                    if(users_view == null){
                        users_view = new UsersView();
                    }

                    users_view.ShowAll();
                    break;
                case "user_automobiles":

                    if(users_view == null){
                        users_automobiles_view = new UserAutomobilesView(AppData.current_user.Id);
                    }

                    users_automobiles_view.ShowAll();
                    break;
                default:
                    Console.WriteLine("Invalid view name.");
                    break;
            }
        }

        public static void hideGivenView(string view_name) {
            switch (view_name) {
                case "dashboard":
                    dashboard_view.Hide();
                    break;
                case "automobiles":
                    automobile_view.Hide();
                    break;
                case "login":
                    login_view.Hide();
                    break;
                case "services":
                    services_view.Hide();
                    break;
                case "spare_parts":
                    spare_parts_view.Hide();
                    break;
                case "spare_parts_order":
                    spare_parts_order_view.Hide();
                    break;
                case "user_dashboard":
                    user_dashboard_view.Hide();
                    break;
                case "users":
                    users_view.Hide();
                    break;
                case "user_automobiles":
                    users_automobiles_view.Hide();
                    break;
                default:
                    Console.WriteLine("Invalid view name.");
                    break;
            }
        }

        public static void ClearAllViews() {
            dashboard_view.Hide();
            automobile_view.Hide();
            login_view.Hide();
            services_view.Hide();
            spare_parts_view.Hide();
            spare_parts_order_view.Hide();
            user_dashboard_view.Hide();
            users_view.Hide();
            users_automobiles_view.Hide();
        }


    }
}