using System;
using Gtk;
using Model;
using ADT;
using Storage;

namespace View {
    unsafe class UsersView : Window {
        Entry idEntry;
        Entry nameEntry;
        Entry lastnameEntry;
        Entry emailEntry;
        Entry passwordEntry;

        public UsersView() : base("Users"){
            SetDefaultSize(400, 450);
            SetPosition(WindowPosition.Center);

            // Main vertical container
            Box mainBox = new Box(Orientation.Vertical, 10);
            mainBox.Margin = 20;

            // Title
            Label titleLabel = new Label("<b>Manage Users</b>") { UseMarkup = true };
            titleLabel.SetAlignment(0.5f, 0.5f); // Center alignment

            // Bulk Upload Button (separated from inputs)
            Button bulkUploadButton = new Button("Bulk Upload");
            bulkUploadButton.Clicked += OnBulkUploadClicked;

            // Form fields
            idEntry = CreateEntry("ID");
            nameEntry = CreateEntry("Name");
            lastnameEntry = CreateEntry("Lastname");
            emailEntry = CreateEntry("Email");
            passwordEntry = CreateEntry("Password");
            passwordEntry.Visibility = false; // Hide text for password

            // Save Button
            Button saveButton = new Button("Save User");
            saveButton.Clicked += OnSaveClicked;
           
            Button deleteButton = new Button("Delete User");
            deleteButton.Clicked += OnDeleteClicked;
            
            Button editButton = new Button("Edit User");
            editButton.Clicked += OnEditClicked;

            // Back Button (returns to Dashboard)
            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked;

            // Add widgets to the main box
            mainBox.PackStart(titleLabel, false, false, 5);
            mainBox.PackStart(bulkUploadButton, false, false, 10);
            mainBox.PackStart(idEntry, false, false, 5);
            mainBox.PackStart(nameEntry, false, false, 5);
            mainBox.PackStart(lastnameEntry, false, false, 5);
            mainBox.PackStart(emailEntry, false, false, 5);
            mainBox.PackStart(passwordEntry, false, false, 5);
            mainBox.PackStart(saveButton, false, false, 10);
            mainBox.PackStart(backButton, false, false, 10); // Back button at the end
            mainBox.PackStart(editButton, false, false, 10);
            mainBox.PackStart(deleteButton, false, false, 10);

            Add(mainBox);
            ShowAll();
        }

        // Creates an entry field with placeholder text
        private Entry CreateEntry(string placeholder)
        {
            Entry entry = new Entry { PlaceholderText = placeholder };
            return entry;
        }

        // Event Handlers
        private void OnBulkUploadClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Bulk upload clicked.");
        }

        private void OnSaveClicked(object sender, EventArgs e){
            User newUser;
            newUser.Id =1;

            newUser.SetFixedString(newUser.Name, nameEntry.Text, 50);
            newUser.SetFixedString(newUser.Lastname, lastnameEntry.Text, 50);
            newUser.SetFixedString(newUser.Email, emailEntry.Text, 100);
            newUser.SetFixedString(newUser.Password, passwordEntry.Text, 50);
            AppData.users_data.insert(newUser);

            Console.WriteLine($"User saved: {newUser}");
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Let't delete a user!");
        }
        
        private void OnEditClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Let't edit a user!");
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            DashboardView dashboardView = new DashboardView();
            dashboardView.ShowAll(); // Show Dashboard
            this.Hide(); // Close UsersWindow
        }
    }

}