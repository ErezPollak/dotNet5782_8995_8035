using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DalApi
{
    /// <summary>
    /// class manages reading configuration metadata from specified xml file
    /// </summary>
    public static class DalConfig
    {
        /// <summary>
        /// metadata about content of dll file 
        /// </summary>
        public class DalPackage
        {
            public string Name;
            public string PkgName;
            public string NameSpace;
            public string ClassName;
        }
        /// <summary>
        /// name of class being used for current configuration
        /// </summary>
        internal static readonly string DalName;
        /// <summary>
        /// dictionary containing metadata for all possible classes that can be used
        /// </summary>
        internal static readonly Dictionary<string, DalPackage> DalPackages;

        /// <summary>
        /// constructor
        /// </summary>
        static DalConfig()
        {
            var dalConfig = XElement.Load(@"..\xml\config.xml");
            DalName = dalConfig.Element("dl")?.Value;
            DalPackages = (from pkg in dalConfig.Element("dl-packages")?.Elements()
                           let tmp1 = pkg.Attribute("namespace")
                           let nameSpace = tmp1 == null ? "Dal" : tmp1.Value
                           let tmp2 = pkg.Attribute("class")
                           let className = tmp2 == null ? pkg.Value : tmp2.Value
                           select new DalPackage()
                           {
                               Name = "" + pkg.Name,
                               PkgName = pkg.Value,
                               NameSpace = nameSpace,
                               ClassName = className
                           })
                .ToDictionary(p => "" + p.Name, p => p);
        }
    }

    [Serializable]
    internal class DalConfigException : Exception
    {
        public DalConfigException()
        {
        }

        public DalConfigException(string message) : base(message)
        {
        }

        public DalConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
