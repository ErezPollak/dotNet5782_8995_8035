
    namespace BL.Models
    {
        public record DroneInCharge
        (
            int Id = default,
            double Battery = default
        )
        {
            public override string ToString() =>
                $"Id: {Id} " +
                $"Battery: {Battery} " +
                $"";
        }
    }