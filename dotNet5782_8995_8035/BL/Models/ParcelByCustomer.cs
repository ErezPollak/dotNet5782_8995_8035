
    namespace BL.Models
    {
        public record ParcelByCustomer
        (
            int Id = default,
            Enums.WeightCategories Weight = default,
            Enums.Priorities Priority = default,
            Enums.ParcelStatus Status = default,
            CoustomerForParcel SenderOrReceiver = default
        )
        {
            public override string ToString() =>
                $"Weight: {Weight} " +
                $"Priority: {Priority} " +
                $"Status: {Status} " +
                $"SenderOrReceiver: {SenderOrReceiver} " +
                $"";
        }
    }