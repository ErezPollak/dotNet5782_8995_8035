namespace IBAL
{
    namespace BO
    {
        public class DroneInCharge
        {
            public int Id { get; set; }
            public double Battary { get; set; }

            public override string ToString()
            {
                return $"Id: {Id} " +
                    $"Battary: {Battary} " +
                    $"";  
            }
        }
    }
}