using System;

namespace Service {

    public interface ServiceInterface {
        int GetId();
        int GetSparePartId();
        int GetAutomobileId();
        string GetDetails();
        double GetCost();
    }

    public unsafe struct Service {
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
            return new string(source, 0, maxLength).TrimEnd('\0'); // Convert and trim nulls
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

}