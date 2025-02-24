using System;

namespace Model {
    public interface AutomobileInterface {
        int GetId();
        int GetUserId();
        string GetBrand();
        string GetModel();
        string GetPlate();
    }

    public unsafe struct Automobile : AutomobileInterface {
        public int Id;
        public int UserId;
        public fixed char Brand[50];
        public fixed char Model[50];
        public fixed char Plate[50];
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
        
        public int GetUserId() {
            return UserId;
        }

        public string GetBrand() {
            fixed (char* ptr = Brand) {
                return GetFixedString(ptr, 50);
            }
        }
        
        public string GetModel() {
            fixed (char* ptr = Model) {
                return GetFixedString(ptr, 50);
            }
        }
        
        public string GetPlate() {
            fixed (char* ptr = Plate) {
                return GetFixedString(ptr, 50);
            }
        }

        public void SetUserId(int userId) {
            UserId = userId;
        }

        public override string ToString() {
            fixed (char* brandPtr = Brand)
            fixed (char* modelPtr = Model)
            fixed (char* platePtr = Plate) {
                return $"Id: {Id}\n" +
                       $"Brand: {GetFixedString(brandPtr, 50)}\n" +
                       $"Plate: {GetFixedString(platePtr, 50)}\n" +
                       $"Model: {GetFixedString(modelPtr, 50)}\n";
            }
        }
    }
}