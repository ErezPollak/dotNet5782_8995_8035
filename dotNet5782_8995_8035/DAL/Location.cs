using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace IDAL
    {
        namespace DO
        {
            public class Location
            {
                public double Longitude { get; set; }
                public double Lattitude { get; set; }

                public override string ToString()
                {
                    return
                       $"longitude: {(int)(this.Longitude)}° {(int)((this.Longitude - (int)(this.Longitude)) * 60)}' {((this.Longitude - (int)(this.Longitude)) * 60 - (int)((this.Longitude - (int)(this.Longitude)) * 60)) * 60}'' S ," +
                       $" lattitude: {(int)(this.Lattitude)}° {(int)((this.Lattitude - (int)(this.Lattitude)) * 60)}' {((this.Lattitude - (int)(this.Lattitude)) * 60 - (int)((this.Lattitude - (int)(this.Lattitude)) * 60)) * 60}'' H ,";
                }
            }
        }
    }


