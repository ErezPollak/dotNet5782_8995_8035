using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IBAL.BO.Location Location { get; set; }
            public int ChargeSlots { get; set; }
            public List<IBAL.BO.DroneInCharge> ChargingDrones { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string str = $"Base Station: " +
                    $"ID: {this.Id}. \n" +
                    $"Name: {this.Name}. \n" +
                    $"Location: {this.Location}. \n" +
                    $"ChargeSlots: {this.ChargeSlots}. \n" +
                    $"drones: {ChargingDrones.Count} \n";
                    this.ChargingDrones.ForEach(cd => str += cd.ToString());

                return str;
            
            }
        }
    }
}
