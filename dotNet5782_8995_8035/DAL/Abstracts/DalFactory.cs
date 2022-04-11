using System;
using System.Collections.Generic;
using System.Reflection;

namespace DalApi
{
    /// <summary>
    /// Factory Design
    /// get an instance of an object from a class that implements the IDal interface
    /// </summary>
    public static class DalFactory
    {
        /// <summary>
        /// get object instance of class specified by the xml file
        /// </summary>
        /// <returns>instance of an object from class implementing Idal interface </returns>
        public static IDal GetDal()
        {
            var dalType = DalConfig.DalName;
            DalConfig.DalPackage dalPackage;

            try
            {
                dalPackage = DalConfig.DalPackages[dalType];
            }
            catch (KeyNotFoundException ex)
            {
                throw new DalConfigException($"Wrong Dal type: {dalType}", ex);
            }
            var dalPackageName = dalPackage.Name;
            var dalNameSpace = dalPackage.NameSpace;
            var dalClass = dalPackage.ClassName;

            try // Load into CLR the dal implementation assembly according to dll file name (taken above)
            {
                Assembly.Load(dalPackageName);
            }
            catch (Exception ex)
            {
                throw new DalConfigException($"Failed loading {dalPackageName}.dll", ex);
            }

            // Get concrete Dal implementation's class metadata object
            // 1st element in the list inside the string is full class name:
            //    namespace = "Dal" or as specified in the "namespace" attribute in the config file,
            //    class name = package name or as specified in the "class" attribute in the config file
            Type type;
            try
            {
                type = Type.GetType($"{dalNameSpace}.{dalClass}, {dalPackageName}", true);
            }
            catch (Exception ex)
            { // If the type is not found - the implementation is not correct - it looks like the class name is wrong...
                throw new DalConfigException($"Class not found due to a wrong namespace or/and class name: {dalPackageName}:{dalNameSpace}.{dalClass}", ex);
            }
            // Get concrete Dal implementation's Instance
            // Get property info for public static property named "Instance" (in the dal implementation class- taken above)
            // If the property is not found or it's not public or not static then it is not properly implemented
            // as a Singleton...
            // Get the value of the property Instance 
            try
            {
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                if (type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) is not IDal dal)
                    throw new DalConfigException($"Class {dalNameSpace}.{dalClass} instance is not initialized");
                // now it looks like we have appropriate dal implementation instance :-)
                return dal;
            }
            catch (NullReferenceException ex)
            {
                throw new DalConfigException($"Class {dalNameSpace}.{dalClass} is not a singleton", ex);
            }

        }
    }
}
