
    namespace BL.Models
    {
        public record ParcelInDelivery
        (
            int Id = default,
            Enums.ParcelStatus Status = default,
            Enums.Priorities Priority = default,
            Enums.WeightCategories Weight = default,
            CoustomerForParcel Sender = default,
            CoustomerForParcel Receiver = default,
            Location PickupLocation = default,
            Location DeliveringLocation = default,
            double Distance = default
        )
        {
            public override string ToString() =>
                $"Id: {Id} " +
                $"Status: {Status} " +
                $"Priority: {Priority} " +
                $"Weight: {Weight} " +
                $"Sender: {Sender} " +
                $"Receiver: {Receiver} " +
                $"PickupLocation: {PickupLocation} " +
                $"Distance: {Distance} " +
                $"";
        }
    }