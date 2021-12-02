namespace IBAL
{
    namespace BO
    {
        public class CustomerForList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int SentToAndDeliverd { get; set; }
            public int SentToAnDNotDelivered { get; set; }
            public int SentFromAndDeliverd { get; set; }
            public int SentFromAndNotDeliverd { get; set; }

            public override string ToString()
            {
                return $"id: {Id} " +
                    $"name: {Name} " +
                    $"phone {Phone} " +
                    $"finished parcels: {SentToAndDeliverd} " +
                    $"not finished parcels: {SentToAnDNotDelivered} " +
                    $"recived: {SentFromAndDeliverd} " +
                    $"parcels on the way: {SentFromAndNotDeliverd} " +
                    $"";
            }
        }
    }
}
