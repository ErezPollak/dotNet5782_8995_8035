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
            public List<IBAL.BO.Drone> CargingDrones { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"Base Station: " +
                    $"ID: {this.Id}." +
                    $"Name: {this.Name}. " +
                    $"Location: {this.Location}." +
                    $"ChargeSlots: {this.ChargeSlots}." +
                    $"drones: {this.CargingDrones}." +
                    $"";
            }
        }
    }
}
