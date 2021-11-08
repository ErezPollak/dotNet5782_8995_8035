//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program contains a struct that represents a base station.
//the program contains the properties and a tostring function.
using System;

namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
           
            public int id { get; set; }
            public string name { get; set; }
            public IDAL.DO.Location Location { get; set; }
            public int chargeSlots { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {

                ///the function returns the current place of the item's properties.///

                return $"Base Station:" +
                   $" ID: {this.id}, " +
                   $"Name: {this.name}, " +
                   $"longitude: {Location}" +
                   $"chargeslotes: {this.chargeSlots}\n";
            }

        }
    }
}
