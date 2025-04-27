using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
namespace Model {

    public interface BillInterface {
        int GetId();
        int GetOrderId();
        double GetTotalCost();
    }

    public class Bill {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double TotalCost { get; set; }
        public string Date { get; set; }
        public string PaymentMethod { get; set; }

        public Bill(int id, int id_servicio, double total, string fecha, string metodo_pago) {
            Id = id;
            OrderId = id_servicio;
            TotalCost = total;
            Date = fecha;
            PaymentMethod = metodo_pago;
        }

        public string GetHash()
        {
            string data = JsonConvert.SerializeObject(this); // Serializar la factura a JSON
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Convertir a hexadecimal
                }
                return builder.ToString();
            }
        }
    }

}