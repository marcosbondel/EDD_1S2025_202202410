using System;
using Gtk;
using Model;
using ADT;
using Storage;
using Utils;

namespace View {
    public class ServicesView : Window {
        Entry idEntry;
        Entry sparePartIdEntry;
        Entry automobileIdEntry;
        Entry detailsEntry;
        Entry dateEntry;
        ListBox paymentMethodListBox;
        SpinButton costEntry;

        private bool isEditing = false;
        private Service serviceNode;
        private string selectedPaymentMethod = "Cash"; // Default value

        public ServicesView() : base("Services Management") {
            SetDefaultSize(450, 500);
            SetPosition(WindowPosition.Center);
            BorderWidth = 10;

            // Main container with vertical layout
            Box mainBox = new Box(Orientation.Vertical, 5);
            
            // Title section
            Label titleLabel = new Label("<span size='large' weight='bold'>Service Management</span>") {
                UseMarkup = true,
                MarginBottom = 15
            };
            titleLabel.SetAlignment(0.5f, 0.5f);

            // Form fields container
            Box formBox = new Box(Orientation.Vertical, 5) {
                Margin = 10
            };

            // Create form fields
            idEntry = CreateEntry("Service ID");
            sparePartIdEntry = CreateEntry("Spare Part ID");
            automobileIdEntry = CreateEntry("Automobile ID");
            detailsEntry = CreateEntry("Service Details");
            dateEntry = CreateEntry("Date (YYYY-MM-DD)");
            costEntry = new SpinButton(0, 10000, 0.01) {
                Digits = 2,
                Value = 0
            };

            // Payment method section
            Label paymentLabel = new Label("Payment Method:") {
                Xalign = 0,
                MarginTop = 10
            };
            
            paymentMethodListBox = CreatePaymentMethodListBox();
            ScrolledWindow scrolledWindow = new ScrolledWindow() {
                ShadowType = ShadowType.EtchedIn,
                HeightRequest = 100
            };
            scrolledWindow.Add(paymentMethodListBox);

            // Button container
            Box buttonBox = new Box(Orientation.Horizontal, 5) {
                Homogeneous = true,
                MarginTop = 15
            };

            Button saveButton = new Button("Save") {
                TooltipText = "Save service record"
            };
            saveButton.Clicked += OnSaveClicked;

            Button editButton = new Button("Edit") {
                TooltipText = "Edit existing service"
            };
            editButton.Clicked += OnEditClicked;

            Button deleteButton = new Button("Delete") {
                TooltipText = "Delete service record"
            };
            deleteButton.Clicked += OnDeleteClicked;

            Button reportButton = new Button("Report") {
                TooltipText = "Generate service report"
            };
            reportButton.Clicked += OnShowReportClicked;

            Button bulkButton = new Button("Bulk Upload") {
                TooltipText = "Upload multiple services"
            };
            bulkButton.Clicked += OnBulkUploadClicked;

            Button backButton = new Button("Back") {
                TooltipText = "Return to dashboard"
            };
            backButton.Clicked += OnBackClicked;

            // Add buttons to button box
            buttonBox.PackStart(saveButton, true, true, 0);
            buttonBox.PackStart(editButton, true, true, 0);
            buttonBox.PackStart(deleteButton, true, true, 0);
            buttonBox.PackStart(reportButton, true, true, 0);
            buttonBox.PackStart(bulkButton, true, true, 0);
            buttonBox.PackStart(backButton, true, true, 0);

            // Add all components to main box
            mainBox.PackStart(titleLabel, false, false, 0);
            mainBox.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
            
            // Add form fields
            formBox.PackStart(idEntry, false, false, 2);
            formBox.PackStart(sparePartIdEntry, false, false, 2);
            formBox.PackStart(automobileIdEntry, false, false, 2);
            formBox.PackStart(detailsEntry, false, false, 2);
            formBox.PackStart(dateEntry, false, false, 2);
            formBox.PackStart(costEntry, false, false, 2);
            formBox.PackStart(paymentLabel, false, false, 2);
            formBox.PackStart(scrolledWindow, false, false, 2);
            
            mainBox.PackStart(formBox, false, false, 5);
            mainBox.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
            mainBox.PackStart(buttonBox, false, false, 0);

            Add(mainBox);
            ShowAll();
        }

