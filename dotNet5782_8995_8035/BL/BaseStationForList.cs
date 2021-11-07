using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class BaseStationForList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int FreeChargingSlots { get; set; }
            public int TakenCharingSlots { get; set; }

            public override string ToString()
            {


            }
        }
    }
}
