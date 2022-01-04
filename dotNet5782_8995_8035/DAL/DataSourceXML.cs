using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    internal class DataSourceXML
    {
        internal static readonly List<Drone> drones = new();
        internal static readonly List<BaseStation> baseStations = new();
        internal static readonly List<Customer> customers = new();
        internal static readonly List<Parcel> parcels = new();


        private static string ConfigPath = @"..\xml\Config.xml";
        private static string BaseStationsPath = @"BaseStations.xml";
        private static string CustomersPath = @"Customers.xml";
        private static string DronesPath = @"Drones.xml";
        private static string ParcelsPath = @"Parcels.xml";
        private static string DroneChargesPath = @"..\xml\DroneCharges.xml";




        //an data structure to contain all the charging of the drones.
        //internal static List<DroneCharge> droneCharges = new();


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

            XElement droneChargesRoot = XElement.Load(DroneChargesPath);
            droneChargesRoot.RemoveAll();
            droneChargesRoot.Save(DroneChargesPath);

            //randomal values for base stations.
            for (int i = 0; i < 5; i++)
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
                    ChargeSlots = (int)(i / 2) + 10
                };
                //adding the base station to the list.
                baseStations.Add(baseStation);
                
            }

            XMLTools.SaveListToXMLSerializer<BaseStation>(baseStations , BaseStationsPath);
            
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

                //DroneListRoot.Add(new XElement("drone",
                //    new XElement("Id", drone.Id),
                //    new XElement("Model", drone.Model),
                //    new XElement("MaxWeight", drone.MaxWeight)
                //    ));
                //DroneListRoot.Save(DronePath);
            }

            XMLTools.SaveListToXMLSerializer<Drone>(drones, DronesPath);


            //randomal values for customers.
            for (int i = 0; i < 10; i++)
            {
                Customer customer = new Customer()
                {
                    Id = i * 22 + 234242,
                    Name = GetRAndomName(),
                    Phone = "05" + rnd.Next() % 10 + "-" + rnd.Next() % 10000000,

                    Location = new Location
                    {
                        Latitude = RandLatitude(),
                        Longitude = RandLongtude()
                    },
                };
                customers.Add(customer);

                //CustomerListRoot.Add(new XElement("Customer",
                //    new XElement("Id", customer.Id),
                //    new XElement("Name", customer.Name),
                //    new XElement("Location",
                //        new XElement("Latitude", customer.Location.Latitude),
                //        new XElement("Longitude", customer.Location.Longitude)
                //    )
                //    ));
                //CustomerListRoot.Save(CustomerPath); 
            }

            XMLTools.SaveListToXMLSerializer<Customer>(customers, CustomersPath);

            int[] dronesForParcels = DroneForParcel();

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
                    DefinededTime = DateTime.Now,  // initilesed to be the time of the initialization.
                    AssigndedTime = dronesForParcels[i] != 0 ? DateTime.Now : null,
                    PickedUpTime = null,                        //initilesed for now, will change in  Dal class, when order is updated to be picked up.
                    DeliveryTime = null
                };
                parcels.Add(parcel);

                //ParcelListRoot.Add(new XElement("Parcel",
                //    new XElement("Id", parcel.Id),
                //    new XElement("SenderId", parcel.SenderId),
                //    new XElement("TargetId", parcel.TargetId),
                //    new XElement("Weight", parcel.Weight),
                //    new XElement("DroneId", parcel.DroneId),
                //    new XElement("DefinededTime", parcel.DefinededTime),
                //    new XElement("AssigndedTime", parcel.AssigndedTime),
                //    new XElement("PickedUpTime", parcel.PickedUpTime),
                //    new XElement("DeliveryTime", parcel.DeliveryTime))
                //);
                //ParcelListRoot.Save(ParcelPath);

            }

            XMLTools.SaveListToXMLSerializer<Parcel>(parcels, ParcelsPath);




        }

        private static string GetRAndomName()
        {

            string[] firstNames = new string[] { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian", "abby", "abigail", "adele", "adrian" };
            string[] lastNames = new string[] { "abbott", "acosta", "adams", "adkins", "aguilar" };


            return firstNames[rnd.Next(firstNames.Length)] + " " + lastNames[rnd.Next(lastNames.Length)];

        }


        /// <summary>
        /// initilze the number of parceles of every drone.
        /// </summary>
        /// <returns></returns>
        private static int[] DroneForParcel()
        {
            int[] nums = new int[10];
            for (int i = 0; i < 10; i++)
                nums[i] = 0;

            for (int i = 1; i < 4; i++)
                nums[rnd.Next() % 10] = drones[i].Id;

            return nums;
        }



        private static double RandLongtude()
        {
            return rnd.NextDouble() * (35.254321 - 35.153024) + 35.153024;
        }

        private static double RandLatitude()
        {
            return rnd.NextDouble() * (31.878338 - 31.745826) + 31.745826;
        }

    }



}

