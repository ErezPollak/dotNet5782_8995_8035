using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return $"id: {this.Id} " +
                    $"name: {this.Name} " +
                    $"phone {this.Phone} " +
                    $"finished parcels: {this.SentToAndDeliverd} " +
                    $"not finished parcels: {this.SentToAnDNotDelivered} " +
                    $"recived: {this.SentFromAndDeliverd} " +
                    $"parcels on the way: {this.SentFromAndNotDeliverd} " +
                    $"";
            }
        }
    }
}
