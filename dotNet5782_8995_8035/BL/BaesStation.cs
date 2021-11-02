using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class BaesStation
        {
            public int id { get; set; }
            public string name { get; set; }
            public double longitude { get; set; }
            public double lattitude { get; set; }
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
                   // printing the location according to the location saved in the memory. 
                   //at first printing the 
                   $"longitude: {(int)(this.longitude)}° {(int)((this.longitude - (int)(this.longitude)) * 60)}' {((this.longitude - (int)(this.longitude)) * 60 - (int)((this.longitude - (int)(this.longitude)) * 60)) * 60}'' ," +
                   $" lattitude: {(int)(this.lattitude)}° {(int)((this.lattitude - (int)(this.lattitude)) * 60)}' {((this.lattitude - (int)(this.lattitude)) * 60 - (int)((this.lattitude - (int)(this.lattitude)) * 60)) * 60}'' ," +
                   $"chargeslotes: {this.chargeSlots}\n";
            }
        }
    }
}
