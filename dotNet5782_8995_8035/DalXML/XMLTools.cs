using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Dal
{
    internal static class XmlTools
    {
        private const string Dir = @"..\xml\";

        static XmlTools()
        {
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
        }


        #region SaveLoadWithXMLSerializer
        public static void SaveListToXmlSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                var file = new FileStream(Dir + filePath, FileMode.Create);//dir + 
                var x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception)
            {
                //throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        public static List<T> LoadListFromXmlSerializer<T>(string filePath)
        {
            if (!File.Exists(Dir + filePath)) return new List<T>();
            var x = new XmlSerializer(typeof(List<T>));
            var file = new FileStream(Dir + filePath, FileMode.Open);//dir + 
            var list = (List<T>)x.Deserialize(file);
            file.Close();
            return list;

        }
        #endregion
    }
}
