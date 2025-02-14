using System;
using Gtk;

namespace View {

    class Dashboard : Window
    {
        public Dashboard() : base("Dashboard")
        {
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

            // Main container (horizontal box)
            Box mainBox = new Box(Orientation.Horizontal, 0);

            // Sidebar (vertical box)
            Box sidebar = new Box(Orientation.Vertical, 10);
            sidebar.Margin = 10;

            // Sidebar Buttons
            Button usersButton = new Button("Users");
            Button servicesButton = new Button("Services");
            Button billsButton = new Button("Bills");

            // Attach event handlers
            usersButton.Clicked += OnUsersClicked;
            servicesButton.Clicked += OnServicesClicked;
            billsButton.Clicked += OnBillsClicked;

            // Add buttons to sidebar
            sidebar.PackStart(usersButton, false, false, 5);
            sidebar.PackStart(servicesButton, false, false, 5);
            sidebar.PackStart(billsButton, false, false, 5);

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
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.ShowAll();
            this.Hide();
        }

        private void OnServicesClicked(object sender, EventArgs e)
        {
            // ServicesWindow servicesWindow = new ServicesWindow();
            // servicesWindow.ShowAll();
        }

        private void OnBillsClicked(object sender, EventArgs e)
        {
            // BillsWindow billsWindow = new BillsWindow();
            // billsWindow.ShowAll();
        }

    }
} 
