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

        public static void Initialize()
        {
            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {
                baseStations[i] = new IDAL.DO.BaseStation() {
                    id = i,
                    name = i,
                    lattitude = r.NextDouble() * 180 - 90,
                    longitude = r.NextDouble() * 180 - 90,
                    chargeSlots = r.Next() % 5 
                };
            }
            Config.freeBaseStation = 2;

            for (int i = 0; i < 5; i++)
            {
                drones[i] = new IDAL.DO.Drone() { 
                    id = i, 
                    model = (char)(r.Next()%26 + 65) +""+ (char)(r.Next() % 26 + 65) + (r.Next()%100000).ToString(), 
                    MaxWeight = (WeightCategories)(r.Next() % 3),
                    Status = (DroneStatuses)(r.Next() % 3), 
                    battery = r.Next() % 50 + 50
                };
            }
            Config.freeDrone = 5;
 
            for (int i = 0; i < 10; i++)
            {
                customers[i] = new IDAL.DO.Customer() { 
                    id = i,
                    name = (char)(r.Next() % 26 + 65) + " , " + (char)(r.Next() % 26 + 65), 
                    phone = "05" + (r.Next() % 10).ToString() + "-" + (r.Next() % 1000000).ToString(),
                    lattitude = r.NextDouble() * 180 - 90, 
                    longitude = r.NextDouble() * 180 - 90
                };
            }
            Config.freeCustumer = 10;

            for (int i = 0; i < 10; i++)
            {
                parcheses[i] = new IDAL.DO.Parcel()
                {
                    id = i,
                    senderId = customers[r.Next() % (Config.freeCustumer)].id,
                    targetId = customers[r.Next() % (Config.freeCustumer)].id,
                    Weight = (WeightCategories)(r.Next() % 3),
                    priority = (Priorities)(r.Next() % 3),
                    requested = pickingBiggerDate(DateTime.Now),
                    pickedUp = DateTime.Now,
                    delivered = DateTime.Now
                };
            }

            for (int i = 0; i < 10; i++)
            {
                parcheses[i].scheduled = pickingBiggerDate(parcheses[i].requested);
            }
            Config.freePerches = 10;

            //Add a loop for pickig a drone accoring to the weight.


        }

        public static DateTime pickingBiggerDate(DateTime d)
        {
            Random r = new Random();

            DateTime newD;

            do
            {
                newD = new DateTime(r.Next() % 4 + 2020, r.Next() % 5 + 1, r.Next() % 5 + 1, r.Next() % 24, r.Next() % 60, r.Next() % 60);
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
    }
}
