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
            public double longitude { get; set; }
            public double lattitude { get; set; }
            public int chargeSlots { get; set; }


            public string toString()
            {

                ///the function returns the current place of the item's properties.///

                return $"Base Station:" +
                   $" ID: {this.id}, " +
                   $"Name: {this.name}, " +
                   // printing the location according to the location saved in the memory. 
                   //at first printing the 
                   $"longitude: {(int)(this.longitude)}° {(int)((this.longitude - (int)(this.longitude))*60)}' {((this.longitude - (int)(this.longitude)) * 60 - (int)((this.longitude - (int)(this.longitude)) * 60))*60}'' ," +
                   $" lattitude: {(int)(this.lattitude)}° {(int)((this.lattitude - (int)(this.lattitude)) * 60)}' {((this.lattitude - (int)(this.lattitude)) * 60 - (int)((this.lattitude - (int)(this.lattitude)) * 60)) * 60}'' ," +
                   $"chargeslotes: {this.chargeSlots}\n";
            }

        }
    }
}
