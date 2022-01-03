using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// creates a new data center according to the reqest from the user.
    /// </summary>
    public class DalFactory
    {
        /// <summary>
        /// the function returns an instance of dal.
        /// </summary>
        /// <param name="dalType"></param>
        /// <returns></returns>
        public static IDal GetDal(string dalType)
        {
            if (dalType == "dalObject")
                return Dal.DalObject.GetInstance();
            else if (dalType == "dalXml")
                return Dal.DalXML.GetInstance();
            else
                throw new DO.NotSuchDataTypeException($"no such type as {dalType}.");

        } 
    }

}
