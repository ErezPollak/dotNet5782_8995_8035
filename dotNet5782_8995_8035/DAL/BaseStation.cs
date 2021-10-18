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
                return $"Base Station:" +
                   $" ID: {this.id}, " +
                   $"Name: {this.name}, " +
                   $"longitude: {(int)(this.longitude)}° {(int)((this.longitude - (int)(this.longitude))*60)}' {((this.longitude - (int)(this.longitude)) * 60 - (int)((this.longitude - (int)(this.longitude)) * 60))*60}'' ," +
                   $" lattitude: {(int)(this.lattitude)}° {(int)((this.lattitude - (int)(this.lattitude)) * 60)}' {((this.lattitude - (int)(this.lattitude)) * 60 - (int)((this.lattitude - (int)(this.lattitude)) * 60)) * 60}'' ," +
                   $"chargeslotes: {this.chargeSlots}\n";
            }

        }
    }
}
