﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IBAL.BO.Location Location { get; set; }
            public int ChargeSlots { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {

                ///the function returns the current place of the item's properties.///

                return $"Base Station:" +
                   $" ID: {this.Id}, " +
                   $"Name: {this.Name}, " +
                   $"Lockation: { this.Location.ToString()} ,"+
                   $"chargeslotes: {this.ChargeSlots}\n";
            }
        }
    }
}
