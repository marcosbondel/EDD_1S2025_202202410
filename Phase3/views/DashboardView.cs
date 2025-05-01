using System;
using System.Collections.Generic;
using System.Linq;
using ADT;
using Gtk;
using Storage;
using Utils;
using Model;
using System.Text.Json;
using Structures;

namespace View {

    public class DashboardView : Window
    {

        private LogModel logModel;
        public DashboardView() : base("DashboardView")
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
            Button generateBackupButton = new Button("Generate Backup");

            // Attach event handlers
            usersButton.Clicked += OnUsersClicked;
            servicesButton.Clicked += OnServicesClicked;
            sparePartsButton.Clicked += OnSparePartsClicked;
            automobilesButton.Clicked += OnAutomobilesClicked;
            generateBackupButton.Clicked += OnGenerateBackupClicked;
            sessionsLogsButton.Clicked += OnShowSessionLogsReportClicked;
            logoutButton.Clicked += OnLogoutClicked;

            // Add buttons to sidebar
            sidebar.PackStart(usersButton, false, false, 5);
            sidebar.PackStart(sparePartsButton, false, false, 5);
            sidebar.PackStart(automobilesButton, false, false, 5);
            sidebar.PackStart(servicesButton, false, false, 5);
            // sidebar.PackStart(billsButton, false, false, 5);
            // sidebar.PackStart(showLogsReportButton, false, false, 5);
            sidebar.PackStart(sessionsLogsButton, false, false, 5);
            sidebar.PackStart(generateBackupButton, false, false, 5);
            sidebar.PackStart(logoutButton, false, false, 5);

            // Main content area (placeholder label)
            Label contentLabel = new Label("Welcome to the ADMIN Dashboard");

            // Add sidebar and content area to main box
            mainBox.PackStart(sidebar, false, false, 10);
            mainBox.PackStart(contentLabel, true, true, 10);

            // Add main box to the window
            Add(mainBox);
            ShowAll();
        }

        // Event Handlers for Sidebar Buttons
        private void OnUsersClicked(object sender, EventArgs e){
            AppViews.renderGivenView("users");
            this.Hide();
        }

        private void OnSparePartsClicked(object sender, EventArgs e){
            AppViews.renderGivenView("spare_parts");
            this.Hide();
        }
        
        private void OnAutomobilesClicked(object sender, EventArgs e){
            AppViews.renderGivenView("automobiles");
            this.Hide();
        }

        private void OnServicesClicked(object sender, EventArgs e)
        {
            AppViews.renderGivenView("services");
            this.Hide();
        }
        
        private void OnGenerateBackupClicked(object sender, EventArgs e)
        {
            AppData.user_blockchain.SaveBlockchainToFile();
            AppData.automobiles_data.GenerateFile();
            AppData.compressor.Compress("automobiles");
            MSDialog.ShowMessageDialog(this, "Backup", "The Backup has been generated successfully!", MessageType.Info);
        }
        
        private void OnShowSessionLogsReportClicked(object sender, EventArgs e)
        {
            // Convert the list to JSON
            string json = JsonSerializer.Serialize(AppData.session_logs_data, new JsonSerializerOptions { WriteIndented = true });

            // Define file path
            string filePath = "reports/logs.json";

            // Write JSON to a file
            File.WriteAllText(filePath, json);

            MSDialog.ShowMessageDialog(this, "Session Logs", "Session Logs Report has been generated successfully!", MessageType.Info);
        }
        
        private void OnLogoutClicked(object sender, EventArgs e)
        {
            // Validation, if the current user is null, it means the admin user is using the app
            if(AppData.current_user != null){
                // Before the user leaves the app, we must log the event
                AppData.session_logs_data[^1].loggedOutAt = DateTime.Now.ToString();
                
                // Then we clean the session variables
                AppData.current_user = null;
            }
            
            // The user leaves the app
            AppViews.renderGivenView("login");
            this.Hide();
        }
        
    }
} 
