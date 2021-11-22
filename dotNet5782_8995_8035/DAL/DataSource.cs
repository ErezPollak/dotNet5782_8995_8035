
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
        internal static List<IDAL.DO.Drone> drones = new List<IDAL.DO.Drone>();
        internal static List<IDAL.DO.BaseStation> baseStations = new List<IDAL.DO.BaseStation>();
        internal static List<IDAL.DO.Customer> customers = new List<IDAL.DO.Customer>();
        internal static List<IDAL.DO.Parcel> parcels = new List<IDAL.DO.Parcel>();

        //an data structure to contain all the charging of the drones.
        internal static List<IDAL.DO.DroneCharge> droneCharges = new List<IDAL.DO.DroneCharge>();


        private static Random r = new Random();     // a static value for 

        /// <summary>
        /// the function contains the information about the electricity use 
        /// </summary>
        internal class Config
        {
            public static double Free; // for the electricity use of a free drone
            public static double Light; // for the electricity use of a drone that carrys a light wight.
            public static double Middel; // for the electricity use of a drone that carrys a middle wight.
            public static double Heavy; // for the electricity use of a drone that carrys a heavy wight.

            public static double ChargingSpeed;//for the speed of the charge. precentage for hour.

            public static int serialNumber = 1000;
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
            Config.Free = r.NextDouble() * 100 + 50;
            Config.Light = Config.Free - r.NextDouble() * 30;
            Config.Middel = Config.Light - r.NextDouble() * 30;
            Config.Heavy = Config.Middel - r.NextDouble() * 30;
            Config.ChargingSpeed = r.NextDouble() * 100 + 50;

            //randomal values for base stations.
            for (int i = 0; i < 2; i++)
            {
                IDAL.DO.BaseStation baseStation = new IDAL.DO.BaseStation()
                {
                    Id = i,
                    Name = i.ToString(),
                    Location = new Location
                    {
                        Lattitude = r.NextDouble() * 360 - 180,   // randomal values from -180 to 180 in order to represent a real coordinated location.
                        Longitude = r.NextDouble() * 180 - 90,   // randomal values from -90 to 90 in order to represent a real coordinated location.
                    },

                    ChargeSlots = r.Next() % 5 + 2
                };
                //adding the base station to the list.
                baseStations.Add(baseStation);
            }

            //randomal values for drones.
            for (int i = 0; i < 5; i++)
            {
                IDAL.DO.Drone drone = new IDAL.DO.Drone()
                {
                    Id = i,
                    Model = (char)(r.Next() % 26 + 65) + "" + (char)(r.Next() % 26 + 65) + (r.Next() % 100000).ToString(),
                    MaxWeight = (WeightCategories)(r.Next() % 3),
                    
                };
                drones.Add(drone);
            }


            //randomal values for customers.
            for (int i = 0; i < 10; i++)
            {
                IDAL.DO.Customer customer = new IDAL.DO.Customer()
                {
                    Id = i,
                    Name = (char)(r.Next() % 26 + 65) + " , " + (char)(r.Next() % 26 + 65),
                    Phone = "05" + (r.Next() % 10).ToString() + "-" + (r.Next() % 1000000).ToString(),

                    Location = new Location
                    {
                        Lattitude = r.NextDouble() * 360 - 180,   // randomal values from -180 to 180 in order to represent a real coordinated location.
                        Longitude = r.NextDouble() * 180 - 90,   // randomal values from -90 to 90 in order to represent a real coordinated location.
                    },
                };

                customers.Add(customer);

            }

            //randomal values for parcels.
            for (int i = 0; i < 10; i++)
            {
                IDAL.DO.Parcel parcel = new IDAL.DO.Parcel()
                {
                    Id = i,
                    SenderId = customers[r.Next() % (customers.Count)].Id, // random values from the avalible customers.
                    TargetId = customers[r.Next() % (customers.Count)].Id, // random values from the avalible customers.
                    Weight = (WeightCategories)(r.Next() % 3),
                    Priority = (Priorities)(r.Next() % 3),
                    DroneId = 0,                                   //initileized to not have any drone, the drone number will be updated in the dalobject class.
                    RequestedTime = pickingBiggerDate(DateTime.Now),  // initilesed to be the time of the initialization.
                    PickedUpTime = null,                        //initilesed for now, will change in  DalObject class, when order is updated to be picked up.
                    AcceptedTime = null
                };

                parcels.Add(parcel);

            }

            //initilazing the "scaduald" date to be after the "reqested" date. by the function below.
            for (int index = 0; index < DataSource.parcels.Count; index++)
            {
                IDAL.DO.Parcel p = DataSource.parcels[index];
                p.DeliveryTime = pickingBiggerDate(p.RequestedTime);
                DataSource.parcels[index] = p;
                ++index;
            }
        }

        //the function recives a date, and randing another while making sure that the randomal date is after the given one.
        private static DateTime pickingBiggerDate(DateTime? d)
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
