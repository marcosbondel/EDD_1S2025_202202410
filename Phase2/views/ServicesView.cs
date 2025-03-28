using System;
using Gtk;
using Model;
using ADT;
using Storage;
using Utils;

namespace View {
    public unsafe class ServicesView : Window {
        Entry idEntry;
        Entry sparePartIdEntry;
        Entry automobileIdEntry;
        Entry detailsEntry;
        SpinButton costEntry;

        private bool isEditing = false;
        private Service* current;
        private SimpleNodeService serviceNode;
        

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
            
            Button showReportButton = new Button("Show report");
            showReportButton.Clicked += OnShowReportClicked;
           
            Button deleteButton = new Button("Delete");
            deleteButton.Clicked += OnDeleteClicked;
            
            Button editButton = new Button("Edit");
            editButton.Clicked += OnEditClicked;

            // Back Button (returns to Dashboard)
            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked;

            // Add widgets to the main box
            mainBox.PackStart(titleLabel, false, false, 5);
            mainBox.PackStart(showReportButton, false, false, 10);
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

        private void OnShowReportClicked(object sender, EventArgs e){
            string dotCode = AppData.services_data_binary_tree.GraficarGraphviz();
            ReportGenerator.GenerateDotFile("Services", dotCode);
            ReportGenerator.ParseDotToImage("Services.dot");

            MSDialog.ShowMessageDialog(this, "Report", "Report has been generated successfully!", MessageType.Info);
        }

        private void OnSaveClicked(object sender, EventArgs e){

            if(isEditing){
                // fixed (Service* service = &serviceNode->value)
                // {
                //     service->SparePartId = Int32.Parse(sparePartIdEntry.Text);
                //     service->AutomobileId = Int32.Parse(automobileIdEntry.Text);
                //     service->Cost = costEntry.Value;
                //     service->SetFixedString(service->Details, detailsEntry.Text, 200);
                // }

                serviceNode.value.SparePartId = Int32.Parse(sparePartIdEntry.Text);
                serviceNode.value.AutomobileId = Int32.Parse(automobileIdEntry.Text);
                serviceNode.value.Cost = costEntry.Value;
                serviceNode.value.Details = detailsEntry.Text;

                MSDialog.ShowMessageDialog(this, "Success", "Edited succesfully!", MessageType.Info);
                isEditing = false;

            
            } else {
                // serviceNode = AppData.services_data.GetById(Int32.Parse(idEntry.Text));

                // if(serviceNode != null){
                //     MSDialog.ShowMessageDialog(this, "Error", "ID already exists!", MessageType.Error);
                //     return;
                // }

                // Service newService;
                // // newService.Id = AppData.services_data.GetSize() + 1;
                // newService.Id = Int32.Parse(idEntry.Text);
                // newService.SparePartId = Int32.Parse(sparePartIdEntry.Text);
                // newService.AutomobileId = Int32.Parse(automobileIdEntry.Text);
                // newService.Cost = costEntry.Value;
                // newService.SetFixedString(newService.Details, detailsEntry.Text, 50);

                // // Validations
                // serviceNode = AppData.services_data.GetById(newService.GetId());
                // SimpleNode<SparePart>* sparePartNode = AppData.spare_parts_data.GetById(newService.GetSparePartId());
                // DoublePointerNode<Automobile>* automobileNode = AppData.automobiles_data.GetById(newService.GetAutomobileId());

                // if(serviceNode != null){
                //     MSDialog.ShowMessageDialog(this, "Error", "Service ID already exists!", MessageType.Error);
                //     return;
                // }

                // if(sparePartNode == null){
                //     MSDialog.ShowMessageDialog(this, "Error", "SparePart not found!", MessageType.Error);
                //     return;
                // }

                // if(automobileNode == null){
                //     MSDialog.ShowMessageDialog(this, "Error", "Automobile not found!", MessageType.Error);
                //     return;
                // }

                // We first check there's no service with the same ID
                ServiceModel serviceExistence = AppData.services_data_binary_tree.BuscarPorId(Int32.Parse(idEntry.Text));

                if(serviceExistence != null){
                    MSDialog.ShowMessageDialog(this, "Error", "Service ID already exists!", MessageType.Error);
                    return;
                }

                // We ensure the spare part exists
                SparePartModel sparePartExistence = AppData.spare_parts_data_avl_tree.BuscarPorId(Int32.Parse(sparePartIdEntry.Text));

                if(sparePartExistence == null){
                    MSDialog.ShowMessageDialog(this, "Error", "SparePart not found!", MessageType.Error);
                    return;
                }

                // We ensure the automobile exists
                DoublePointerNode automobileNode = AppData.automobiles_data.GetById(Int32.Parse(automobileIdEntry.Text));

                if(automobileNode == null){
                    MSDialog.ShowMessageDialog(this, "Error", "Automobile not found!", MessageType.Error);
                    return;
                }

                // AppData.services_data.enqueu(newService);
                AppData.services_data_binary_tree.Insertar(Int32.Parse(idEntry.Text), Int32.Parse(sparePartIdEntry.Text), Int32.Parse(automobileIdEntry.Text), detailsEntry.Text, costEntry.Value);
                MSDialog.ShowMessageDialog(this, "Success", "Service added succesfully!", MessageType.Info);

                // // Here we need to create a new Bill
                // Bill newBill;

                // // SimpleNode<SparePart>* sparePartNode = AppData.spare_parts_data.GetById(newService.GetSparePartId());
                // newBill.Id = AppData.bills_data.GetSize() + 1;
                // newBill.OrderId = newService.GetId();
                // newBill.TotalCost = newService.GetCost() + sparePartNode->value.GetCost();

                // AppData.bills_data.push(newBill);



                //Here we need to insert the SparePartId and the automobileId to the matrix
                // AppData.logs_data.Insert(Int32.Parse(sparePartIdEntry.Text), Int32.Parse(automobileIdEntry.Text), detailsEntry.Text);
                
                AppData.bill_id_counter++;
                AppData.bills_data_b_tree.Insertar(AppData.bill_id_counter,Int32.Parse(idEntry.Text), costEntry.Value);
                MSDialog.ShowMessageDialog(this, "Success", "Bill Added succesfully!", MessageType.Info);

            }

            ClearFields();
        }

