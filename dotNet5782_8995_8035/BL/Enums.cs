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
            public enum Priorities { REGULAR, FAST, URGENT };
            public enum DroneStatuses { FREE , DELIVERY , MAINTENANCE };
            public enum ParcelStatus { DEFINED , ASSIGNED , PICKEDUP , DELIVERED };
            public enum CustomerEnum {SENDER, TARGET };
        }
    }
}
