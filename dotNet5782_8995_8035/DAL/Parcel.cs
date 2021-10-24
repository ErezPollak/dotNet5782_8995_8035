//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program contains a struct class that represents a parcel.
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
        public struct Parcel
        {
            public int id { get; set; }
            public int senderId { get; set; }
            public int targetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities priority { get; set; }
            public DateTime requested { get; set; }
            public int droneId { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }


            public string toString()
            {

                ///the function returns the current place of the item's properties.///

                return $"Parcel: id: {id} , senderld: {senderId} , targetld : {targetId} " +
                    $", weight: {Weight}, priority: {priority},  droneld: {droneId}, " +
                    $"requested: {requested}, scheduled: {scheduled},delivered: {delivered},  " +
                    $"pickedUp: {pickedUp}\n";
            }

        }
    }
}
