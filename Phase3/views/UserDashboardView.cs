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
            Button automobilesButton = new Button("Automobiles");
            Button servicesButton = new Button("Services");
            Button billsButton = new Button("Bills");
            Button logoutButton = new Button("Logout");

            // Attach event handlers
            servicesButton.Clicked += OnServicesClicked;
            billsButton.Clicked += OnBillsClicked;
            automobilesButton.Clicked += OnAutomobilesClicked;
            logoutButton.Clicked += OnLogoutClicked;

            // Add buttons to sidebar
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

        private void OnAutomobilesClicked(object sender, EventArgs e){
            AppViews.renderGivenView("user_automobiles");
            this.Hide();
        }

        private void OnServicesClicked(object sender, EventArgs e)
        {
            List<int> List_Ids_vehiculos = AppData.automobiles_data.ListarVehiculos_Usuario(AppData.current_user.Id);

            AppData.List_Servicios_Usuarios_InOrden = AppData.services_data_binary_tree.TablaInOrden_Vehiculos(List_Ids_vehiculos);
            AppData.List_Servicios_Usuarios_PreOrden = AppData.services_data_binary_tree.TablaPreOrden_Vehiculos(List_Ids_vehiculos);
            AppData.List_Servicios_Usuarios_PostOrden = AppData.services_data_binary_tree.TablaPostOrden_Vehiculos(List_Ids_vehiculos);

            AppViews.renderGivenView("user_services");
            this.Hide();
        }

        private void OnBillsClicked(object sender, EventArgs e)
        {
            List<int> List_Ids_vehiculos = AppData.automobiles_data.ListarVehiculos_Usuario(AppData.current_user.Id);
            List<int> Lista_Ids_Servicios = AppData.services_data_binary_tree.Servicios_Vehiculos(List_Ids_vehiculos);
            AppData.Lista_Facturas_Usuario = AppData.bills_data_merkle_tree.GetBillsByServiceIds(Lista_Ids_Servicios);

            AppViews.renderGivenView("user_bills");
            this.Hide();
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
