
//course: Mini Project in Windows Systems
//lecture: Eliezer Grintsborger
//from the students: Erez Polak 322768995



//the program is an internal class for the namespace "DalObjects", that contains all the data structures for the project.
//
//the program contains the properties and an initialization function that suppose to give the initial values to the structures.


using DO;
using System;
using System.Collections.Generic;
using DalFacade.Models;


namespace Dal
{

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class DataSource
    {
        internal static readonly List<Drone> Drones = new();
        internal static readonly List<BaseStation> BaseStations = new();
        internal static readonly List<Customer> Customers = new();
        internal static readonly List<Parcel> Parcels = new();

        //an data structure to contain all the charging of the drones.
        internal static readonly List<DroneCharge> DroneCharges = new();


        private static readonly Random Rnd = new(); 

        /// <summary>
        /// the function contains the information about the electricity use 
        /// </summary>
        // ReSharper disable once ClassNeverInstantiated.Global
        internal class Config
        {
            internal static double Free; // for the electricity use of a free drone
            internal static double Light; // for the electricity use of a drone that carry a light wight.
            internal static double Middle; // for the electricity use of a drone that carry a middle wight.
            internal static double Heavy; // for the electricity use of a drone that carry a heavy wight.

            internal static double ChargingSpeed;//for the speed of the charge. percentage for hour.

            internal static int SerialNumber = 1000;
        }


        /// <summary>
        ///the function that initialize the data bases with random values.
        ///the initialize will add to the lists the The following objects:
        ///2 base stations.
        ///5 drones.
        ///10 customers.
        ///10 parcels.
        ///the objects will be initialised with random values.
        /// </summary>
        internal static void Initialize()
        {

            //initialize values of the config function.
            Config.Free = Rnd.NextDouble() * 1 + 2; 
            Config.Light = Config.Free - Rnd.NextDouble() * 0.5;
            Config.Middle = Config.Light - Rnd.NextDouble() * 0.5;
            Config.Heavy = Config.Middle - Rnd.NextDouble() * 0.5;
            Config.ChargingSpeed = Rnd.NextDouble() * 10;

            //random values for base stations.
            for (var i = 0; i < 5; i++)
            {
                var baseStation = new BaseStation()
                {
                    Id = i * 12 + 12345,
                    Name = i.ToString(),
                    Location = new Location
                    {
                        Latitude = RandLatitude(),
                        Longitude = RandLongitudes()
                    },
                    ChargeSlots = (i / 2) + 10
                };
                //adding the base station to the list.
                BaseStations.Add(baseStation);
            }

            //random values for drones.
            for (var i = 0; i < 20; i++)
            {
                var drone = new Drone()
                {
                    Id = i * 34 + 254254,
                    Model = (char)(Rnd.Next() % 26 + 65) + "" + (char)(Rnd.Next() % 26 + 65) + Rnd.Next() % 100000,
                    MaxWeight = (WeightCategories)(Rnd.Next() % 3),
                    
                };
                Drones.Add(drone);
            }


            //random values for customers.
            for (var i = 0; i < 10; i++)
            {
                var customer = new Customer()
                {
                    Id = i * 22 + 234242,
                    Name = GetRandomName(),
                    Phone = "05" + Rnd.Next() % 10 + "-" + Rnd.Next() % 10000000,

                    Location = new Location
                    {
                        Latitude = RandLatitude(),
                        Longitude = RandLongitudes()
                    },
                };

                Customers.Add(customer);

            }

            var dronesForParcels = DroneForParcel();

            //random values for parcels.
            for (var i = 0; i < 10; i++)
            {
                var parcel = new Parcel()
                {
                    Id = i * 32 + 232345,
                    SenderId = Customers[Rnd.Next() % Customers.Count].Id, // random values from the available customers.
                    TargetId = Customers[Rnd.Next() % Customers.Count].Id, // random values from the available customers.
                    Weight = (WeightCategories)(Rnd.Next() % 3),
                    Priority = (Priorities)(Rnd.Next() % 3),
                    DroneId = dronesForParcels[i],                                   //initialized to not have any drone, the drone number will be updated in the dalObject class.
                    DefinedTime = DateTime.Now,  // initialised to be the time of the initialization.
                    AssignedTime = dronesForParcels[i] != 0 ? DateTime.Now : null,
                    PickedUpTime = null,                        //initialised for now, will change in  Dal class, when order is updated to be picked up.
                    DeliveryTime = null
                };

                Parcels.Add(parcel);

            }

        }

        private static string GetRandomName()
        {
         
            var firstNames = new [] { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian" , "abby", "abigail", "adele", "adrian" };
            var lastNames = new [] { "abbott", "acosta", "adams", "adkins", "aguilar" };


            return firstNames[Rnd.Next(firstNames.Length)] + " " + lastNames[Rnd.Next(lastNames.Length)];

        }


        /// <summary>
        /// initialize the number of parcels of every drone.
        /// </summary>
        /// <returns></returns>
        private static int[] DroneForParcel()
        {
            var nums = new int[10];
            for (var i = 0; i < 10; i++)
                nums[i] = 0;

            for (var i = 1; i < 4; i++)
                nums[Rnd.Next() % 10] = Drones[i].Id;

            return nums;
        }



        private static double RandLongitudes()
        {
            return Rnd.NextDouble()*(35.254321 - 35.153024) + 35.153024;
        }   

        private static double RandLatitude()
        {
            return Rnd.NextDouble() * (31.878338 - 31.745826) + 31.745826;
        }

    }

    


}


