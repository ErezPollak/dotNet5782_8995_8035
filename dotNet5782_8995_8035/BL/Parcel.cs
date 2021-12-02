using System;

namespace IBAL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public CoustomerForParcel Sender { get; set; }
            public CoustomerForParcel Reciver { get; set; }
            public Enums.WeightCategories Weight { get; set; }
            public Enums.Priorities Priority { get; set; }
            public DroneForParcel Drone { get; set; }
            public DateTime? RequestedTime { get; set; } // time of request
            public DateTime? AcceptedTime { get; set; } //
            public DateTime? PickupTime { get; set; }
            public DateTime? DeliveringTime { get; set; }


            public override string ToString()
            {
                return $"Id: {Id} " +
                    $"Sender: {Sender} " +
                    $"Reciver: {Reciver} " +
                    $"Weight: {Weight} " +
                    $"Priority: {Priority} " +
                    $"Drone: {Drone} " +
                    $"CreationTime: {RequestedTime} " +
                    $"AssigningTime: {AcceptedTime} " +
                    $"PickupTime: {PickupTime} " +
                    $"DeliveringTime: {DeliveringTime} " +
                    $"";
            }
        }
    }
}
