using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enums;

namespace IBAL
{
    namespace BO
    {
        public class Parcel
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

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
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
