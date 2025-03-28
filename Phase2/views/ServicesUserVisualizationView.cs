using System;
using Gtk;
using Model;
using ADT;
using Storage;
using Utils;
using Trees.Binary;

namespace View {
    public class ServicesUserVisualizationView : Window
    {
        private Button inOrdenButton;
        private Button preOrdenButton;
        private Button postOrdenButton;
        private ListBox listBoxRecorridos;
        private List<BinaryNode> inOrdenList;
        private List<BinaryNode> preOrdenList;
        private List<BinaryNode> postOrdenList;

        public ServicesUserVisualizationView(List<BinaryNode> inOrden, List<BinaryNode> preOrden, List<BinaryNode> postOrden) 
            : base("ServicesUserVisualizationView")
        {
            inOrdenList = inOrden;
            preOrdenList = preOrden;
            postOrdenList = postOrden;

            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

            Box box = new Box(Orientation.Vertical, 10);

            Label titleLabel = new Label("Services Orders Visualization");
            titleLabel.SetSizeRequest(200, 30); // Adjust size for the label
            box.PackStart(titleLabel, false, false, 10);

             // Create the back button
            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked; // Event handler for button click
            box.PackStart(backButton, false, false, 10);

            inOrdenButton = new Button("InOrden");
            inOrdenButton.Clicked += OnInOrdenButtonClicked;
            box.PackStart(inOrdenButton, false, false, 0);

            preOrdenButton = new Button("PreOrden");
            preOrdenButton.Clicked += OnPreOrdenButtonClicked;
            box.PackStart(preOrdenButton, false, false, 0);

            postOrdenButton = new Button("PostOrden");
            postOrdenButton.Clicked += OnPostOrdenButtonClicked;
            box.PackStart(postOrdenButton, false, false, 0);

            listBoxRecorridos = new ListBox();
            box.PackStart(listBoxRecorridos, true, true, 0);

            Add(box);
            ShowAll();
        }

        private void OnInOrdenButtonClicked(object sender, EventArgs e)
        {
            MostrarRecorrido(inOrdenList);
        }

        private void OnPreOrdenButtonClicked(object sender, EventArgs e)
        {
            MostrarRecorrido(preOrdenList);
        }

        private void OnPostOrdenButtonClicked(object sender, EventArgs e)
        {
            MostrarRecorrido(postOrdenList);
        }

        private void MostrarRecorrido(List<BinaryNode> recorrido)
        {
            foreach (var row in listBoxRecorridos.Children)
            {
                listBoxRecorridos.Remove(row);
            }

            foreach (var servicio in recorrido)
            {
                var row = new ListBoxRow();
                var label = new Label($"ID: {servicio.Value.Id}, Repuesto: {servicio.Value.SparePartId}, Vehículo: {servicio.Value.AutomobileId}, Detalles: {servicio.Value.Details}, Costo: {servicio.Value.Cost}");
                row.Add(label);
                listBoxRecorridos.Add(row);
            }

            listBoxRecorridos.ShowAll();
        }

        private void OnBackClicked(object sender, EventArgs e){
            UserDashboardView userDashboardView = new UserDashboardView();
            userDashboardView.ShowAll(); // Show Dashboard
            this.Hide(); // Close
        }
    }

}