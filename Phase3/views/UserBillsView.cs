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
        private List<BillModel> facturas;

        public UserBillsView(List<BillModel> listaFacturas) : base("UserBillsView")
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

            Button cancelBillButton = new Button("Cancel Bill");
            cancelBillButton.Clicked += OnCancelBillButtonClicked;
            box.PackStart(cancelBillButton, false, false, 10);

            listBoxFacturas = new ListBox();
            box.PackStart(listBoxFacturas, true, true, 10);

            Add(box);
            MostrarFacturas();
            ShowAll();
        }

        private unsafe void OnCancelBillButtonClicked(object sender, EventArgs e){
            string userId = MSDialog.ShowInputDialog(this, "Delete User", "Enter User ID to delete:");

            if (string.IsNullOrEmpty(userId)){
                MSDialog.ShowMessageDialog(this, "Error", "User ID cannot be empty!", MessageType.Error);
                return;
            }

            // if (int.TryParse(userId, out int id))
            // {
            //     int idUsuario = AppData.current_user.value.Id;
            //     List<int> List_Ids_vehiculos = AppData.automobiles_data.ListarVehiculos_Usuario(idUsuario);
            //     List<int> Lista_Ids_Servicios = AppData.services_data_binary_tree.Servicios_Vehiculos(List_Ids_vehiculos);
            //     List<BillModel> Lista_Facturas_Usuario = AppData.bills_data_b_tree.ObtenerFacturasPorServicios(Lista_Ids_Servicios);
                
            //     List<int> Ids_Facturas_Usuario = Lista_Facturas_Usuario.Select(f => f.Id).ToList();

            //     if (Ids_Facturas_Usuario.Contains(id))
            //     {
            //         AppData.bills_data_b_tree.Eliminar(id);
            //         MSDialog.ShowMessageDialog(this, "Success", "Bill deleted succesfully!", MessageType.Info);
            //     }
            //     else
            //     {
            //         MSDialog.ShowMessageDialog(this, "Error", "You cannot delete a bill that does not belong to you!", MessageType.Error);
            //     }
            // }
            // else
            // {
            //     MSDialog.ShowMessageDialog(this, "Error", "Please, enter a valid ID!", MessageType.Error);
            // }
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

