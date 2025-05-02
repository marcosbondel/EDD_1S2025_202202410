using System;
using ADT;
using Gtk;
using Storage;
using Model;
using Utils;

namespace View {
    public class UserBillsView : Window
    {
        private ListBox listBoxFacturas;
        private List<Bill> facturas;

        public UserBillsView(List<Bill> listaFacturas) : base("UserBillsView")
        {
            facturas = listaFacturas;

            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

            Box box = new Box(Orientation.Vertical, 10);

            Label titleLabel = new Label("My Bills");
            titleLabel.SetSizeRequest(200, 30); // Adjust size for the label
            box.PackStart(titleLabel, false, false, 10);

            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked; // Event handler for button click
            box.PackStart(backButton, false, false, 10);

            listBoxFacturas = new ListBox();
            box.PackStart(listBoxFacturas, true, true, 10);

            Add(box);
            MostrarFacturas();
            ShowAll();
        }

        private void MostrarFacturas()
        {
            foreach (var row in listBoxFacturas.Children)
            {
                listBoxFacturas.Remove(row);
            }

            foreach (var factura in facturas)
            {
                var row = new ListBoxRow();
                var label = new Label($"ID: {factura.Id}, Servicio: {factura.OrderId}, Total: {factura.TotalCost}");
                row.Add(label);
                listBoxFacturas.Add(row);
            }

            listBoxFacturas.ShowAll();
        }

        private void OnBackClicked(object sender, EventArgs e){
            UserDashboardView userDashboardView = new UserDashboardView();
            userDashboardView.ShowAll(); // Show Dashboard
            this.Hide(); // Close
        }
    }
}

