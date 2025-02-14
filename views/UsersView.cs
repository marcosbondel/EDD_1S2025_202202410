using System;
using Gtk;

namespace View {
    class UsersWindow : Window {
        public UsersWindow() : base("Users"){
            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            Label label = new Label("Manage Users");
            Add(label);

            ShowAll();
        }
    }

}