using System;

namespace Model {

    public interface ServiceInterface {
        int GetId();
        int GetSparePartId();
        int GetAutomobileId();
        string GetDetails();
        double GetCost();
    }

    public unsafe struct Service : ServiceInterface {
        public int Id;
        public int SparePartId;
        public int AutomobileId;
        public fixed char Details[200];
        public double Cost;

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
        
        public int GetSparePartId() {
            return SparePartId;
        }
        
        public int GetAutomobileId() {
            return AutomobileId;
        }

        public string GetDetails() {
            fixed (char* ptr = Details) {
                return GetFixedString(ptr, 200);
            }
        }
        
        public double GetCost() {
            return Cost;
        }
    }

    public class ServiceModel {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public int AutomobileId { get; set; }
        public string Details { get; set; }
        public double Cost { get; set; }                // Altura del nodo

        public ServiceModel(int id, int SparePartId, int AutomobileId, string Details, double Cost)
        {
            Id = id;
            this.SparePartId = SparePartId;
            this.AutomobileId = AutomobileId;
            this.Details = Details;
            this.Cost = Cost;
        }
    }

}