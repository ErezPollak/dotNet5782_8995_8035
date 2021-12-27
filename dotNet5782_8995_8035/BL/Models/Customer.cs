using System.Collections.Generic;

    namespace BO
    {
        public record Customer
        (
            int Id = default,
            string Name = default,
            string Phone = default,
            Location Location = default,
            IEnumerable<ParcelByCustomer> FromCustomer = default,
            IEnumerable<ParcelByCustomer> ToCustomer = default
        )
        {
            public override string ToString() =>
                $"Customer: " +
                $"id: {Id} , " +
                $"name: {Name} , " +
                $"phone : {Phone} , " +
                $"location: {Location} \n" +
                $"the parcels that this customer got: {FromCustomer} \n" +
                $"the parcels that this customer sent: {ToCustomer}" +
                $"";
        }
    }