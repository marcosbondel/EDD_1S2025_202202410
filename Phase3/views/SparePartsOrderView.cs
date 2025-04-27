using System;
using ADT;
using Gtk;
using Storage;
using Model;
using Utils;
using Trees.AVL;

namespace View {

    public class SparePartsOrderView : Window
    {
        private Button inOrdenButton;
        private Button preOrdenButton;
        private Button postOrdenButton;
        private ListBox listBoxRecorridos;

        public SparePartsOrderView() : base("SparePartsOrderView")
        {
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

           // Create a Box to arrange the title and the button in a horizontal layout
            Box box = new Box(Orientation.Vertical, 10); // Box with horizontal orientation and spacing of 10
            
            // Create the title label
            Label titleLabel = new Label("SpareParts Orders Visualization");
            titleLabel.SetSizeRequest(200, 30); // Adjust size for the label
            box.PackStart(titleLabel, false, false, 10);

            // Create the back button
            Button backButton = new Button("Back");
            backButton.Clicked += OnBackClicked; // Event handler for button click
            box.PackStart(backButton, false, false, 10);

            // Crear los botones para seleccionar el tipo de recorrido
            inOrdenButton = new Button("InOrden");
            inOrdenButton.Clicked += OnInOrdenButtonClicked;
            box.PackStart(inOrdenButton, false, false, 10);

            preOrdenButton = new Button("PreOrden");
            preOrdenButton.Clicked += OnPreOrdenButtonClicked;
            box.PackStart(preOrdenButton, false, false, 10);

            postOrdenButton = new Button("PostOrden");
            postOrdenButton.Clicked += OnPostOrdenButtonClicked;
            box.PackStart(postOrdenButton, false, false, 10);

            // Crear el ListBox para mostrar los resultados
            listBoxRecorridos = new ListBox();
            box.PackStart(listBoxRecorridos, true, true, 10);

            // Agregar la caja al contenedor principal
            Add(box);
            ShowAll();
        }

        private void OnBackClicked(object sender, EventArgs e){
            AppViews.renderGivenView("dashboard"); // Regresar al Dashboard
            this.Hide(); // Close
        }

        // Acción para el botón InOrden
        private void OnInOrdenButtonClicked(object sender, EventArgs e)
        {
            MostrarRecorrido("InOrden");
        }

        // Acción para el botón PreOrden
        private void OnPreOrdenButtonClicked(object sender, EventArgs e)
        {
            MostrarRecorrido("PreOrden");
        }

        // Acción para el botón PostOrden
        private void OnPostOrdenButtonClicked(object sender, EventArgs e)
        {
            MostrarRecorrido("PostOrden");
        }

        // Método para mostrar el recorrido seleccionado
        private void MostrarRecorrido(string tipoRecorrido)
        {
            // Eliminar todas las filas del ListBox antes de agregar nuevas
            foreach (var row in listBoxRecorridos.Children)
            {
                listBoxRecorridos.Remove(row);
            }

            // Obtener el árbol de repuestos de las variables
            var arbol = AppData.spare_parts_data_avl_tree;

            AVLNode[] recorrido = null;

            // Seleccionar el tipo de recorrido
            switch (tipoRecorrido)
            {
                case "InOrden":
                    recorrido = arbol.TablaInOrden();
                    break;

                case "PreOrden":
                    recorrido = arbol.TablaPreOrden();
                    break;

                case "PostOrden":
                    recorrido = arbol.TablaPostOrden();
                    break;
            }

            // Mostrar los resultados en el ListBox
            foreach (var nodo in recorrido)
            {
                var row = new ListBoxRow();
                var label = new Label($"ID: {nodo.value.Id}, Repuesto: {nodo.value.Name}, Costo: {nodo.value.Cost}");
                row.Add(label);
                listBoxRecorridos.Add(row);
            }

            // Refrescar el ListBox para que se actualice con los nuevos elementos
            listBoxRecorridos.ShowAll();
        }

    }
}

