using System;
using System.Collections.Generic;
using System.Linq;
using ADT;
using Gtk;
using Storage;
using Utils;
using Model;

namespace View {

    class DashboardView : Window
    {
        public DashboardView() : base("DashboardView")
        {
            SetDefaultSize(900, 500);
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
            Button showTopOlderAutomobilesButton = new Button("Show Top 5 - Older Automobiles");
            Button showTopAutomobilesServicesButton = new Button("Show Top 5 - Automobiles Services");
            Button logoutButton = new Button("Logout");

            // Attach event handlers
            usersButton.Clicked += OnUsersClicked;
            servicesButton.Clicked += OnServicesClicked;
            billsButton.Clicked += OnBillsClicked;
            sparePartsButton.Clicked += OnSparePartsClicked;
            automobilesButton.Clicked += OnAutomobilesClicked;
            showLogsReportButton.Clicked += OnShowLogsReportClicked;
            showTopOlderAutomobilesButton.Clicked += OnShowTopOlderAutomobilesCliked;
            showTopAutomobilesServicesButton.Clicked += OnShowTopAutomobilesServicesCliked;
            logoutButton.Clicked += OnLogoutClicked;

            // Add buttons to sidebar
            sidebar.PackStart(usersButton, false, false, 5);
            sidebar.PackStart(sparePartsButton, false, false, 5);
            sidebar.PackStart(automobilesButton, false, false, 5);
            sidebar.PackStart(servicesButton, false, false, 5);
            sidebar.PackStart(billsButton, false, false, 5);
            sidebar.PackStart(showLogsReportButton, false, false, 5);
            sidebar.PackStart(showTopOlderAutomobilesButton, false, false, 5);
            sidebar.PackStart(showTopAutomobilesServicesButton, false, false, 5);
            sidebar.PackStart(logoutButton, false, false, 5);

            // Main content area (placeholder label)
            Label contentLabel = new Label("Welcome to the Dashboard");

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
            ServicesView onServicesView = new ServicesView();
            onServicesView.ShowAll();
            this.Hide();
        }

        private void OnBillsClicked(object sender, EventArgs e)
        {
            BillsView billsView = new BillsView();
            billsView.ShowAll();
            this.Hide();
        }
        
        private void OnLogoutClicked(object sender, EventArgs e)
        {
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
