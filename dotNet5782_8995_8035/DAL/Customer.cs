﻿using System;
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
                return $"Customer: " +
                    $"id: {id} , " +
                    $"name: {name} , " +
                    $"phone : {phone } " +
                    $"longitude: {(int)(this.longitude)}° {(int)((this.longitude - (int)(this.longitude)) * 60)}' {((this.longitude - (int)(this.longitude)) * 60 - (int)((this.longitude - (int)(this.longitude)) * 60)) * 60}'' ," +
                    $" lattitude: {(int)(this.lattitude)}° {(int)((this.lattitude - (int)(this.lattitude)) * 60)}' {((this.lattitude - (int)(this.lattitude)) * 60 - (int)((this.lattitude - (int)(this.lattitude)) * 60)) * 60}'' ,";
          
            }

        }
    }
}
