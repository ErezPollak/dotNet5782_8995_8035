
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


        private static readonly Random Random = new(); 

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
            Config.Free = Random.NextDouble() * 100 + 50;
            Config.Light = Config.Free - Random.NextDouble() * 30;
            Config.Middel = Config.Light - Random.NextDouble() * 30;
            Config.Heavy = Config.Middel - Random.NextDouble() * 30;
            Config.ChargingSpeed = Random.NextDouble() * 10;

            //randomal values for base stations.
            for (int i = 0; i < 2; i++)
            {
                BaseStation baseStation = new BaseStation()
                {
                    Id = i,
                    Name = i.ToString(),
                    Location = new Location
                    {
                        Latitude = Random.NextDouble() * 360 - 180,   // randomal values from -180 to 180 in order to represent a real coordinated location.
                        Longitude = Random.NextDouble() * 180 - 90,   // randomal values from -90 to 90 in order to represent a real coordinated location.
                    },

                    ChargeSlots = Random.Next() % 5 + 2
                };
                //adding the base station to the list.
                baseStations.Add(baseStation);
            }

            //randomal values for drones.
            for (int i = 0; i < 5; i++)
            {
                Drone drone = new Drone()
                {
                    Id = i,
                    Model = (char)(Random.Next() % 26 + 65) + "" + (char)(Random.Next() % 26 + 65) + Random.Next() % 100000,
                    MaxWeight = (WeightCategories)(Random.Next() % 3),
                    
                };
                drones.Add(drone);
            }


            //randomal values for customers.
            for (int i = 0; i < 10; i++)
            {
                Customer customer = new Customer()
                {
                    Id = i,
                    Name = (char)(Random.Next() % 26 + 65) + " , " + (char)(Random.Next() % 26 + 65),
                    Phone = "05" + Random.Next() % 10 + "-" + Random.Next() % 1000000,

                    Location = new Location
                    {
                        Latitude = Random.NextDouble() * 360 - 180,   // randomal values from -180 to 180 in order to represent a real coordinated location.
                        Longitude = Random.NextDouble() * 180 - 90,   // randomal values from -90 to 90 in order to represent a real coordinated location.
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
                    Id = i,
                    SenderId = customers[Random.Next() % customers.Count].Id, // random values from the avalible customers.
                    TargetId = customers[Random.Next() % customers.Count].Id, // random values from the avalible customers.
                    Weight = (WeightCategories)(Random.Next() % 3),
                    Priority = (Priorities)(Random.Next() % 3),
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

        //the function recives a date, and randing another while making sure that the randomal date is after the given one.
        private static DateTime PickingBiggerDate(DateTime? d)
        {
            DateTime newD;

            do
            {
                newD = new DateTime(Random.Next() % 4 + 2020, Random.Next() % 5 + 1, Random.Next() % 5 + 1, Random.Next() % 24, Random.Next() % 60, Random.Next() % 60);
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
                nums[Random.Next() % 10] = i;

            return nums;
        }

    }
}
