using System.Collections.Generic;

namespace IBAL
{
    namespace BO
    {
        public class BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Location Location { get; set; }
            public int ChargeSlots { get; set; }
            public List<DroneInCharge> ChargingDrones { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string str = $"Base Station: " +
                    $"ID: {Id}. \n" +
                    $"Name: {Name}. \n" +
                    $"Location: {Location}. \n" +
                    $"ChargeSlots: {ChargeSlots}. \n" +
                    $"drones: {ChargingDrones.Count} \n";
                    ChargingDrones.ForEach(cd => str += cd.ToString());

                return str;
            
            }
        }
    }
}
