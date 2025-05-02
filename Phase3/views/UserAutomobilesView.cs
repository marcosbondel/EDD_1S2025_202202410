using System;
using Gtk;
using Model;
using ADT;
using Storage;

namespace View {
    public class UserAutomobilesView : Window {
        private TreeView automobileTreeView;
        private ListStore automobileStore;
        private int userId;  // The user ID to filter by

        public UserAutomobilesView(int userId) : base("UserAutomobilesView") {
            this.userId = userId;
            
            // Window setup
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);
            BorderWidth = 10;

            // Main container
            VBox mainBox = new VBox(false, 5);

            // Title
            Label titleLabel = new Label($"<b>Automobiles for User {userId}</b>") { 
                UseMarkup = true,
                Xalign = 0.5f  // Center alignment
            };

            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked; // Event handler for button click

            // Create and configure the table
            CreateAutomobileTable();
            
            // Add scrolling
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(automobileTreeView);

            // Layout
            mainBox.PackStart(titleLabel, false, false, 5);
            mainBox.PackStart(backButton, false, false, 5);
            mainBox.PackStart(scrolledWindow, true, true, 5);
            Add(mainBox);

            // Load data
            RefreshAutomobileList();
            ShowAll();
        }

        private void CreateAutomobileTable() {
            automobileTreeView = new TreeView();
            automobileStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

            // Add columns
            automobileTreeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 0));
            automobileTreeView.AppendColumn(new TreeViewColumn("Brand", new CellRendererText(), "text", 1));
            automobileTreeView.AppendColumn(new TreeViewColumn("Model", new CellRendererText(), "text", 2));
            automobileTreeView.AppendColumn(new TreeViewColumn("Plate", new CellRendererText(), "text", 3));

            automobileTreeView.Model = automobileStore;
        }

        private void RefreshAutomobileList() {
            automobileStore.Clear();
            var allAutomobiles = AppData.automobiles_data.GetAll();

            foreach (var autoNode in allAutomobiles) {
                var auto = autoNode.value;
                if (auto.UserId == userId) {  // Only show cars for this user
                    automobileStore.AppendValues(
                        auto.Id.ToString(),
                        auto.Brand,
                        auto.Model,
                        auto.Plate
                    );
                }
            }
        }

        private void OnBackClicked(object sender, EventArgs e){
            AppViews.renderGivenView("user_dashboard");
            this.Hide(); // Close
        }
    }
}