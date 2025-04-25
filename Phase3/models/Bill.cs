using System;

namespace Model {

    public interface BillInterface {
        int GetId();
        int GetOrderId();
        double GetTotalCost();
    }

    public unsafe struct Bill : BillInterface {
        public int Id;
        public int OrderId;
        public double TotalCost;

        public void SetFixedString(char* destination, string source, int maxLength) {
            int length = Math.Min(source.Length, maxLength - 1); // Leave space for null-terminator
            for (int i = 0; i < length; i++)
            {
                destination[i] = source[i];
            }
            destination[length] = '\0'; // Null-terminate (optional)
        }

        public string GetFixedString(char* source, int maxLength) {
            return new string(source).TrimEnd('\0'); // Convert and trim nulls
        }

        public int GetId() {
            return Id;
        }
        
        public int GetOrderId() {
            return OrderId;
        }
        
        public double GetTotalCost() {
            return TotalCost;
        }
    }

    public class BillModel {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double TotalCost { get; set; }

        public BillModel(int id, int id_servicio, double total)
        {
            Id = id;
            OrderId = id_servicio;
            TotalCost = total;
        }
    }

}