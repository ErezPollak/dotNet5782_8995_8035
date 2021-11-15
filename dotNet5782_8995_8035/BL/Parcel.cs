using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public IBAL.BO.CoustomerForParcel Sender { get; set; }
            public IBAL.BO.CoustomerForParcel Reciver { get; set; }
            public IBAL.BO.Enums.WeightCategories Weight { get; set; }
            public IBAL.BO.Enums.Priorities Priority { get; set; }
            public IBAL.BO.DroneForParcel Drone { get; set; }
            public DateTime? RequestedTime { get; set; } // time of request
            public DateTime? AcceptedTime { get; set; } //
            public DateTime? PickupTime { get; set; }
            public DateTime? DeliveringTime { get; set; }


            public override string ToString()
            {
                return $"Id: {this.Id} " +
                    $"Sender: {this.Sender} " +
                    $"Reciver: {this.Reciver} " +
                    $"Weight: {this.Weight} " +
                    $"Priority: {this.Priority} " +
                    $"Drone: {this.Drone} " +
                    $"CreationTime: {this.RequestedTime} " +
                    $"AssigningTime: {this.AcceptedTime} " +
                    $"PickupTime: {this.PickupTime} " +
                    $"DeliveringTime: {this.DeliveringTime} " +
                    $"";
            }
        }
    }
}
