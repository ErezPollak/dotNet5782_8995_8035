//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the students: Erez Polak 322768995



//the program contains a struct that represents a base station.
//the program contains the properties and a toString function.

using DO;

namespace DalFacade.Models
    {
        public struct BaseStation
        {
           
            public int Id { get; init; }
            public string Name { get; set; }
            public Location Location { get; init; }
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
                   $"chargeSlots: {ChargeSlots}\n";
            }

        }
    }
