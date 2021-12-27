using System.Collections.Generic;

    namespace BO
    {
        public record BaseStation
        (
            int Id = default,
            string Name = default,
            Location Location = default,
            int ChargeSlots = default,
            IEnumerable<DroneInCharge> ChargingDrones = default
        )

        {
            public override string ToString()
            {
                string str = $"Base Station: " +
                             $"ID: {Id}. \n" +
                             $"Name: {Name}. \n" +
                             $"Location: {Location}. \n" +
                             $"ChargeSlots: {ChargeSlots}. \n" +
                             $"drones: {ChargingDrones} \n";
                
            //ChargingDrones.ForEach(cd => str += cd.ToString());

                return str;
            }
        }
    }