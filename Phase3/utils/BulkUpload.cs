using Gtk;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Storage;
using Model;
using ADT;
using Utils;

namespace Utils
{
    public static class BulkUpload
    {
        private static User userNode;
        private static Automobile newAutomobile;

        public static void OnLoadFileClicked<T>(Window parent) where T : class
        {
            using (var fileChooser = new FileChooserDialog(
                "Select a JSON file",
                parent,
                FileChooserAction.Open,
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept))
            {
                // Configure file filter
                var filter = new FileFilter();
                filter.Name = "JSON Files";
                filter.AddPattern("*.json");
                fileChooser.AddFilter(filter);

                try
                {
                    if (fileChooser.Run() == (int)ResponseType.Accept)
                    {
                        string filePath = fileChooser.Filename;
                        
                        // Process asynchronously to avoid blocking the UI
                        Task.Run(() => LoadJSON<T>(filePath, parent));
                    }
                }
                finally
                {
                    fileChooser.Hide();
                }
            }
        }

        private static void LoadJSON<T>(string filePath, Window parent) where T : class
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                var items = JsonConvert.DeserializeObject<T[]>(jsonContent);

                Application.Invoke((sender, e) =>
                {
                    Console.WriteLine("Data loaded successfully:");
                });

                foreach (var item in items)
                {
                    ProcessItem(item);
                }

                ShowMessageDialog(parent, "JSON file loaded successfully!", MessageType.Info);
            }
            catch (JsonException jsonEx)
            {
                ShowMessageDialog(parent, $"Invalid JSON format: {jsonEx.Message}", MessageType.Error);
            }
            catch (IOException ioEx)
            {
                ShowMessageDialog(parent, $"File access error: {ioEx.Message}", MessageType.Error);
            }
            catch (Exception ex)
            {
                ShowMessageDialog(parent, $"Unexpected error: {ex.Message}", MessageType.Error);
            }
        }

        private static void ProcessItem<T>(T item) where T : class
        {
            if (typeof(T) == typeof(UserImport))
            {
                ProcessUserImport(item as UserImport);
            }
            else if (typeof(T) == typeof(AutomobileImport))
            {
                ProcessAutomobileImport(item as AutomobileImport);
            }
            else if (typeof(T) == typeof(SparePartImport))
            {
                ProcessSparePartImport(item as SparePartImport);
            }
            else if (typeof(T) == typeof(ServiceImport))
            {
                ProcessServiceImport(item as ServiceImport);
            }
            else
            {
                Console.WriteLine($"Unknown type: {typeof(T).Name}");
            }
        }

        private static void ProcessUserImport(UserImport local)
        {
            if (local == null) return;

            userNode = AppData.user_blockchain.GetById(local.ID);

            if (userNode != null)
            {
                Console.WriteLine($"User ID already exists {local.ID}!");
                return;
            }

            userNode = AppData.user_blockchain.FindByEmail(local.Correo);

            if (userNode != null)
            {
                Console.WriteLine($"Email already exists {local.ID}!");
                return;
            }

            var newUser = new User
            {
                Id = local.ID,
                Age = local.Edad,
                Name = local.Nombres,
                Lastname = local.Apellidos,
                Email = local.Correo,
                Password = SHA256Utils.GenerarHashSHA256(local.Contrasenia)
            };

            AppData.user_blockchain.AddBlock(newUser);
        }

        private static void ProcessAutomobileImport(AutomobileImport local)
        {
            if (local == null) return;

            var automobileNode = AppData.automobiles_data.GetById(local.ID);

            if (automobileNode != null)
            {
                Console.WriteLine($"Automobile ID already exists {local.ID}!");
                return;
            }

            userNode = AppData.user_blockchain.GetById(local.ID_Usuario);

            if (userNode == null)
            {
                Console.WriteLine($"User ID does not exist {local.ID_Usuario}!");
                return;
            }

            newAutomobile = new Automobile
            {
                Id = local.ID,
                UserId = local.ID_Usuario,
                Brand = local.Marca,
                Model = local.Modelo.ToString(),
                Plate = local.Placa
            };

            AppData.automobiles_data.insert(newAutomobile);
        }

        private static void ProcessSparePartImport(SparePartImport local)
        {
            if (local == null) return;

            var sparePartModelFound = AppData.spare_parts_data_avl_tree.BuscarPorId(local.ID);

            if (sparePartModelFound != null)
            {
                Console.WriteLine($"SparePart ID already exists {local.ID}!");
                return;
            }

            AppData.spare_parts_data_avl_tree.Insertar(
                local.ID,
                local.Repuesto,
                local.Detalles,
                local.Costo);
        }

        private static void ProcessServiceImport(ServiceImport local)
        {
            if (local == null) return;

            var serviceNode = AppData.services_data_binary_tree.BuscarPorId(local.ID);

            if (serviceNode != null)
            {
                Console.WriteLine($"Service ID already exists {local.ID}!");
                return;
            }

            var sparePartModelFound = AppData.spare_parts_data_avl_tree.BuscarPorId(local.ID_Repuesto);

            if (sparePartModelFound == null)
            {
                Console.WriteLine($"SparePart ID does not exist {local.ID_Repuesto}!");
                return;
            }

            var automobileNode = AppData.automobiles_data.GetById(local.ID_Vehiculo);

            if (automobileNode == null)
            {
                Console.WriteLine($"Automobile ID does not exist {local.ID_Vehiculo}!");
                return;
            }

            var newService = new Service
            {
                Id = local.ID,
                SparePartId = local.ID_Repuesto,
                AutomobileId = local.ID_Vehiculo,
                Details = local.Detalles,
                Cost = local.Costo
            };

            AppData.services_data_binary_tree.Insertar(newService.Id, newService.SparePartId, newService.AutomobileId, newService.Details, newService.Cost);
        
            // Here we create a new bill
            AppData.bill_id_counter++;
            Bill newBill = new Bill(
                AppData.bill_id_counter,
                newService.Id,
                newService.Cost,
                DateTime.Now.ToString("yyyy-MM-dd"),
                "Cash"
            );

            AppData.bills_data_merkle_tree.Insert(newBill);
            AppData.automobile_spare_parts_graph.Insertar(
                newService.AutomobileId.ToString(),
                newService.SparePartId.ToString()
            );
        }

        private static void ShowMessageDialog(Window parent, string message, MessageType type)
        {
            Application.Invoke((sender, e) =>
            {
                using (var dialog = new MessageDialog(
                    parent,
                    DialogFlags.Modal,
                    type,
                    ButtonsType.Ok,
                    message)
                )
                {
                    dialog.Run();
                    dialog.Hide();
                }
            });
        }
    }
}