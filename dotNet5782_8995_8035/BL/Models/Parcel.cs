using System;

namespace BL.Models
    {
        public class Parcel
        {
            public int Id { get; init; }
            public CoustomerForParcel Sender { get; init; }
            public CoustomerForParcel Receiver { get; init; }
            public Enums.WeightCategories Weight { get; init; }
            public Enums.Priorities Priority { get; init; }
            public DroneForParcel Drone { get; init; }
            public DateTime? DefinedTime { get; set; } // time of request
            public DateTime? AssignedTime { get; init; } //
            public DateTime? PickupTime { get; init; }
            public DateTime? DeliveringTime { get; set; }


            public override string ToString()
            {
                return $"Id: {Id} " +
                    $"Sender: {Sender} " +
                    $"Receiver: {Receiver} " +
                    $"Weight: {Weight} " +
                    $"Priority: {Priority} " +
                    $"Drone: {Drone} " +
                    $"CreationTime: {DefinedTime} " +
                    $"AssigningTime: {AssignedTime} " +
                    $"PickupTime: {PickupTime} " +
                    $"DeliveringTime: {DeliveringTime} " +
                    $"";
            }
        }
    }
