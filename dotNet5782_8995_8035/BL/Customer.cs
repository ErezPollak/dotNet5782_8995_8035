using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public IBAL.BO.Location Location { get; set; }
            public List<IBAL.BO.ParcelByCustomer> fromCustomer  { get; set; }
            public List<IBAL.BO.ParcelByCustomer> ToCustomer { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {

                return $"Customer: " +
                    $"id: {Id} , " +
                    $"name: {Name} , " +
                    $"phone : {Phone } , " +
                    $"location: {Location.ToString()}";
            }
        }
    }
}
