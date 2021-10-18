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
            public int droneld { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }


            public string toString()
            {
                return $"Parcel: id: {id} , senderld: {senderId} , targetld : {targetId} " +
                    $", weight: {Weight}, priority: {priority},  droneld: {droneld}, " +
                    $"requested: {requested}, scheduled: {scheduled},delivered: {delivered},  " +
                    $"pickedUp: {pickedUp}\n";
            }

        }
    }
}
