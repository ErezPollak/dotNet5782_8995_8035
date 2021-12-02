using System.Collections.Generic;

namespace IBAL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location { get; set; }
            private List<ParcelByCustomer> FromCustomer  { get; set; }
            private List<ParcelByCustomer> ToCustomer { get; set; }


            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {

                return $"Customer: " +
                    $"id: {Id} , " +
                    $"name: {Name} , " +
                    $"phone : {Phone} , " +
                    $"location: {Location} \n" +
                    $"the parcels that this customer got: {FromCustomer} \n" +
                    $"the parcels that this customer sent: {ToCustomer}" +
                    $"";

            }
        }
    }
}
