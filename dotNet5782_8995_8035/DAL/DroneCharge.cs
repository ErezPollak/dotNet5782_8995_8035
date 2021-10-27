//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program contains a struct class that represents a connection between a drone and a base station.
//the program contains the properties and a tostring function.


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

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public string ToString()
            {

                ///the function returns the current place of the item's properties.///

                return $"DroneCharge: droneld: {droneId} , stationled {stationId}\n";
            }

        }
    }
}
