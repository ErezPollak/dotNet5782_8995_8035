using IDAL.DO;
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
                baseStations[i] = new IDAL.DO.BaseStation() { id = 1, name = 1, lattitude = r.Next() % 1000 - 500, longitude = r.Next() % 1000 - 500, chargeSlots = r.Next() % 5 };
                Config.freeBaseStation = 2;
            }
            for (int i = 0; i < 5; i++)
            {
                drones[i] = new IDAL.DO.Drone() { id = i, model = "11111", MaxWeight = (WeightCategories)(r.Next() % 3), Status = (DroneStatuses)(r.Next() % 3), battery = r.Next() % 50 + 50 };
            }
            for (int i = 0; i < 10; i++)
            {
                customers[i] = new IDAL.DO.Customer() { id = i, name = "22222", phone = "23432", lattitude = r.Next() % 1000 - 500, longitude = r.Next() % 1000 - 500 };
            }

            for (int i = 0; i < 10; i++)
            {
                parcheses[i] = new IDAL.DO.Parcel()
                {
                    id = i,
                    senderId = customers[r.Next() % (Config.freeCustumer)].id,
                    targetId = customers[r.Next() % (Config.freeCustumer)].id,
                    droneld = pickingDronefordelivery(),
                    Weight = (WeightCategories)(r.Next() % 3),
                    priority = (Priorities)(r.Next() % 3),
                    requested = pickingBiggerDate(new DateTime()),
                    scheduled = pickingBiggerDate(parcheses[i].requested),
                    delivered = pickingBiggerDate(parcheses[i].scheduled),
                    pickedUp = pickingBiggerDate(parcheses[i].delivered)
                };
            }

        }
        
        public static DateTime pickingBiggerDate(DateTime d)
        {
            Random r = new Random();

            DateTime newD;

            do
            {
                newD = new DateTime(r.Next() % 3 + 2021, r.Next() % 12 + 1, r.Next() % 30 + 1, r.Next() % 12, r.Next() % 60, r.Next() % 60);
            } while (newD < d);
            

            return newD;

        }

        public static int pickingDronefordelivery()
        {
            Random r = new Random();

            int drone;

            do {
                drone = drones[r.Next() % (Config.freeDrone)].id;
            } while (drones[drone].Status == DroneStatuses.FIXING) ;

            return drone;
        }

        //string rndStr(int len)
        //{
        //    Random r = new Random();

        //    string s;

        //    for (int i = 0; i < len; i++)
        //    {
        //        if (r.NextDouble() > 0.5)
        //        {
        //        }
        //        else
        //        {

        //        }
        //    }
        //}

    }
}
