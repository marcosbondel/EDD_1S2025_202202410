using System;
using Gtk;
using Utils;
using Model;
using ADT;
using Storage;

namespace View {

    unsafe class LoginView : Window
    {
        
        private Entry userEntry;
        private Entry passEntry;
        private SimpleNode userNode;
        private LogModel logModel;

        public LoginView() : base("Login"){
            SetDefaultSize(300, 200);
            SetPosition(WindowPosition.Center);

            Box vbox = new Box(Orientation.Vertical, 10);

            Label userLabel = new Label("Email:");
            Label marcosBondelLabel = new Label("LabEDD - Marcos Bonifasi - 202202410");
            userEntry = new Entry();
            userEntry.StyleContext.AddClass("entry");  // Add the "entry" class for styling

            Label passLabel = new Label("Password:");
            passEntry = new Entry();
            passEntry.StyleContext.AddClass("entry");  // Add the "entry" class for styling
            passEntry.Visibility = false;  // Hide password input

            Button loginButton = new Button("Validate");
            loginButton.StyleContext.AddClass("button");  // Add the "button" class for styling

            loginButton.Clicked += onDoLogin;

            vbox.PackStart(marcosBondelLabel, false, false, 5);
            vbox.PackStart(userLabel, false, false, 5);
            vbox.PackStart(userEntry, false, false, 5);
            vbox.PackStart(passLabel, false, false, 5);
            vbox.PackStart(passEntry, false, false, 5);
            vbox.PackStart(loginButton, false, false, 10);

            Add(vbox);
            ShowAll();
        }


        private void onDoLogin(object sender, EventArgs e){
            // MSDialog.ShowMessageDialog(this, "Success", "Welcome, Marcos Bonifasi - 202202410!", MessageType.Info);
            // DashboardView dashboard = new DashboardView();
            // dashboard.ShowAll();
            // this.Hide();
            // return;


            Console.WriteLine("Loging in..");
            // string rootUsername = "root";
            string rootUsername = "“admin@usac.com";
            // string rootPassword = "root";
            string rootPassword = "admin123";

            // We first check if the user entered the admin credentials
            if(userEntry.Text == rootUsername || passEntry.Text == rootPassword){
                MSDialog.ShowMessageDialog(this, "Success", "Welcome, Marcos Bonifasi - 202202410!", MessageType.Info);
                DashboardView dashboard = new DashboardView();
                dashboard.ShowAll();
                this.Hide();
                return;
            } 

            bool userCheck = AppData.users_data.CheckUserCredentials(userEntry.Text, passEntry.Text);

            // We check if it is another user trying to loging
            if(userCheck) {
                Console.WriteLine("Welcom, user!");

                // Here we need to save the user session
                userNode = AppData.users_data.GetByEmail(userEntry.Text);
                logModel = new LogModel(userNode.value.Email, DateTime.Now.ToString(), "");
                AppData.session_logs_data.Add(logModel);

                // We set the current user node as a global instance
                AppData.current_user_node = userNode;

                MSDialog.ShowMessageDialog(this, "Success", $"Welcome, {userNode.value.GetFullname()}!", MessageType.Info);

                // Then we redirect the user to the corresponding view
                UserDashboardView userDashboard = new UserDashboardView();
                userDashboard.ShowAll();
                this.Hide();
            }else {
                Console.WriteLine("Login failed");
                MSDialog.ShowMessageDialog(this, "Error", "Invalid credentials!", MessageType.Error);
            }

        }
    }
}

