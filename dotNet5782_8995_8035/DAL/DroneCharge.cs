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
            public int droneld { get; set; }
            public int stationled { get; set; }

            public string toString()
            {
                return $"DroneCharge: droneld: {droneld} , stationled {stationled}\n";
            }

        }
    }
}
