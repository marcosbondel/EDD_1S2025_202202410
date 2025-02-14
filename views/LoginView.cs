using System;
using Gtk;

namespace View {

    class LoginView : Window
    {
        
        private Entry userEntry;
        private Entry passEntry;

        public LoginView() : base("Login"){
            SetDefaultSize(300, 200);
            SetPosition(WindowPosition.Center);

            // Load the CSS file
            var styleProvider = new CssProvider();
            styleProvider.LoadFromPath("style.css");  // Make sure the path is correct
            // StyleContext.AddProviderForScreen(Gdk.Screen.Default, styleProvider, (int)Gtk.CssProviderPriority.User);
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, styleProvider, 800); // 800 is the priority value.

            Box vbox = new Box(Orientation.Vertical, 10);

            Label userLabel = new Label("Username:");
            userEntry = new Entry();
            userEntry.StyleContext.AddClass("entry");  // Add the "entry" class for styling

            Label passLabel = new Label("Password:");
            passEntry = new Entry();
            passEntry.StyleContext.AddClass("entry");  // Add the "entry" class for styling
            passEntry.Visibility = false;  // Hide password input

            Button loginButton = new Button("Login");
            loginButton.StyleContext.AddClass("button");  // Add the "button" class for styling
            // loginButton.Clicked += (sender, e) =>
            // {
            //     string username = userEntry.Text;
            //     string password = passEntry.Text;
            //     Console.WriteLine($"Username: {username}, Password: {password}");
            // };

            loginButton.Clicked += onDoLogin;

            vbox.PackStart(userLabel, false, false, 5);
            vbox.PackStart(userEntry, false, false, 5);
            vbox.PackStart(passLabel, false, false, 5);
            vbox.PackStart(passEntry, false, false, 5);
            vbox.PackStart(loginButton, false, false, 10);

            Add(vbox);
            ShowAll();
        }

        private void onDoLogin(object sender, EventArgs e){
            Console.WriteLine("Loging in..");
            string rootUsername = "root";
            // string rootUsername = "root@gmail.com";
            string rootPassword = "root";
            // string rootPassword = "root123";

            if(userEntry.Text == rootUsername || passEntry.Text == rootPassword){
                Console.WriteLine("Login successful");
                DashboardView dashboard = new DashboardView();
                dashboard.ShowAll();
                this.Hide();
            } else {
                Console.WriteLine("Login failed");
            }

        }
    }
}

