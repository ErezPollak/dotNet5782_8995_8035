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
            public double longitude { get; set; }
            public double lattitude { get; set; }

            public override string ToString()
            {
                return
                   $"longitude: {(int)(this.longitude)}° {(int)((this.longitude - (int)(this.longitude)) * 60)}' {((this.longitude - (int)(this.longitude)) * 60 - (int)((this.longitude - (int)(this.longitude)) * 60)) * 60}'' S ," +
                   $" lattitude: {(int)(this.lattitude)}° {(int)((this.lattitude - (int)(this.lattitude)) * 60)}' {((this.lattitude - (int)(this.lattitude)) * 60 - (int)((this.lattitude - (int)(this.lattitude)) * 60)) * 60}'' H ,";
            }
        }
    }
}
