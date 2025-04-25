using System;
using System.Collections.Generic;
using System.Linq;
using ADT;
using Gtk;
using Storage;
using Utils;
using Model;
using System.Text.Json;
using Trees.Binary;

namespace View {

    public class UserDashboardView : Window
    {

        private LogModel logModel;
        public UserDashboardView() : base("UserDashboardView")
        {
            SetDefaultSize(900, 300);
            SetPosition(WindowPosition.Center);

            // Main container (horizontal box)
            Box mainBox = new Box(Orientation.Horizontal, 0);

            // Sidebar (vertical box)
            Box sidebar = new Box(Orientation.Vertical, 10);
            sidebar.Margin = 10;

            // Sidebar Buttons
            Button usersButton = new Button("Users");
            Button automobilesButton = new Button("Automobiles");
            Button sparePartsButton = new Button("Spare Parts");
            Button servicesButton = new Button("Services");
            Button billsButton = new Button("Bills");
            Button showLogsReportButton = new Button("Show Logs Report");
            Button sessionsLogsButton = new Button("Session Logs Report");
            Button showTopOlderAutomobilesButton = new Button("Show Top 5 - Older Automobiles");
            Button showTopAutomobilesServicesButton = new Button("Show Top 5 - Automobiles Services");
            Button logoutButton = new Button("Logout");

            // Attach event handlers
            usersButton.Clicked += OnUsersClicked;
            servicesButton.Clicked += OnServicesClicked;
            billsButton.Clicked += OnBillsClicked;
            sparePartsButton.Clicked += OnSparePartsClicked;
            automobilesButton.Clicked += OnAutomobilesClicked;
            // showLogsReportButton.Clicked += OnShowLogsReportClicked;
            sessionsLogsButton.Clicked += OnShowSessionLogsReportClicked;
            // showTopOlderAutomobilesButton.Clicked += OnShowTopOlderAutomobilesCliked;
            // showTopAutomobilesServicesButton.Clicked += OnShowTopAutomobilesServicesCliked;
            logoutButton.Clicked += OnLogoutClicked;

            // Add buttons to sidebar
            // sidebar.PackStart(usersButton, false, false, 5);
            // sidebar.PackStart(sparePartsButton, false, false, 5);
            sidebar.PackStart(automobilesButton, false, false, 5);
            sidebar.PackStart(servicesButton, false, false, 5);
            sidebar.PackStart(billsButton, false, false, 5);
            sidebar.PackStart(logoutButton, false, false, 5);

            // Main content area (placeholder label)
            Label contentLabel = new Label("Welcome to the USER Dashboard");

            // Add sidebar and content area to main box
            mainBox.PackStart(sidebar, false, false, 10);
            mainBox.PackStart(contentLabel, true, true, 10);

            // Add main box to the window
            Add(mainBox);
            ShowAll();
        }

        // Event Handlers for Sidebar Buttons
        private void OnUsersClicked(object sender, EventArgs e){
            UsersView usersView = new UsersView();
            usersView.ShowAll();
            this.Hide();
        }

        private void OnSparePartsClicked(object sender, EventArgs e){
            SparePartsView onSparePartsView = new SparePartsView();
            onSparePartsView.ShowAll();
            this.Hide();
        }
        
        private void OnAutomobilesClicked(object sender, EventArgs e){
            AutomobilesView onAutomobilesView = new AutomobilesView();
            onAutomobilesView.ShowAll();
            this.Hide();
        }

        private void OnServicesClicked(object sender, EventArgs e)
        {
            int idUsuario = AppData.current_user_node.value.Id;
            List<int> List_Ids_vehiculos = AppData.automobiles_data.ListarVehiculos_Usuario(idUsuario);

            List<BinaryNode> List_Servicios_Usuarios_InOrden = AppData.services_data_binary_tree.TablaInOrden_Vehiculos(List_Ids_vehiculos);
            List<BinaryNode> List_Servicios_Usuarios_PreOrden = AppData.services_data_binary_tree.TablaPreOrden_Vehiculos(List_Ids_vehiculos);
            List<BinaryNode> List_Servicios_Usuarios_PostOrden = AppData.services_data_binary_tree.TablaPostOrden_Vehiculos(List_Ids_vehiculos);

            ServicesUserVisualizationView onServicesView = new ServicesUserVisualizationView(List_Servicios_Usuarios_InOrden, List_Servicios_Usuarios_PreOrden, List_Servicios_Usuarios_PostOrden);
            onServicesView.ShowAll();
            this.Hide();
        }

        private void OnBillsClicked(object sender, EventArgs e)
        {
            int idUsuario = AppData.current_user_node.value.Id;
            List<int> List_Ids_vehiculos = AppData.automobiles_data.ListarVehiculos_Usuario(idUsuario);
            List<int> Lista_Ids_Servicios = AppData.services_data_binary_tree.Servicios_Vehiculos(List_Ids_vehiculos);
            List<BillModel> Lista_Facturas_Usuario = AppData.bills_data_b_tree.ObtenerFacturasPorServicios(Lista_Ids_Servicios);

            UserBillsView billsView = new UserBillsView(Lista_Facturas_Usuario);
            billsView.ShowAll();
            this.Hide();
        }
        
        private void OnShowSessionLogsReportClicked(object sender, EventArgs e)
        {
            // Convert the list to JSON
            string json = JsonSerializer.Serialize(AppData.session_logs_data, new JsonSerializerOptions { WriteIndented = true });

            // Define file path
            string filePath = "logs.json";

            // Write JSON to a file
            File.WriteAllText(filePath, json);

            MSDialog.ShowMessageDialog(this, "Session Logs", "Session Logs Report has been generated successfully!", MessageType.Info);
        }
        
        private void OnLogoutClicked(object sender, EventArgs e)
        {
            // Validation, if the current user is null, it means the admin user is using the app
            if(AppData.current_user_node != null){
                // Before the user leaves the app, we must log the event
                AppData.session_logs_data[^1].loggedOutAt = DateTime.Now.ToString();
                
                // Then we clean the session variables
                AppData.current_user_node = null;
            }
            
            // The user leaves the app
            LoginView loginView = new LoginView();
            loginView.ShowAll();
            this.Hide();
        }
        

    }
} 
