using System;

namespace Model {

    public interface ServiceInterface {
        int GetId();
        int GetSparePartId();
        int GetAutomobileId();
        string GetDetails();
        double GetCost();
    }

    public class ServiceImport
    {
        public int ID { get; set; }
        public int ID_Repuesto { get; set; }
        public int ID_Vehiculo { get; set; }
        public string Detalles { get; set; }
        public double Costo { get; set; }                // Altura del nodo

    }


    public class Service {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public int AutomobileId { get; set; }
        public string Details { get; set; }
        public double Cost { get; set; }
        
        public Service(){ }           // Altura del nodo

        public Service(int id, int SparePartId, int AutomobileId, string Details, double Cost)
        {
            Id = id;
            this.SparePartId = SparePartId;
            this.AutomobileId = AutomobileId;
            this.Details = Details;
            this.Cost = Cost;
        }
    }

}