using System;

namespace Model {
    public interface AutomobileInterface {
        int GetId();
        int GetUserId();
        string GetBrand();
        string GetModel();
        string GetPlate();
    }


    public class AutomobileImport
    {
        public int ID { get; set; }
        public int ID_Usuario { get; set; }
        public string Marca { get; set; }
        public int Modelo { get; set; }
        public string Placa { get; set; }
    }

    public class Automobile {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public Automobile(int id, int userId, string brand, string model, string plate) {
            Id = id;
            UserId = userId;
            Brand = brand;
            Model = model;
            Plate = plate;
        }

        public Automobile(){}
    }
}