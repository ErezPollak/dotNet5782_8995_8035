using System;

namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
           
            private int id { get; set; }
            private int name { get; set; }
            private double longitude { get; set; }
            private double lattitude { get; set; }
            private int chargeSlots { get; set; }

           
            public string toString()
            {
                return $"Base Station: ID: {this.id}, Name: {this.name}, " +
                    $"longitude: {this.longitude}, lattitude: {this.lattitude}, " +
                    $"chargeslotes: {this.chargeSlots}\n";
            }

        }
    }
}
