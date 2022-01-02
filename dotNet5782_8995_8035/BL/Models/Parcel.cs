using System;


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
            public DateTime? DefinedTime { get; set; } // time of request
            public DateTime? AssigedTime { get; set; } //
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
                    $"CreationTime: {DefinedTime} " +
                    $"AssigningTime: {AssigedTime} " +
                    $"PickupTime: {PickupTime} " +
                    $"DeliveringTime: {DeliveringTime} " +
                    $"";
            }
        }
    }
