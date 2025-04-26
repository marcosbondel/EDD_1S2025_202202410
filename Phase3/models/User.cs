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
