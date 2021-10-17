using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DalObject
{
    internal class DataSource
    {
        public static IDAL.DO.Drone[] drones = new IDAL.DO.Drone[10];
        public static IDAL.DO.BaseStation[] baseStations = new IDAL.DO.BaseStation[5];
        public static IDAL.DO.Customer[] customers = new IDAL.DO.Customer[100];
        public static IDAL.DO.Parcel[] parcheses = new IDAL.DO.Parcel[1000];

        internal class Config
        {
            public static int freeDrone = 0;
            public static int freeBaseStation = 0;
            public static int freeCustumer = 0;
            public static int freePerches = 0;

            public static int serialNumberForPackeges = 0;
        }

        public void Initialize()
        {
            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {
                baseStations[i] = new IDAL.DO.BaseStation(i, i, r.Next()%1000 - 500 , r.Next() % 1000 -500 , r.Next() % 5);
            }
            
        }

    }
}
