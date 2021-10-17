using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public double longitude { get; set; }
            public double lattitude { get; set; }

            public string toString()
            {
                return $"Customer: id: {id} , name: {name} , phone : {phone } " +
                    $", longitude: {longitude}, lattitude: {lattitude}\n";
            }

        }
    }
}
