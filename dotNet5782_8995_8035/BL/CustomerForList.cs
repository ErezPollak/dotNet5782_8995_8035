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
            public int Phone { get; set; }
            public int SentAndDeliverd { get; set; }
            public int SentAnDNotDelivered { get; set; }
            public int Recived { get; set; }
            public int OnTheWay { get; set; }

            //public override string ToString()
            //{
            //}
        }
    }
}
