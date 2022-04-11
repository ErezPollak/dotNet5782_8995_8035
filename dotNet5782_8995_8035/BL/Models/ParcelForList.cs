
    namespace BL.Models
    {
        public record ParcelForList
        (
            int Id = default,
            string SenderName = default,
            string ReceiverName = default,
            Enums.WeightCategories Weight = default,
            Enums.Priorities Priority = default,
            Enums.ParcelStatus Status = default
        )
        {
            public override string ToString() =>
                $"Id: {Id} " +
                $"SenderName: {SenderName} " +
                $"ReceiverName: {ReceiverName} " +
                $"Weight: {Weight} " +
                $"Priority: {Priority} " +
                $"Status: {Status} " +
                $"";
        }
    }