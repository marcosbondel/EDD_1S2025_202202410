using System;
using Gtk;
using Model;
using ADT;
using Storage;
using Utils;

namespace View {
    public class UsersView : Window {
        Entry idEntry;
        Entry ageEntry;
        Entry nameEntry;
        Entry lastnameEntry;
        Entry emailEntry;
        Entry passwordEntry;

        private bool isEditing = false;
        private User* current;
        private SimpleNode userNode;
        

        public UsersView() : base("UsersView"){
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
            ageEntry = CreateEntry("Age");
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
            
            Button showReportButton = new Button("Show report");
            showReportButton.Clicked += OnShowReportClicked;

            // Back Button (returns to Dashboard)
            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked;

            // Add widgets to the main box
            mainBox.PackStart(titleLabel, false, false, 5);
            mainBox.PackStart(bulkUploadButton, false, false, 10);
            mainBox.PackStart(showReportButton, false, false, 10);
            mainBox.PackStart(editButton, false, false, 10);
            mainBox.PackStart(deleteButton, false, false, 10);
            mainBox.PackStart(idEntry, false, false, 5);
            mainBox.PackStart(nameEntry, false, false, 5);
            mainBox.PackStart(lastnameEntry, false, false, 5);
            mainBox.PackStart(ageEntry, false, false, 5);
            mainBox.PackStart(emailEntry, false, false, 5);
            mainBox.PackStart(passwordEntry, false, false, 5);
            mainBox.PackStart(saveButton, false, false, 10);
            mainBox.PackStart(backButton, false, false, 10); // Back button at the end

            Add(mainBox);
            ShowAll();
        }

        // Creates an entry field with placeholder text
        private Entry CreateEntry(string placeholder){
            Entry entry = new Entry { PlaceholderText = placeholder };
            return entry;
        }

        // Event Handlers
        private void OnBulkUploadClicked(object sender, EventArgs e){
            BulkUpload.OnLoadFileClicked<UserImport>(this);
        }
        
        private void OnShowReportClicked(object sender, EventArgs e){
            string dotCode = AppData.users_data.GenerateDotCode();
            ReportGenerator.GenerateDotFile("Users", dotCode);
            ReportGenerator.ParseDotToImage("Users.dot");

            MSDialog.ShowMessageDialog(this, "Report", "Report has been generated successfully!", MessageType.Info);
        }

        private void OnSaveClicked(object sender, EventArgs e){

            if(isEditing){
                userNode.value.Name = nameEntry.Text;
                userNode.value.Lastname = lastnameEntry.Text;
                userNode.value.Email = emailEntry.Text;
                userNode.value.Password = passwordEntry.Text;
                userNode.value.Age = int.Parse(ageEntry.Text);

                MSDialog.ShowMessageDialog(this, "Success", "User edited succesfully!", MessageType.Info);
                isEditing = false;
            } else {
                
                userNode = AppData.users_data.GetById(Int32.Parse(idEntry.Text));

                if(userNode != null){
                    MSDialog.ShowMessageDialog(this, "Error", "User ID already exists!", MessageType.Error);
                    return;
                }
                
                userNode = AppData.users_data.GetByEmail(emailEntry.Text);
                
                if(userNode != null){
                    MSDialog.ShowMessageDialog(this, "Error", "Email already exists!", MessageType.Error);
                    return;
                }
                
                UserModel newUser = new UserModel();
                // newUser.Id = AppData.users_data.GetSize() + 1;
                newUser.Id = int.Parse(idEntry.Text);
                newUser.Age = int.Parse(ageEntry.Text);
                newUser.Name = nameEntry.Text;
                newUser.Lastname = lastnameEntry.Text;
                newUser.Email = emailEntry.Text;
                newUser.Password = passwordEntry.Text;
                AppData.users_data.insert(newUser);
                MSDialog.ShowMessageDialog(this, "Success", "User added succesfully!", MessageType.Info);
            }

            ClearFields();
        }

        private void OnDeleteClicked(object sender, EventArgs e){
            string userId = MSDialog.ShowInputDialog(this, "Delete User", "Enter User ID to delete:");

            if (string.IsNullOrEmpty(userId)){
                MSDialog.ShowMessageDialog(this, "Error", "User ID cannot be empty!", MessageType.Error);
                return;
            }

            Console.WriteLine($"UserID to delete: {userId}");

            // Here we need to delete all the related automobiles
            DoublePointerNode current = AppData.automobiles_data.GetFirst();
            Console.WriteLine("Remove the referenced automobiles ...");
            for (int i = 0; i < AppData.automobiles_data.GetSize(); i++)
            {
                if (current.value.UserId == Int32.Parse(userId))
                {
                    AppData.automobiles_data.deleteById(Int32.Parse(userId));
                }
                current = current.next;
            }

            bool deletion = AppData.users_data.deleteById(Int32.Parse(userId));

            if(deletion){
                MSDialog.ShowMessageDialog(this, "Success", "User deleted succesfully!", MessageType.Info);
                AppData.users_data.list();
            }else{
                MSDialog.ShowMessageDialog(this, "Error", "User not found!", MessageType.Error);
            }
        }
        
        private void OnEditClicked(object sender, EventArgs e){
            string userId = MSDialog.ShowInputDialog(this, "Edit User", "Enter User ID to edit:");

            if (string.IsNullOrEmpty(userId)){
                MSDialog.ShowMessageDialog(this, "Error", "User ID cannot be empty!", MessageType.Error);
                return;
            }

            userNode = AppData.users_data.GetById(Int32.Parse(userId));

            if(userNode != null){
                idEntry.Text = userNode.value.Id.ToString();
                nameEntry.Text = userNode.value.Name;
                lastnameEntry.Text = userNode.value.Lastname;
                ageEntry.Text = userNode.value.Age.ToString();
                emailEntry.Text = userNode.value.Email;
                passwordEntry.Text = userNode.value.Password;

                isEditing = true;
            }else{
                MSDialog.ShowMessageDialog(this, "Error", "User not found!", MessageType.Error);
            }
        }

        private void OnBackClicked(object sender, EventArgs e){
            DashboardView dashboardView = new DashboardView();
            dashboardView.ShowAll(); // Show Dashboard
            this.Hide(); // Close UsersWindow
        }

        private void ClearFields(){
            idEntry.Text = "";
            nameEntry.Text = "";
            lastnameEntry.Text = "";
            emailEntry.Text = "";
            passwordEntry.Text = "";
            ageEntry.Text = "";
        }
    }

}