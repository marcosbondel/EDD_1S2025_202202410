using System;

namespace Model {

    public interface ServiceInterface {
        int GetId();
        int GetSparePartId();
        int GetAutomobileId();
        string GetDetails();
        double GetCost();
    }

    public class Service {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public int AutomobileId { get; set; }
        public string Details { get; set; }
        public double Cost { get; set; }                // Altura del nodo

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