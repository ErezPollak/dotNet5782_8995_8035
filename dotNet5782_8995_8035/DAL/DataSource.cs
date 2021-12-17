
//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program is an internal class for the namespace "DalObjects", that contains all the data structures for the project.
//
//the program contains the properties and an initilasetion function that supose to give the initial values to the structeres.


using DO;
using System;
using System.Collections.Generic;


namespace Dal
{

    internal class DataSource
    {
        internal static readonly List<Drone> drones = new();
        internal static readonly List<BaseStation> baseStations = new();
        internal static readonly List<Customer> customers = new();
        internal static readonly List<Parcel> parcels = new();

        //an data structure to contain all the charging of the drones.
        internal static List<DroneCharge> droneCharges = new();


        private static readonly Random rnd = new(); 

        /// <summary>
        /// the function contains the information about the electricity use 
        /// </summary>
        internal class Config
        {
            internal static double Free; // for the electricity use of a free drone
            internal static double Light; // for the electricity use of a drone that carrys a light wight.
            internal static double Middel; // for the electricity use of a drone that carrys a middle wight.
            internal static double Heavy; // for the electricity use of a drone that carrys a heavy wight.

            internal static double ChargingSpeed;//for the speed of the charge. precentage for hour.

            internal static int SerialNumber = 1000;
        }

        /// <summary>
        ///the fundction that initilaze the data bases with randomal values.
        ///the initialize will add to the lists the The following objects:
        ///2 base stations.
        ///5 drones.
        ///10 customers.
        ///10 parcels.
        ///the objects will be initlised with randomal values.
        /// </summary>
        internal static void Initialize()
        {

            //initilize values of the config function.
            Config.Free = rnd.NextDouble() * 100 + 50;
            Config.Light = Config.Free - rnd.NextDouble() * 30;
            Config.Middel = Config.Light - rnd.NextDouble() * 30;
            Config.Heavy = Config.Middel - rnd.NextDouble() * 30;
            Config.ChargingSpeed = rnd.NextDouble() * 10;

            //randomal values for base stations.
            for (int i = 0; i < 2; i++)
            {
                BaseStation baseStation = new BaseStation()
                {
                    Id = i * 12 + 12345,
                    Name = i.ToString(),
                    Location = new Location
                    {
                        Latitude = RandLatitude(),
                        Longitude = RandLongtude()            
                    },
                    ChargeSlots = rnd.Next() % 10 + 2
                };
                //adding the base station to the list.
                baseStations.Add(baseStation);
            }

            //randomal values for drones.
            for (int i = 0; i < 20; i++)
            {
                Drone drone = new Drone()
                {
                    Id = i * 34 + 254254,
                    Model = (char)(rnd.Next() % 26 + 65) + "" + (char)(rnd.Next() % 26 + 65) + rnd.Next() % 100000,
                    MaxWeight = (WeightCategories)(rnd.Next() % 3),
                    
                };
                drones.Add(drone);
            }


            //randomal values for customers.
            for (int i = 0; i < 10; i++)
            {
                Customer customer = new Customer()
                {
                    Id = i * 22 + 234242,
                    Name = GetRAndomName(),
                    Phone = "05" + rnd.Next() % 10 + "-" + rnd.Next() % 1000000,

                    Location = new Location
                    {
                        Latitude = RandLatitude(),
                        Longitude = RandLongtude()
                    },
                };

                customers.Add(customer);

            }

            int[] dronesForParcels = DroneForParcel();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(dronesForParcels[i]);
            }

            

            //randomal values for parcels.
            for (int i = 0; i < 10; i++)
            {
                Parcel parcel = new Parcel()
                {
                    Id = i * 32 + 232345,
                    SenderId = customers[rnd.Next() % customers.Count].Id, // random values from the avalible customers.
                    TargetId = customers[rnd.Next() % customers.Count].Id, // random values from the avalible customers.
                    Weight = (WeightCategories)(rnd.Next() % 3),
                    Priority = (Priorities)(rnd.Next() % 3),
                    DroneId = dronesForParcels[i],                                   //initileized to not have any drone, the drone number will be updated in the dalobject class.
                    RequestedTime = PickingBiggerDate(DateTime.Now),  // initilesed to be the time of the initialization.
                    PickedUpTime = null,                        //initilesed for now, will change in  Dal class, when order is updated to be picked up.
                    AcceptedTime = null
                };

                parcels.Add(parcel);

            }

            //initilazing the "scaduald" date to be after the "reqested" date. by the function below.
            for (int index = 0; index < parcels.Count; index++)
            {
                Parcel p = parcels[index];
                p.DeliveryTime = PickingBiggerDate(p.RequestedTime);
                parcels[index] = p;
                ++index;
            }
        }

        private static string GetRAndomName()
        {
         
            string[] firstNames = new string[] { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian" , "abby", "abigail", "adele", "adrian" };
            string[] lastNames = new string[] { "abbott", "acosta", "adams", "adkins", "aguilar" };


            return firstNames[rnd.Next(firstNames.Length)] + " " + lastNames[rnd.Next(lastNames.Length)];

        }

        //the function recives a date, and randing another while making sure that the randomal date is after the given one.
        private static DateTime PickingBiggerDate(DateTime? d)
        {
            DateTime newD;

            do
            {
                newD = new DateTime(rnd.Next() % 4 + 2020, rnd.Next() % 5 + 1, rnd.Next() % 5 + 1, rnd.Next() % 24, rnd.Next() % 60, rnd.Next() % 60);
            } while (newD < d);

            return newD;

        }

        /// <summary>
        /// initilze the number of parceles of every drone.
        /// </summary>
        /// <returns></returns>
        private static int[] DroneForParcel()
        {
            int[] nums = new int[10];
            for (int i = 0; i < 10; i++)
                nums[i] = -1;

            for (int i = 0; i < 3; i++)
                nums[rnd.Next() % 10] = drones[i].Id;

            return nums;
        }



        private static double RandLongtude()
        {
            return rnd.NextDouble()*(35.254321 - 35.153024) + 35.153024;
        }   

        private static double RandLatitude()
        {
            return rnd.NextDouble() * (31.878338 - 31.745826) + 31.745826;
        }

    }

    


}



//31.878338, 35.153024 LU                                  31.806531, 35.254321 RU










//31.745826, 35.140715 LD




