using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBAL.BO.Enums;

namespace IBAL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {

                ///the function returns the current place of the item's properties.///

                return $"Parcel: id: {Id} , senderld: {SenderId} , targetld : {TargetId} " +
                    $", weight: {Weight}, priority: {priority},  droneld: {DroneId}, " +
                    $"requested: {Requested}, scheduled: {Scheduled},delivered: {Delivered},  " +
                    $"pickedUp: {PickedUp}\n";
            }
        }
    }
}
