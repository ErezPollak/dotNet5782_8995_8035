
    namespace BO
    {
        public record DroneForParcel
        (
            int Id = default,
            int Battery = default,
            Location CurrentLocation = default
        )
        {
            public override string ToString() =>
                $"ID: {Id} " +
                $"battery: {Battery} " +
                $"location: {CurrentLocation}" +
                $"";
        }
    }