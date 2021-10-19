using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int droneId { get; set; }
            public int stationId { get; set; }

            public string toString()
            {
                return $"DroneCharge: droneld: {droneId} , stationled {stationId}\n";
            }

        }
    }
}
