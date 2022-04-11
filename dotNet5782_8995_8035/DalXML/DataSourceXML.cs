using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DalFacade.Models;
using DO;

namespace Dal
{
    /// <summary>
    /// initializing all the files of the xml, activated only when needed.
    /// </summary>
    public static class DataSourceXml
    {
        private static readonly List<Drone> Drones = new();
        private static readonly List<BaseStation> BaseStations = new();
        private static readonly List<Customer> Customers = new();
        private static readonly List<Parcel> Parcels = new();

        private const string BaseStationsPath = @"BaseStations.xml";
        private const string CustomersPath = @"Customers.xml";
        private const string DronesPath = @"Drones.xml";
        private const string ParcelsPath = @"Parcels.xml";
        private const string DroneChargesPath = @"..\xml\DroneCharges.xml";


        private static readonly Random Rnd = new();

        /// <summary>
        ///the function that initialize the data bases with random values.
        ///the initialize will add to the lists the The following objects:
        ///2 base stations.
        ///5 drones.
        ///10 customers.
        ///10 parcels.
        ///the objects will be initialized with random values.
        /// </summary>
        internal static void Initialize()
        {

            //resting all the charges database to point zero.

            var droneChargesRoot = XElement.Load(DroneChargesPath);
            droneChargesRoot.RemoveAll();
            droneChargesRoot.Save(DroneChargesPath);


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
                        Longitude = RandLongitude()
                    },
                    ChargeSlots = (i / 2) + 10
                };
                //adding the base station to the list.
                BaseStations.Add(baseStation);
                
            }
            XmlTools.SaveListToXmlSerializer(BaseStations , BaseStationsPath);
            
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

            XmlTools.SaveListToXmlSerializer(Drones, DronesPath);


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
                        Longitude = RandLongitude()
                    },
                };
                Customers.Add(customer);
            }
            XmlTools.SaveListToXmlSerializer(Customers, CustomersPath);

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
                    DefinedTime = DateTime.Now,  // initialized to be the time of the initialization.
                    AssignedTime = dronesForParcels[i] != 0 ? DateTime.Now : null,
                    PickedUpTime = null,                        //initialized for now, will change in  Dal class, when order is updated to be picked up.
                    DeliveryTime = null
                };
                Parcels.Add(parcel);
            }
            XmlTools.SaveListToXmlSerializer(Parcels, ParcelsPath);

        }

        private static string GetRandomName()
        {

            var firstNames = new [] { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian", "abby", "abigail", "adele", "adrian" };
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

        private static double RandLongitude()
        {
            return Rnd.NextDouble() * (35.254321 - 35.153024) + 35.153024;
        }

        private static double RandLatitude()
        {
            return Rnd.NextDouble() * (31.878338 - 31.745826) + 31.745826;
        }

    }



}

