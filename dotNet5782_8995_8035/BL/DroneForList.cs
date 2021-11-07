﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class DroneForList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public IBAL.BO.Enums.WeightCategories Weight { get; set; }
            public int Battary { get; set; }
            public IBAL.BO.Enums.DroneStatuses Status { get; set; }
            public Location Location { get; set; }
            public int ParcelId { get; set; }

            public override string ToString()
            { 



            }

        }
    }
}