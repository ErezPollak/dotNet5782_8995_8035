using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class ParcelInDelivery
        {
            public int Id { get; set; }
            public IBAL.BO.Enums.ParcelStatus Status { get; set; }
            public IBAL.BO.Enums.Priorities Priority { get; set; }
            public IBAL.BO.Enums.WeightCategories Weight { get; set; }
            public IBAL.BO.CoustomerForParcel Sender { get; set; }
            public IBAL.BO.CoustomerForParcel Reciver { get; set; }
            public IBAL.BO.Location PickupLockation { get; set; }
            public IBAL.BO.Location DeliveringLockation { get; set; }
            public double Distance { get; set; }

            public override string ToString()
            {
                return $"Id: {this.Id} " +
                   $"Status: {this.Status} " +
                   $"Priority: {this.Priority} " +
                   $"Weight: {this.Weight} " +
                   $"Sender: {this.Sender} " +
                   $"reciver: {this.Reciver} " +
                   $"PickupLockation: {this.PickupLockation} " +
                   $"Distance: {this.Distance} " +
                   $"";
            }


        }
    }
}
