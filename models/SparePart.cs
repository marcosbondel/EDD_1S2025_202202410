using System;

namespace SparePart {
    public interface SparePartInterface {
        int GetId();
        string GetSparePartName();
        string GetDetails();
        double GetCost();
    }

    public unsafe struct SparePart: SparePartInterface {
        public int Id;
        public fixed char SparePartName[50];
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

        public string GetSparePartName() {
            fixed (char* ptr = SparePartName) {
                return GetFixedString(ptr, 50);
            }
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