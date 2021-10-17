using System;

namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
           
            public int id { get; set; }
            public int name { get; set; }
            public double longitude { get; set; }
            public double lattitude { get; set; }
            public int chargeSlots { get; set; }

           
            public string toString()
            {
                return $"Base Station: ID: {this.id}, Name: {this.name}, " +
                    $"longitude: {this.longitude}, lattitude: {this.lattitude}, " +
                    $"chargeslotes: {this.chargeSlots}\n";
            }

        }
    }
}
