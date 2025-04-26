using Gtk;
using System;
using System.IO;
using Newtonsoft.Json;
using Storage;
using Model;
using ADT;

namespace Utils {
    public static class BulkUpload {
        public static void OnLoadFileClicked<T>(Window parent) where T: class
        {
            // Crear un diálogo para seleccionar archivo
            FileChooserDialog fileChooser = new FileChooserDialog(
                "Select a JSON file",
                parent,
                FileChooserAction.Open,
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);

            // Filtrar solo archivos JSON
            FileFilter filter = new FileFilter();
            filter.Name = "JSON Files";
            filter.AddPattern("*.json");
            fileChooser.AddFilter(filter);

            // Si el usuario selecciona un archivo
            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                string filePath = fileChooser.Filename;
                LoadJSON<T>(filePath, parent);
            }

            fileChooser.Destroy();
        }

        public static unsafe void LoadJSON<T>(string filePath, Window parent) where T : class{
            try
            {
                // Leer el contenido del archivo JSON
                string jsonContent = File.ReadAllText(filePath);

                // Deserializar el JSON a una lista de objetos
                var items = JsonConvert.DeserializeObject<T[]>(jsonContent);

                // Mostrar los datos en consola (o procesarlos como necesites)
                Console.WriteLine("Data loaded successfully:");
                
                foreach (var item in items){
                    if (typeof(T) == typeof(UserImport)){
                        var local = item as UserImport;
                        if (local != null){

                            // SimpleNode userNode = AppData.users_data.GetById(local.ID);

                            // if(userNode != null){
                            //     Console.WriteLine($"User ID already exists {local.ID} !");
                            //     continue;
                            // }
                            
                            // userNode = AppData.users_data.GetByEmail(local.Correo);

                            // if(userNode != null){
                            //     Console.WriteLine($"Email already exists {local.ID} !");
                            //     continue;
                            // }

                            // UserModel newUser = new UserModel();
                            // newUser.Id = local.ID;
                            // newUser.Age = local.Edad;
                            // newUser.Name = local.Nombres;
                            // newUser.Lastname = local.Apellidos;
                            // newUser.Email = local.Correo;
                            // newUser.Password = local.Contrasenia;
                            
                            // AppData.users_data.insert(newUser);
                        }
                    } else if (typeof(T) == typeof(AutomobileImport)) {
                        var local = item as AutomobileImport;
                        if (local != null){

                            DoublePointerNode automobileNode = AppData.automobiles_data.GetById(local.ID);

                            if(automobileNode != null){
                                Console.WriteLine($"Automobile ID already exists {local.ID} !");
                                continue;
                            }

                            // SimpleNode userNode = AppData.users_data.GetById(local.ID_Usuario);
                            // if(userNode == null){
                            //     Console.WriteLine($"User ID does not exist {local.ID_Usuario}!");
                            //     continue;
                            // }

                            AutomobileModel newAutomobile = new AutomobileModel();

                            newAutomobile.Id = local.ID;
                            newAutomobile.UserId = local.ID_Usuario;
                            newAutomobile.Brand = local.Marca;
                            newAutomobile.Model = local.Modelo.ToString();
                            newAutomobile.Plate = local.Placa;

                            AppData.automobiles_data.insert(newAutomobile); 
                        }
                    } else if (typeof(T) == typeof(SparePartImport)) {
                        var local = item as SparePartImport;
                        if (local != null){

                            SparePartModel sparePartModelFound = AppData.spare_parts_data_avl_tree.BuscarPorId(local.ID);
                
                            if (sparePartModelFound  != null)
                            {
                                Console.WriteLine($"SparePart ID already exists {local.ID} !");
                                continue;
                            }
                            
                            AppData.spare_parts_data_avl_tree.Insertar(local.ID, local.Repuesto, local.Detalles, local.Costo);
                        }
                    }
                }

                // Mostrar mensaje de éxito
                MessageDialog successDialog = new MessageDialog(
                    parent,
                    DialogFlags.Modal,
                    MessageType.Info,
                    ButtonsType.Ok,
                    "JSON file loaded correctly.");
                successDialog.Run();
                successDialog.Destroy();
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si algo falla
                MessageDialog errorDialog = new MessageDialog(
                    parent,
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    $"Error when loading JSON file: {ex.Message}");
                errorDialog.Run();
                errorDialog.Destroy();
            }
        }

    }
}