        private void OnDeleteClicked(object sender, EventArgs e){
            string id = MSDialog.ShowInputDialog(this, "Delete", "Enter ID to delete:");

            if (string.IsNullOrEmpty(id)){
                MSDialog.ShowMessageDialog(this, "Error", "ID cannot be empty!", MessageType.Error);
                return;
            }

            // bool deletion = AppData.spare_parts_data.deleteById(Int32.Parse(id));

            // if(deletion){
            //     MSDialog.ShowMessageDialog(this, "Success", "Deleted succesfully!", MessageType.Info);
            //     AppData.spare_parts_data.list();
            // }else{
            //     MSDialog.ShowMessageDialog(this, "Error", "Record not found!", MessageType.Error);
            // }
        }
        
        private void OnEditClicked(object sender, EventArgs e){
            string id = MSDialog.ShowInputDialog(this, "Edit", "Enter ID to edit:");

            if (string.IsNullOrEmpty(id)){
                MSDialog.ShowMessageDialog(this, "Error", "ID cannot be empty!", MessageType.Error);
                return;
            }

            // serviceNode = AppData.services_data.GetById(Int32.Parse(id));


            // if(serviceNode != null){
            //     idEntry.Text = serviceNode.value.Id.ToString();
            //     sparePartIdEntry.Text = serviceNode.value.SparePartId.ToString();
            //     automobileIdEntry.Text = serviceNode.value.AutomobileId.ToString();
            //     detailsEntry.Text = serviceNode.value.Details;
            //     costEntry.Text = serviceNode.value.Cost.ToString();

            //     isEditing = true;
            // }else{
            //     MSDialog.ShowMessageDialog(this, "Error", "Record not found!", MessageType.Error);
            // }
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