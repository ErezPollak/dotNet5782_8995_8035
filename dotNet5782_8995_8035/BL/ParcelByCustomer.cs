using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
        public class ParcelByCustomer
        {
            public int Id { get; set; }
            public IBAL.BO.Enums.WeightCategories Weight { get; set; }
            public IBAL.BO.Enums.Priorities Priority { get; set; }
            public IBAL.BO.Enums.ParcelStatus Status { get; set; }
            public IBAL.BO.CoustomerForParcel SenderOrReciver { get; set; }

            public override string ToString()
            {
                return $"Weight: {this.Weight} " +
                    $"Priority: {this.Priority} " +
                    $"Status: {this.Status} " +
                    $"SenderOrReciver: {this.SenderOrReciver} " +
                    $"";
            }


        }
    }
}
