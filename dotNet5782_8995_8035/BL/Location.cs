using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class Location
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }

            public override string ToString()
            {
                return
                   $"longitude: {(int)(this.Longitude)}° {(int)((this.Longitude - (int)(this.Longitude)) * 60)}' {((this.Longitude - (int)(this.Longitude)) * 60 - (int)((this.Longitude - (int)(this.Longitude)) * 60)) * 60}'' S ," +
                   $" lattitude: {(int)(this.Latitude)}° {(int)((this.Latitude - (int)(this.Latitude)) * 60)}' {((this.Latitude - (int)(this.Latitude)) * 60 - (int)((this.Latitude - (int)(this.Latitude)) * 60)) * 60}'' H ,";
            }
        }
    }
}
