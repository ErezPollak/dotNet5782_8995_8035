using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    internal class DataSourceXML
    {
        static string stationsPath = @"xml\stations.xml";
        XElement stationRoot;

        public XElement BaseStations
        {
            get
            {

                return stationRoot;
            }
        }
    }
}
