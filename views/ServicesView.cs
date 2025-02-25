using System;
using Gtk;
using Model;
using ADT;
using Storage;
using utils;

namespace View {
    unsafe class ServicesView : Window {
        Entry idEntry;
        Entry sparePartIdEntry;
        Entry automobileIdEntry;
        Entry detailsEntry;
        SpinButton costEntry;

        private bool isEditing = false;
        private Service* current;
        private SimpleNode<Service>* serviceNode;
        

        public ServicesView() : base("ServicesView"){
            SetDefaultSize(400, 450);
            SetPosition(WindowPosition.Center);

            // Main vertical container
            Box mainBox = new Box(Orientation.Vertical, 10);
            mainBox.Margin = 20;

            // Title
            Label titleLabel = new Label("<b>Manage Services</b>") { UseMarkup = true };
            titleLabel.SetAlignment(0.5f, 0.5f); // Center alignment

            // Form fields
            idEntry = CreateEntry("ID");
            automobileIdEntry = CreateEntry("Automobile Id");
            sparePartIdEntry = CreateEntry("Spare part Id");
            detailsEntry = CreateEntry("Details");
            
            costEntry = new SpinButton(0, 100, 0.1);
            costEntry.Digits = 2;

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
            mainBox.PackStart(editButton, false, false, 10);
            mainBox.PackStart(deleteButton, false, false, 10);
            mainBox.PackStart(idEntry, false, false, 5);
            mainBox.PackStart(sparePartIdEntry, false, false, 5);
            mainBox.PackStart(automobileIdEntry, false, false, 5);
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

        private void OnSaveClicked(object sender, EventArgs e){

            if(isEditing){
                fixed (Service* service = &serviceNode->value)
                {
                    service->SparePartId = Int32.Parse(sparePartIdEntry.Text);
                    service->AutomobileId = Int32.Parse(automobileIdEntry.Text);
                    service->Cost = costEntry.Value;
                    service->SetFixedString(service->Details, detailsEntry.Text, 200);
                }
                MSDialog.ShowMessageDialog(this, "Success", "Edited succesfully!", MessageType.Info);
                isEditing = false;
            } else {
                Service newService;
                newService.Id = AppData.services_data.GetSize() + 1;
                newService.SparePartId = Int32.Parse(sparePartIdEntry.Text);
                newService.AutomobileId = Int32.Parse(automobileIdEntry.Text);
                newService.Cost = costEntry.Value;
                newService.SetFixedString(newService.Details, detailsEntry.Text, 50);

                AppData.services_data.push(newService);
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

            serviceNode = AppData.services_data.GetById(Int32.Parse(id));


            if(serviceNode != null){
                idEntry.Text = serviceNode->value.GetId().ToString();
                sparePartIdEntry.Text = serviceNode->value.GetSparePartId().ToString();
                automobileIdEntry.Text = serviceNode->value.GetAutomobileId().ToString();
                detailsEntry.Text = serviceNode->value.GetDetails();
                costEntry.Text = serviceNode->value.GetCost().ToString();

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
            automobileIdEntry.Text = "";
            sparePartIdEntry.Text = "";
            detailsEntry.Text = "";
            costEntry.Value = 0;
        }
    }

}