        private ListBox CreatePaymentMethodListBox() {
            ListBox listBox = new ListBox() {
                SelectionMode = SelectionMode.Single,
                ActivateOnSingleClick = true
            };

            string[] methods = { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Check", "Mobile Payment" };
            
            foreach (string method in methods) {
                Box rowBox = new Box(Orientation.Horizontal, 5) {
                    Margin = 3
                };
                
                // Add icon (optional - would need Gtk.Image)
                // Image icon = new Image(IconTheme.Default.LoadIcon("payment-icon", 16, 0));
                // rowBox.PackStart(icon, false, false, 0);
                
                rowBox.PackStart(new Label(method) { Xalign = 0 }, true, true, 0);
                
                ListBoxRow row = new ListBoxRow() {
                    Selectable = true
                };
                row.Add(rowBox);
                listBox.Add(row);
                
                // Select "Cash" by default
                if (method == "Cash") {
                    listBox.SelectRow(row);
                }
            }

            listBox.RowSelected += (sender, e) => {
                if (e.Row != null) {
                    foreach (var child in ((Box)e.Row.Child).Children) {
                        if (child is Label label) {
                            selectedPaymentMethod = label.Text;
                            break;
                        }
                    }
                }
            };

            return listBox;
        }

        private Entry CreateEntry(string placeholder) {
            return new Entry() {
                PlaceholderText = placeholder,
                MarginBottom = 5
            };
        }

        private void OnBulkUploadClicked(object sender, EventArgs e) {
            BulkUpload.OnLoadFileClicked<ServiceImport>(this);
        }

        private void OnShowReportClicked(object sender, EventArgs e) {
            string dotCode = AppData.services_data_binary_tree.GraficarGraphviz();
            ReportGenerator.GenerateDotFile("Services", dotCode);
            ReportGenerator.ParseDotToImage("Services.dot");

            MSDialog.ShowMessageDialog(this, "Report", "Service report generated successfully!", MessageType.Info);
        }

        private void OnSaveClicked(object sender, EventArgs e) {
            if (isEditing) {
                serviceNode.SparePartId = Int32.Parse(sparePartIdEntry.Text);
                serviceNode.AutomobileId = Int32.Parse(automobileIdEntry.Text);
                serviceNode.Cost = costEntry.Value;
                serviceNode.Details = detailsEntry.Text;

                MSDialog.ShowMessageDialog(this, "Success", "Service updated successfully!", MessageType.Info);
                isEditing = false;
            } else {
                if (string.IsNullOrEmpty(idEntry.Text)) {
                    MSDialog.ShowMessageDialog(this, "Error", "Service ID is required!", MessageType.Error);
                    return;
                }

                Service existingService = AppData.services_data_binary_tree.BuscarPorId(Int32.Parse(idEntry.Text));
                if (existingService != null) {
                    MSDialog.ShowMessageDialog(this, "Error", "Service ID already exists!", MessageType.Error);
                    return;
                }

                SparePartModel sparePart = AppData.spare_parts_data_avl_tree.BuscarPorId(Int32.Parse(sparePartIdEntry.Text));
                if (sparePart == null) {
                    MSDialog.ShowMessageDialog(this, "Error", "Spare part not found!", MessageType.Error);
                    return;
                }

                DoublePointerNode automobile = AppData.automobiles_data.GetById(Int32.Parse(automobileIdEntry.Text));
                if (automobile == null) {
                    MSDialog.ShowMessageDialog(this, "Error", "Automobile not found!", MessageType.Error);
                    return;
                }

                AppData.services_data_binary_tree.Insertar(
                    Int32.Parse(idEntry.Text),
                    Int32.Parse(sparePartIdEntry.Text),
                    Int32.Parse(automobileIdEntry.Text),
                    detailsEntry.Text,
                    costEntry.Value
                );

                // Here we create a new bill
                AppData.bill_id_counter++;
                Bill newBill = new Bill(
                    AppData.bill_id_counter,
                    Int32.Parse(idEntry.Text),
                    costEntry.Value,
                    dateEntry.Text,
                    selectedPaymentMethod
                );
                AppData.bills_data_merkle_tree.Insert(newBill);

                MSDialog.ShowMessageDialog(this, "Success", "Service and bill created successfully!", MessageType.Info);
            }

            ClearFields();
        }

        private void OnDeleteClicked(object sender, EventArgs e) {
            string id = MSDialog.ShowInputDialog(this, "Delete Service", "Enter Service ID to delete:");
            
            if (string.IsNullOrEmpty(id)) {
                MSDialog.ShowMessageDialog(this, "Error", "ID cannot be empty!", MessageType.Error);
                return;
            }

            // Implement your delete logic here
            // Example:
            // bool deleted = AppData.services_data_binary_tree.Eliminar(Int32.Parse(id));
            // if (deleted) { ... } else { ... }
        }

        private void OnEditClicked(object sender, EventArgs e) {
            string id = MSDialog.ShowInputDialog(this, "Edit Service", "Enter Service ID to edit:");

            if (string.IsNullOrEmpty(id)) {
                MSDialog.ShowMessageDialog(this, "Error", "ID cannot be empty!", MessageType.Error);
                return;
            }

            serviceNode = AppData.services_data_binary_tree.BuscarPorId(Int32.Parse(id));

            if (serviceNode != null) {
                idEntry.Text = serviceNode.Id.ToString();
                sparePartIdEntry.Text = serviceNode.SparePartId.ToString();
                automobileIdEntry.Text = serviceNode.AutomobileId.ToString();
                detailsEntry.Text = serviceNode.Details;
                costEntry.Value = serviceNode.Cost;
                isEditing = true;
            } else {
                MSDialog.ShowMessageDialog(this, "Error", "Service not found!", MessageType.Error);
            }
        }

        private void OnBackClicked(object sender, EventArgs e) {
            DashboardView dashboard = new DashboardView();
            dashboard.ShowAll();
            this.Destroy();
        }

        private void ClearFields() {
            idEntry.Text = "";
            sparePartIdEntry.Text = "";
            automobileIdEntry.Text = "";
            detailsEntry.Text = "";
            dateEntry.Text = "";
            costEntry.Value = 0;
            paymentMethodListBox.SelectRow(paymentMethodListBox.GetRowAtIndex(0)); // Reset to Cash
        }
    }
}