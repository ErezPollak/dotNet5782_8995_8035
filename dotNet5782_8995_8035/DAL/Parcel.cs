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
            public int senderld { get; set; }
            public int targetld { get; set; }
            public WeghitCategories weight { get; set; }
            public Priorities priority { get; set; }
            public DateTime requested { get; set; }
            public int droneld { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }


            public string toString()
            {
                return $"Parcel: id: {id} , senderld: {senderld} , targetld : {targetld} " +
                    $", weight: {weight}, priority: {priority}, requested: {requested}, " +
                    $"droneld: {droneld}, scheduled: {scheduled}, pickedUp: {pickedUp}, " +
                    $"delivered: {delivered}\n";
            }

        }
    }
}
