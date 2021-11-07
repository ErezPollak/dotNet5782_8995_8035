using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class Enums
        {
            public enum WeightCategories { LIGHT, MIDIUM, HEAVY };
            public enum Priorities { REGULAR, FAST, EMERGENCY };
            public enum DroneStatuses { FREE , DELIVERY , FIXING }
            public enum ParcelStatus { DEFINED , ASSIGNED , PICKEDUP , DELIVERED }
        }
    }
}
