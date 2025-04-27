namespace Graphs{
    public class AutoSparePartNode
    {
        public string IdVehiculo { get; set; } 
        public string IdRepuesto { get; set; } 

        
        public AutoSparePartNode(string idVehiculo, string idRepuesto)
        {
            IdVehiculo = idVehiculo;
            IdRepuesto = idRepuesto;
        }

        // Estos métodos ayudan a comparar nodos
        public override bool Equals(object obj)
        {
            if (obj is AutoSparePartNode otro)
            {
                return IdVehiculo == otro.IdVehiculo && IdRepuesto == otro.IdRepuesto;
            }
            return false;
        }

        // Este método genera un código único para cada nodo.
        public override int GetHashCode()
        {
            return HashCode.Combine(IdVehiculo, IdRepuesto);
        }
    }
}