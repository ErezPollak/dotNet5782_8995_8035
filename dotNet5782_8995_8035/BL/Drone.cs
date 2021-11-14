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
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public Enums.WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public IBAL.BO.Enums.DroneStatuses Status { get; set; }
            public IBAL.BO.ParcelInDelivery ParcelInDelivery { get; set; }
            public Location Location { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"ID: {this.Id} " +
                    $"Model: {this.Model} " +
                    $"max Weight: {this.MaxWeight} " +
                    $"status: {this.Status}" +
                    $"battary: {this.Battery} " +
                    $"parcel in delivery: {this.ParcelInDelivery} " +
                    $"locstion: {this.Location} " +
                    $"";
            }
        }
    }
}
