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

    unsafe class UserDashboardView : Window
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
            // billsButton.Clicked += OnBillsClicked;
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
            // sidebar.PackStart(showLogsReportButton, false, false, 5);
            // sidebar.PackStart(sessionsLogsButton, false, false, 5);
            // sidebar.PackStart(showTopOlderAutomobilesButton, false, false, 5);
            // sidebar.PackStart(showTopAutomobilesServicesButton, false, false, 5);
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
            int idUsuario = AppData.current_user_node->value.GetId();
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
            BillsView billsView = new BillsView();
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
        
        private void OnShowLogsReportClicked(object sender, EventArgs e)
        {
            string dotCode = AppData.logs_data.GenerateDotCode();
            ReportGenerator.GenerateDotFile("Logs", dotCode);
            ReportGenerator.ParseDotToImage("Logs.dot");

            MSDialog.ShowMessageDialog(this, "Report", "Report has been generated successfully!", MessageType.Info);
        }
        
        private unsafe void OnShowTopOlderAutomobilesCliked(object sender, EventArgs e)
        {

            if(AppData.automobiles_data.GetSize() == 0){
                MSDialog.ShowMessageDialog(this, "Error", "No automobiles available to generate report!", MessageType.Error);
                return;
            }

            Dictionary<int, int> automobilesModelDict = new Dictionary<int, int>();
            DoublePointerNode<Automobile>* current = AppData.automobiles_data.GetFirst();

            for (int i = 0; i < AppData.automobiles_data.GetSize(); i++)
            {
                automobilesModelDict.Add(current->value.GetId(), Int32.Parse(current->value.GetModel()));
                current = current -> next;
            }

            var top5 = automobilesModelDict.OrderBy(entry => entry.Value).Take(5).ToList();
            
            string message = "Top 5 - Older automobiles:\n\n";
            foreach (var entry in top5)
            {
                message += $"ID Automobile: {entry.Key}, Model: {entry.Value}\n";
            }

            MessageDialog md = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                message
            );

            md.Run();
            md.Destroy();
        }
        
        private unsafe void OnShowTopAutomobilesServicesCliked(object sender, EventArgs e)
        {
            // Top 5 vehículos con más servicios

            if(AppData.automobiles_data.GetSize() == 0 || AppData.services_data.GetSize() == 0){
                MSDialog.ShowMessageDialog(this, "Error", "No enough data available to generate report!", MessageType.Error);
                return;
            }

            Dictionary<string, int> automobilesModelDict = new Dictionary<string, int>();
            DoublePointerNode<Automobile>* currentAutomobile = AppData.automobiles_data.GetFirst();

            for (int i = 0; i < AppData.automobiles_data.GetSize(); i++)
            {
                automobilesModelDict.Add(currentAutomobile->value.GetId().ToString(), 0);
                currentAutomobile = currentAutomobile -> next;
            }

            SimpleNode<Service>* currentService = AppData.services_data.GetHead();

            for (int i = 0; i < AppData.services_data.GetSize(); i++)
            {
                automobilesModelDict[currentService->value.GetAutomobileId().ToString()] = 1 + automobilesModelDict[currentService->value.GetAutomobileId().ToString()];
            }

            var top5 = automobilesModelDict.OrderByDescending(entry => entry.Value).Take(5).ToList();

            string message = "Top 5 - Automobiles with more services:\n\n";
            foreach (var entry in top5)
            {
                message += $"ID Automobile: {entry.Key}, Services: {entry.Value}\n";
            }

            MessageDialog md = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                message
            );

            md.Run();
            md.Destroy();
        }

    }
} 
