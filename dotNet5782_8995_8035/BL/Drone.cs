﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBAL.BO.Enums;

namespace IBAL
{
    namespace BO
    {
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public Location location { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                ///the function returns the current place of the item's properties.///

                return $"Drone: id: {Id} , model: {Model} , MaxWeight: {MaxWeight} " +
                    $", Status: {Status}, battery: {Battery} , Location: {location}\n";
            }
        }
    }
}
