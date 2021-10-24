//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program contains a struct class that represents a drone.
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
        public struct Drone
        {
            public int id { get; set; }
            public string model { get; set; }
            public WeightCategories MaxWeight{ get; set; }
            public DroneStatuses Status{ get; set; }
            public double battery { get; set; }

            public string toString()
            {
                ///the function returns the current place of the item's properties.///

                return $"Drone: id: {id} , model: {model} , MaxWeight: {MaxWeight} " +
                    $", Status: {Status}, battery: {battery}\n";
            }

        }
    }
}
