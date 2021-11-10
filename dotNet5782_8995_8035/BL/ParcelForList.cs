using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAL
{
    namespace BO
    {
         public class ParcelForList
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string reciverName { get; set; }
            public IBAL.BO.Enums.WeightCategories Weight { get; set; }
            public IBAL.BO.Enums.Priorities Priority { get; set; }
            public IBAL.BO.Enums.ParcelStatus Status { get; set; }

            //public override string ToString()
            //{


            //}

        }
    }
}
