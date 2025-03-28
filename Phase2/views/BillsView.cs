using System;
using ADT;
using Gtk;
using Storage;
using Model;
using Utils;

namespace View {

    class BillsView : Window
    {
        ListStore listStore;
    

        public unsafe BillsView() : base("BillsView"){
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

            // Create a Box to arrange the title and the button in a horizontal layout
            Box box = new Box(Orientation.Vertical, 10); // Box with horizontal orientation and spacing of 10
            
            // Create the title label
            Label titleLabel = new Label("Bills");
            titleLabel.SetSizeRequest(200, 30); // Adjust size for the label

            // Create the back button
            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked; // Event handler for button click
            
            Button cancelBillButton = new Button("Cancel bill");
            cancelBillButton.Clicked += OnCancelBillClicked; // Event handler for button click
            
            Button showReportButton = new Button("Show Report");
            showReportButton.Clicked += OnShowReportClicked; // Event handler for button click
            
            // Add the label and button to the Box
            box.PackStart(titleLabel, false, false, 10);
            box.PackStart(backButton, false, false, 10);
            box.PackStart(cancelBillButton, false, false, 10);
            // box.PackStart(showReportButton, false, false, 10);
            
            // Create a ListStore to hold the data for the TreeView
            listStore = new ListStore(typeof(int), typeof(int), typeof(double));

            SimpleNode<Bill>* current = AppData.bills_data.GetTop();

            for (int i = 0; i < AppData.bills_data.GetSize(); i++)
            {
                listStore.AppendValues(current->value.GetId(), current->value.GetOrderId(), current->value.GetTotalCost());
                current = current->next;
            }

            // Create the TreeView and associate it with the ListStore
            TreeView treeView = new TreeView(listStore);

            // Create the columns for the TreeView
            TreeViewColumn column1 = new TreeViewColumn { Title = "ID" };
            TreeViewColumn column2 = new TreeViewColumn { Title = "Order ID" };
            TreeViewColumn column3 = new TreeViewColumn { Title = "Total Cost" };

            // Create cell renderers for each column
            CellRendererText cellRenderer1 = new CellRendererText();
            CellRendererText cellRenderer2 = new CellRendererText();
            CellRendererText cellRenderer3 = new CellRendererText();

            // Add the cell renderers to the columns
            column1.PackStart(cellRenderer1, true);
            column2.PackStart(cellRenderer2, true);
            column3.PackStart(cellRenderer3, true);

            // Assign the data from the ListStore to the columns
            column1.AddAttribute(cellRenderer1, "text", 0);
            column2.AddAttribute(cellRenderer2, "text", 1);
            column3.AddAttribute(cellRenderer3, "text", 2);

            // Add the columns to the TreeView
            treeView.AppendColumn(column1);
            treeView.AppendColumn(column2);
            treeView.AppendColumn(column3);

            // Create a scrollable window for the TreeView
            ScrolledWindow scrolledWindow = new ScrolledWindow
            {
                HscrollbarPolicy = PolicyType.Automatic,
                VscrollbarPolicy = PolicyType.Automatic,
                ShadowType = ShadowType.In
            };
            scrolledWindow.Add(treeView);

            // Create a main Box (VBox style)
            Box vbox = new Box(Orientation.Vertical, 10); // Box with vertical orientation and spacing of 10
            vbox.PackStart(box, false, false, 10); // Add the Box (title and button) to the top
            vbox.PackStart(scrolledWindow, true, true, 10); // Add the TreeView below

            // Add the VBox to the main window
            Add(vbox);

            // Show all widgets
            ShowAll();
        }

        private void OnBackClicked(object sender, EventArgs e){
            DashboardView dashboardView = new DashboardView();
            dashboardView.ShowAll(); // Show Dashboard
            this.Hide(); // Close
        }

        private void OnShowReportClicked(object sender, EventArgs e){
            string dotCode = AppData.bills_data.GenerateDotFile();
            ReportGenerator.GenerateDotFile("Bill", dotCode);
            ReportGenerator.ParseDotToImage("Bill.dot");

            MSDialog.ShowMessageDialog(this, "Report", "Report has been generated successfully!", MessageType.Info);
        }
        
        private unsafe void OnCancelBillClicked(object sender, EventArgs e){
            if(AppData.bills_data.GetSize() == 0){
                MSDialog.ShowMessageDialog(this, "Error", "The pile is empty!", MessageType.Error);
                return;
            }

            SimpleNode<Bill>* deletedNode = AppData.bills_data.pop();

             TreeIter iter;
    
            // Get the first row in the ListStore
            if (listStore.GetIterFirst(out iter)) 
            {
                // Remove the first row
                listStore.Remove(ref iter);
            }
            else
            {
                Console.WriteLine("The list is empty. No row to delete.");
            }

            MSDialog.ShowMessageDialog(this, "Success", "Bill deleted succesfully!", MessageType.Info);

            // Here we need to refresh the tableview
        }

    }
}

