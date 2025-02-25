using System;
using Gtk;
using Model;
using ADT;
using Storage;
using Utils;

namespace View {
    unsafe class SparePartsView : Window {
        Entry idEntry;
        Entry nameEntry;
        Entry detailsEntry;
        SpinButton costEntry;

        private bool isEditing = false;
        private SparePart* current;
        private SimpleNode<SparePart>* sparePartNode;
        

        public SparePartsView() : base("SparePartsView"){
            SetDefaultSize(400, 450);
            SetPosition(WindowPosition.Center);

            // Main vertical container
            Box mainBox = new Box(Orientation.Vertical, 10);
            mainBox.Margin = 20;

            // Title
            Label titleLabel = new Label("<b>Manage SpareParts</b>") { UseMarkup = true };
            titleLabel.SetAlignment(0.5f, 0.5f); // Center alignment

            // Bulk Upload Button (separated from inputs)
            Button bulkUploadButton = new Button("Bulk Upload");
            bulkUploadButton.Clicked += OnBulkUploadClicked;

            // Form fields
            idEntry = CreateEntry("ID");
            nameEntry = CreateEntry("Spare Part Name");
            detailsEntry = CreateEntry("Details");
            //costEntry = CreateEntry("Cost");

            costEntry = new SpinButton(1, 1000, 1);
            costEntry.Value = 1; // Default value
            costEntry.Numeric = true; // Ensure it only takes numbers

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
            mainBox.PackStart(editButton, false, false, 10);
            mainBox.PackStart(deleteButton, false, false, 10);
            mainBox.PackStart(idEntry, false, false, 5);
            mainBox.PackStart(nameEntry, false, false, 5);
            mainBox.PackStart(detailsEntry, false, false, 5);
            mainBox.PackStart(costEntry, false, false, 5);
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
            BulkUpload.OnLoadFileClicked<SparePartImport>(this);
        }

        private void OnSaveClicked(object sender, EventArgs e){

            if(isEditing){
                fixed (SparePart* sparePart = &sparePartNode->value){
                    sparePart->SetFixedString(sparePart->Name, nameEntry.Text, 50);
                    sparePart->SetFixedString(sparePart->Details, detailsEntry.Text, 50);
                    sparePart->SetCost(costEntry.Value);
                }
                MSDialog.ShowMessageDialog(this, "Success", "Edited succesfully!", MessageType.Info);
                isEditing = false;
            } else {
                SparePart newSparePart;
                newSparePart.Id = AppData.spare_parts_data.GetSize() + 1;
                newSparePart.Cost = costEntry.Value;
                newSparePart.SetFixedString(newSparePart.Name, nameEntry.Text, 50);
                newSparePart.SetFixedString(newSparePart.Details, detailsEntry.Text, 50);

                AppData.spare_parts_data.insert(newSparePart);
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

            sparePartNode = AppData.spare_parts_data.GetById(Int32.Parse(id));

            if(sparePartNode != null){
                idEntry.Text = sparePartNode->value.GetId().ToString();
                nameEntry.Text = sparePartNode->value.GetName();
                detailsEntry.Text = sparePartNode->value.GetDetails();
                costEntry.Value = sparePartNode->value.GetCost();

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
            nameEntry.Text = "";
            detailsEntry.Text = "";
            costEntry.Value = 1; // Default value
        }
    }

}