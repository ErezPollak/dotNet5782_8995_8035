using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public IBAL.BO.CoustomerForParcel Sender { get; set; }
            public IBAL.BO.CoustomerForParcel Reciver { get; set; }
            public IBAL.BO.Enums.WeightCategories Weight { get; set; }
            public IBAL.BO.Enums.Priorities Prioritie { get; set; }
            public IBAL.BO.DroneForParcel Drone { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime AssigningTime { get; set; }
            public DateTime PickupTime { get; set; }
            public DateTime deliveringTime { get; set; }


            public override string ToString()
            { 

            }
        }
    }
}
