namespace IBAL
{
    namespace BO
    {
        public record ParcelInDelivery
        (
            int Id = default,
            Enums.ParcelStatus Status = default,
            Enums.Priorities Priority = default,
            Enums.WeightCategories Weight = default,
            CoustomerForParcel Sender = default,
            CoustomerForParcel Reciver = default,
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
                $"Receiver: {Reciver} " +
                $"PickupLocation: {PickupLocation} " +
                $"Distance: {Distance} " +
                $"";
        }
    }
}