using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    /// <summary>
    /// the creator of the Bl acccoding to the singulary
    /// </summary>
    public class BlFactory
    {
        /// <summary>
        /// the function retuens an instance of bl.
        /// </summary>
        /// <returns></returns>
        public static IBL GetBl()
        {
            return  BL.GetInstance();
        }
    }
}
