using System;

namespace Model {
    public interface UserInterface {
        int GetId();
        string GetName();
        string GetLastname();
        string GetEmail();
        string GetPassword();
        string GetFullname();
        int GetAge();
    }

    // public unsafe struct User : UserInterface {
    //     public int Id;
    //     public fixed char Name[50];
    //     public fixed char Lastname[50];
    //     public fixed char Email[100];
    //     public fixed char Password[50];
    //     public int Age;

    //     public void SetFixedString(char* destination, string source, int maxLength) {
    //         int length = Math.Min(source.Length, maxLength - 1); // Leave space for null-terminator
    //         for (int i = 0; i < length; i++)
    //         {
    //             destination[i] = source[i];
    //         }
    //         destination[length] = '\0'; // Null-terminate (optional)
    //     }

    //     public string GetFixedString(char* source, int maxLength) {
    //         return new string(source).TrimEnd('\0'); // Convert and trim nulls
    //     }

    //     public override string ToString() {
    //         fixed (char* firstNamePtr = Name)
    //         fixed (char* lastNamePtr = Lastname)
    //         fixed (char* emailPtr = Email)
    //         fixed (char* passwordPtr = Password) {
    //             return $"Id: {Id}\n" +
    //                    $"First Name: {GetFixedString(firstNamePtr, 50)}\n" +
    //                    $"Last Name: {GetFixedString(lastNamePtr, 50)}\n" +
    //                    $"Email: {GetFixedString(emailPtr, 100)}\n" +
    //                    $"Edad: {Age}\n" +
    //                    $"Password: {GetFixedString(passwordPtr, 50)}\n";
    //         }
    //     }

    //     public int GetId() {
    //         return Id;
    //     }
        
    //     public int GetAge() {
    //         return Age;
    //     }

    //     public string GetName() {
    //         fixed (char* ptr = Name) {
    //             return GetFixedString(ptr, 50);
    //         }
    //     }
    //     public string GetLastname() {
    //         fixed (char* ptr = Lastname) {
    //             return GetFixedString(ptr, 50);
    //         }
    //     }
    //     public string GetEmail() {
    //         fixed (char* ptr = Email) {
    //             return GetFixedString(ptr, 100);
    //         }
    //     }
    //     public string GetPassword() {
    //         fixed (char* ptr = Password) {
    //             return GetFixedString(ptr, 50);
    //         }
    //     }

    //     public string GetFullname(){
    //         fixed (char* namePtr = Name) 
    //         fixed (char* lastnamePtr = Lastname) {
    //             string name = GetFixedString(namePtr, 50);
    //             string lastname = GetFixedString(lastnamePtr, 50);
    //             // return GetFixedString(namePtr, 50);
    //             return $"{name} {lastname}";
    //         }
    //     }

    // }

    public class UserImport
    {
        public int ID { get; set; }
        public int Edad { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
    }

    public class User {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }

        public User(int Id, string Name, string Lastname, string Email, string Password, int Age){
            this.Id = Id;
            this.Name = Name;
            this.Lastname = Lastname;
            this.Email = Email;
            this.Password = Password;
            this.Age = Age;
        }

        public User(){ }

        public string GetFullname(){
            return $"{Name} {Lastname}";
        }
    }

}
