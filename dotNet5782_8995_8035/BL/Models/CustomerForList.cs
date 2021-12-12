
    namespace BO
    {
        public record CustomerForList
        (
            int Id = default,
            string Name = default,
            string Phone = default,
            int SentToAndDeliverd = default,
            int SentToAnDNotDelivered = default,
            int SentFromAndDeliverd = default,
            int SentFromAndNotDeliverd = default
        )
        {
            public override string ToString() =>
                $"id: {Id} " +
                $"name: {Name} " +
                $"phone {Phone} " +
                $"finished parcels: {SentToAndDeliverd} " +
                $"not finished parcels: {SentToAnDNotDelivered} " +
                $"recived: {SentFromAndDeliverd} " +
                $"parcels on the way: {SentFromAndNotDeliverd} " +
                $"";
        }
    }