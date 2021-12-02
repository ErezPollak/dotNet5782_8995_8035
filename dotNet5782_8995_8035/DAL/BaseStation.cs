//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program contains a struct that represents a base station.
//the program contains the properties and a tostring function.

namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
           
            public int Id { get; set; }
            public string Name { get; set; }
            public Location Location { get; set; }
            public int ChargeSlots { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {

                //the function returns the current place of the item's properties.//

                return $"Base Station:" +
                   $" ID: {Id}, " +
                   $"Name: {Name}, " +
                   $"longitude: {Location}" +
                   $"chargeslotes: {ChargeSlots}\n";
            }

        }
    }
}
