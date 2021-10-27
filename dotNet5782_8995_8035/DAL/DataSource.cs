
//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program is an internal class for the namespace "DalObjects", that contains all the data structures for the project.
//
//the program contains the properties and an initilasetion function that supose to give the initial values to the structeres.


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
        public static IDAL.DO.Drone[] drones = new IDAL.DO.Drone[10];                      //contains up to 10 drones.
        public static IDAL.DO.BaseStation[] baseStations = new IDAL.DO.BaseStation[5];     // contains up to 5 base stations
        public static IDAL.DO.Customer[] customers = new IDAL.DO.Customer[100];            // contains up to 100 customers.
        public static IDAL.DO.Parcel[] parcheses = new IDAL.DO.Parcel[1000];               // contains up to 1000 parcels.

        public static Random r = new Random();     // a static value for 


        //the class that hols the updated status of the free space in the arraies.
        //the properties of the class are being changed in the functions that add add or deletes properties from the arreis.
        internal class Config
        {
            public static int freeDrone = 0;      //the first free place for drone.
            public static int freeBaseStation = 0;//the first free place for base station.
            public static int freeCustumer = 0;   //the first free place for a customer.
            public static int freePerches = 0;    //the first free place for percel.

            public static int serialNumberForPackeges = 0;
        }
        
       
        /// <summary>
        ///the fundction that initilaze the data bases with randomal values.
        /// </summary>
        public static void Initialize()
        {
            //randomal values for base stations.
            for (int i = 0; i < 2; i++)
            {
                baseStations[i] = new IDAL.DO.BaseStation() {
                    id = i,
                    name = i.ToString(),
                    lattitude = r.NextDouble() * 180 - 90,   // randomal values from -90 to 90 in order to represent a real coordinated location.
                    longitude = r.NextDouble() * 180 - 90,   // randomal values from -90 to 90 in order to represent a real coordinated location.
                    chargeSlots = r.Next() % 5 
                };
            }
            Config.freeBaseStation = 2;  //update for congif class.


            //randomal values for drones.
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
            Config.freeDrone = 5; //update for congif class.


            //randomal values for customers.
            for (int i = 0; i < 10; i++)
            {
                customers[i] = new IDAL.DO.Customer() { 
                    id = i,
                    name = (char)(r.Next() % 26 + 65) + " , " + (char)(r.Next() % 26 + 65), 
                    phone = "05" + (r.Next() % 10).ToString() + "-" + (r.Next() % 1000000).ToString(),
                    lattitude = r.NextDouble() * 180 - 90,   // randomal values from -90 to 90 in order to represent a real coordinated location.
                    longitude = r.NextDouble() * 180 - 90   // randomal values from -90 to 90 in order to represent a real coordinated location.
                };
            }
            Config.freeCustumer = 10;//update for congif class.

            //randomal values for parcels.
            for (int i = 0; i < 10; i++)
            {
                parcheses[i] = new IDAL.DO.Parcel()
                {
                    id = i,
                    senderId = customers[r.Next() % (Config.freeCustumer)].id, // random values from the avalible customers.
                    targetId = customers[r.Next() % (Config.freeCustumer)].id, // random values from the avalible customers.
                    Weight = (WeightCategories)(r.Next() % 3),
                    priority = (Priorities)(r.Next() % 3),
                    droneId = -1,
                    requested = pickingBiggerDate(DateTime.Now),  // initilesed to be the time of the initialization.
                    pickedUp = DateTime.Now,                        //initilesed for now, will change in  DalObject class, when order is updated to be picked up.
                    delivered = DateTime.Now
                };
            }

            //initilazing the "scaduald" date to be after the "reqested" date. by the function below.
            for (int i = 0; i < 10; i++)
            {
                parcheses[i].scheduled = pickingBiggerDate(parcheses[i].requested);
            }
            Config.freePerches = 10;
        }

        //the function recives a date, and randing another while making sure that the randomal date is after the given one.
        private static DateTime pickingBiggerDate(DateTime d)
        {
            DateTime newD;

            do
            {
                newD = new DateTime(r.Next() % 4 + 2020, r.Next() % 5 + 1, r.Next() % 5 + 1, r.Next() % 24, r.Next() % 60, r.Next() % 60);
            } while (newD < d);

            return newD;

        }
    }
}
