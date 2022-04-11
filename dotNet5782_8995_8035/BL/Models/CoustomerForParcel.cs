
    namespace BL.Models
    {
        public record CoustomerForParcel
        (
            int Id = default,
            string CustomerName = default
        )
        {
            public override string ToString() =>
                $"ID: {Id} " +
                $"customer name: {CustomerName} " +
                $"";
        }
    }