using System;

namespace Model {
    public interface UserInterface {
        int GetId();
        string GetName();
        string GetLastname();
        string GetEmail();
        string GetPassword();
        string GetFullname();
    }

    public unsafe struct User : UserInterface {
        public int Id;
        public fixed char Name[50];
        public fixed char Lastname[50];
        public fixed char Email[100];
        public fixed char Password[50];

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

        public override string ToString() {
            fixed (char* firstNamePtr = Name)
            fixed (char* lastNamePtr = Lastname)
            fixed (char* emailPtr = Email)
            fixed (char* passwordPtr = Password) {
                return $"Id: {Id}\n" +
                       $"First Name: {GetFixedString(firstNamePtr, 50)}\n" +
                       $"Last Name: {GetFixedString(lastNamePtr, 50)}\n" +
                       $"Email: {GetFixedString(emailPtr, 100)}\n" +
                       $"Password: {GetFixedString(passwordPtr, 50)}\n";
            }
        }

        public int GetId() {
            return Id;
        }

        public string GetName() {
            fixed (char* ptr = Name) {
                return GetFixedString(ptr, 50);
            }
        }
        public string GetLastname() {
            fixed (char* ptr = Lastname) {
                return GetFixedString(ptr, 50);
            }
        }
        public string GetEmail() {
            fixed (char* ptr = Email) {
                return GetFixedString(ptr, 100);
            }
        }
        public string GetPassword() {
            fixed (char* ptr = Password) {
                return GetFixedString(ptr, 50);
            }
        }

        public string GetFullname(){
            fixed (char* namePtr = Name) 
            fixed (char* lastnamePtr = Lastname) {
                string name = GetFixedString(namePtr, 50);
                string lastname = GetFixedString(lastnamePtr, 50);
                // return GetFixedString(namePtr, 50);
                return $"{name} {lastname}";
            }
        }

    }

    public class UserImport
    {
        public int ID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
    }

}
