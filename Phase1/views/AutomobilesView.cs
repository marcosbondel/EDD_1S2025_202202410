using System;
using Gtk;
using Model;
using ADT;
using Storage;
using Utils;

namespace View {
    unsafe class AutomobilesView : Window {
        Entry idEntry;
        Entry userIdEntry;
        Entry brandEntry;
        Entry modelEntry;
        Entry plateEntry;

        private bool isEditing = false;
        private Automobile* current;
        private DoublePointerNode<Automobile>* automobileNode;
        

        public AutomobilesView() : base("AutomobilesView"){
            SetDefaultSize(400, 450);
            SetPosition(WindowPosition.Center);

            // Main vertical container
            Box mainBox = new Box(Orientation.Vertical, 10);
            mainBox.Margin = 20;

            // Title
            Label titleLabel = new Label("<b>Manage Automobiles</b>") { UseMarkup = true };
            titleLabel.SetAlignment(0.5f, 0.5f); // Center alignment

            // Bulk Upload Button (separated from inputs)
            Button bulkUploadButton = new Button("Bulk Upload");
            bulkUploadButton.Clicked += OnBulkUploadClicked;
            
            Button showReportButton = new Button("Show Report");
            showReportButton.Clicked += OnShowReportClicked;

            // Form fields
            idEntry = CreateEntry("ID");
            brandEntry = CreateEntry("Brand Name");
            userIdEntry = CreateEntry("User Id");
            modelEntry = CreateEntry("Model");
            plateEntry = CreateEntry("Plate");

            // Save Button
            Button saveButton = new Button("Save");
            saveButton.Clicked += OnSaveClicked;
           
            Button deleteButton = new Button("Delete");
            deleteButton.Clicked += OnDeleteClicked;
            
            Button editButton = new Button("Edit");
            editButton.Clicked += OnEditClicked;

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
            mainBox.PackStart(userIdEntry, false, false, 5);
            mainBox.PackStart(brandEntry, false, false, 5);
            mainBox.PackStart(modelEntry, false, false, 5);
            mainBox.PackStart(plateEntry, false, false, 5);
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
            Console.WriteLine("Bulk upload clicked.");
            BulkUpload.OnLoadFileClicked<AutomobileImport>(this);
        }

        private void OnShowReportClicked(object sender, EventArgs e){
            string dotCode = AppData.automobiles_data.GenerateDotCode();
            ReportGenerator.GenerateDotFile("Automobiles", dotCode);
            ReportGenerator.ParseDotToImage("Automobiles.dot");

            MSDialog.ShowMessageDialog(this, "Report", "Report has been generated successfully!", MessageType.Info);
        }

        private void OnSaveClicked(object sender, EventArgs e){

            if(isEditing){
                automobileNode->value.SetFixedString(automobileNode->value.Brand, brandEntry.Text, 50);
                automobileNode->value.SetFixedString(automobileNode->value.Model, modelEntry.Text, 50);
                automobileNode->value.SetFixedString(automobileNode->value.Plate, plateEntry.Text, 50);
                // automobileNode->value.SetUserId(Int32.Parse(userIdEntry.Text));
                MSDialog.ShowMessageDialog(this, "Success", "Edited succesfully!", MessageType.Info);
                isEditing = false;
            } else {
                automobileNode = AppData.automobiles_data.GetById(Int32.Parse(idEntry.Text));

                if(automobileNode != null){
                    MSDialog.ShowMessageDialog(this, "Error", "Id already exists!", MessageType.Error);
                    return;
                }

                SimpleNode<User>* userNode = AppData.users_data.GetById(Int32.Parse(userIdEntry.Text));

                if(userNode == null){
                    Console.WriteLine($"User ID does not exist {userIdEntry.Text}!");
                    return;
                }

                Automobile newAutomobile;
                newAutomobile.Id = AppData.automobiles_data.GetSize() + 1;
                newAutomobile.UserId = Int32.Parse(userIdEntry.Text);
                newAutomobile.SetFixedString(newAutomobile.Brand, brandEntry.Text, 50);
                newAutomobile.SetFixedString(newAutomobile.Model, modelEntry.Text, 50);
                newAutomobile.SetFixedString(newAutomobile.Plate, plateEntry.Text, 50);

                AppData.automobiles_data.insert(newAutomobile);
                MSDialog.ShowMessageDialog(this, "Success", "Added succesfully!", MessageType.Info);
            }

            ClearFields();
        }

        private void OnDeleteClicked(object sender, EventArgs e){
            string id = MSDialog.ShowInputDialog(this, "Delete", "Enter ID to delete:");

            if (string.IsNullOrEmpty(id)){
                MSDialog.ShowMessageDialog(this, "Error", "ID cannot be empty!", MessageType.Error);
                return;
            }

            bool deletion = AppData.spare_parts_data.deleteById(Int32.Parse(id));

            if(deletion){
                MSDialog.ShowMessageDialog(this, "Success", "Deleted succesfully!", MessageType.Info);
                AppData.spare_parts_data.list();
            }else{
                MSDialog.ShowMessageDialog(this, "Error", "Record not found!", MessageType.Error);
            }
        }
        
        private void OnEditClicked(object sender, EventArgs e){
            string id = MSDialog.ShowInputDialog(this, "Edit", "Enter ID to edit:");

            if (string.IsNullOrEmpty(id)){
                MSDialog.ShowMessageDialog(this, "Error", "ID cannot be empty!", MessageType.Error);
                return;
            }

            automobileNode = AppData.automobiles_data.GetById(Int32.Parse(id));

            if(automobileNode != null){
                idEntry.Text = automobileNode->value.GetId().ToString();
                userIdEntry.Text = automobileNode->value.GetUserId().ToString();
                brandEntry.Text = automobileNode->value.GetBrand();
                modelEntry.Text = automobileNode->value.GetModel();
                plateEntry.Text = automobileNode->value.GetPlate();

                isEditing = true;
            }else{
                MSDialog.ShowMessageDialog(this, "Error", "Record not found!", MessageType.Error);
            }
        }

        private void OnBackClicked(object sender, EventArgs e){
            DashboardView dashboardView = new DashboardView();
            dashboardView.ShowAll(); // Show Dashboard
            this.Hide(); // Close
        }

        private void ClearFields(){
            idEntry.Text = "";
            brandEntry.Text = "";
            userIdEntry.Text = "";
            modelEntry.Text = "";
            plateEntry.Text = "";
        }
    }

}