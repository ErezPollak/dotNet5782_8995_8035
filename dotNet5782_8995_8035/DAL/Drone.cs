using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int id { get; set; }
            public string model { get; set; }
            public WeightCategories MaxWeight{ get; set; }
            public DroneStatuses Status{ get; set; }
            public double battery { get; set; }

            public string toString()
            {
                return $"Drone: id: {id} , model: {model} , MaxWeight: {MaxWeight} " +
                    $", Status: {Status}, battery: {battery}\n";
            }

        }
    }
}
