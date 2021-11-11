using IBL;
using System;
using System.Collections.Generic;

namespace ConsoleUI_BL
{
    class Program
    { 
        private static Random r = new Random();     // a static value for 


        public static void Main(string[] args)
        {
            IBL.IBL bl = new BL();

            int choice = inputChoice();

            while (choice != 5)
            {
                //the main switch to choose the category of the needed action.
                switch (choice)
                {
                    //case 1: adding options
                    case 1:
                        {
                            Console.WriteLine("enter your choice: \n" +
                                               "for adding a base station to the list: 1. \n" +
                                               "for adding a drone to the list: 2. \n" +
                                               "for adding a customer to the list: 3.\n" +
                                               "for adding a perches to the list: 4.\n");

                            int addingChoice;
                            int.TryParse(Console.ReadLine(), out addingChoice);

                            switch (addingChoice)
                            {
                                case 1:
                                    {
                                        //case 1: adding a base station

                                        Console.Write("Enter the number of the station: ");
                                        int number;
                                        int.TryParse(Console.ReadLine(), out number);

                                        Console.Write("enter the name of the station: ");
                                        string name = Console.ReadLine();

                                        Console.Write("enter the longtude: ");
                                        double longtude;
                                        double.TryParse(Console.ReadLine(), out longtude);

                                        Console.Write("enter the lattitude: ");
                                        double lattitude;
                                        double.TryParse(Console.ReadLine(), out lattitude);

                                        Console.Write("enter the number of charge slots: ");
                                        int chargeslots;
                                        int.TryParse(Console.ReadLine(), out chargeslots);

                                        IBAL.BO.BaseStation baseStation = new IBAL.BO.BaseStation() { 
                                            Id = number, 
                                            Name = name, 
                                            CargingDrones = new List<IBAL.BO.Drone>(), 
                                            ChargeSlots = chargeslots, 
                                            Location = new IBAL.BO.Location()
                                            { 
                                                Longitude = longtude, 
                                                Lattitude = lattitude
                                            } 
                                        };

                                        bl.AddBaseStation(baseStation);

                                        Console.WriteLine();
                                    }
                                    break;
                                case 2:
                                    {

                                        //case 2: adding a drone

                                        Console.Write("Enter the serial number of the drone: ");
                                        int number;
                                        int.TryParse(Console.ReadLine(), out number);

                                        Console.Write("enter the model of the drone: ");
                                        string name = Console.ReadLine();

                                        Console.Write("enter the max weight of the drone: ");
                                        IBAL.BO.Enums.WeightCategories maxWeight;
                                        IBAL.BO.Enums.WeightCategories.TryParse(Console.ReadLine(), out maxWeight);

                                        Console.WriteLine("enter the number if the station to put the drone fir initial charge: ");
                                        int stationNumber;
                                        int.TryParse(Console.ReadLine(), out stationNumber);

                                        IBAL.BO.DroneForList drone = new IBAL.BO.DroneForList()
                                        {
                                            Id = number,
                                            Battary = r.Next() % 20 + 20,
                                            Model = name,
                                            Status = IBAL.BO.Enums.DroneStatuses.MAINTENANCE,
                                            ParcelId = -1,
                                            Weight = maxWeight,
                                            Location = bl.GetBaseStation(stationNumber).Location
                                        };

                                        bl.AddDrone(drone);// , status , battary);

                                        Console.WriteLine();

                                    }
                                    break;
                                case 3:
                                    {

                                        //case 3: adding a customer

                                        Console.Write("Enter the number of the customer: ");
                                        int number;
                                        int.TryParse(Console.ReadLine(), out number);

                                        Console.Write("enter the name of the customer: ");
                                        string name = Console.ReadLine();

                                        Console.Write("enter the phone: ");
                                        string phone = Console.ReadLine();

                                        Console.Write("enter the longtude: ");
                                        double longtude;
                                        double.TryParse(Console.ReadLine(), out longtude);

                                        Console.Write("enter the lattitude: ");
                                        double lattitude;
                                        double.TryParse(Console.ReadLine(), out lattitude);

                                        IBAL.BO.Customer customer = new IBAL.BO.Customer() 
                                        { Id = number, 
                                            Name = name, 
                                            Phone = phone,
                                            Location = new IBAL.BO.Location()
                                            { 
                                                Longitude = longtude, 
                                                Lattitude = lattitude 
                                            } 
                                        };

                                        bl.AddCustumer(customer);
                                        
                                        Console.WriteLine();
                                    }
                                    break;
                                case 4:
                                    {

                                        //case 4: adding a parcel

                                        //Console.Write("Enter the number of the parcel: ");
                                        //int number;
                                        //int.TryParse(Console.ReadLine(), out number);

                                        Console.WriteLine("enter the sender ID: ");
                                        int senderId;
                                        int.TryParse(Console.ReadLine(), out senderId);

                                        Console.WriteLine("enter the target ID: ");
                                        int targetId;
                                        int.TryParse(Console.ReadLine(), out targetId);

                                        Console.WriteLine("enter the weight of the parcel: ");
                                        IBAL.BO.Enums.WeightCategories weight;
                                        IBAL.BO.Enums.WeightCategories.TryParse(Console.ReadLine(), out weight);

                                        Console.WriteLine("enter the praiority of the parcel: ");
                                        IBAL.BO.Enums.Priorities priority;
                                        IBAL.BO.Enums.Priorities.TryParse(Console.ReadLine(), out priority);


                                        IBAL.BO.Parcel parcel = new IBAL.BO.Parcel()
                                        {
                                            Id = bl.GetNextSerialNumberForParcel(),
                                            Sender = bl.GetCustomerForParcel(senderId),
                                            Reciver = bl.GetCustomerForParcel(targetId),
                                            Priority = priority,
                                            Drone = null,
                                            Weight = weight,
                                            CreationTime = DateTime.Now,
                                            PickupTime = new DateTime(),
                                            AssigningTime = new DateTime(),
                                            DeliveringTime = new DateTime()
                                        };


                                        bl.AddParcel(parcel);

                                        Console.WriteLine();
                                    }
                                    break;
                            }

                        }
                        break;
                    //end of case 1 in the main switch


                    //case 2: updating options


                    case 2:
                        {
                            Console.WriteLine("\n" +
                                              "enter your choice: \n" +
                                              "for updating model of a drone: 1. \n" +
                                              "for updating ditails of a base station: 2. \n" +
                                              "for updating ditails of customer: 3.\n" +

                                              "for snding a drone to a base station: 4.\n" +
                                              "for releasing a drone from a base station: 5.\n");

                            int updatingChoice;
                            int.TryParse(Console.ReadLine(), out updatingChoice);

                            switch (updatingChoice)
                            {
                                case 1:
                                    {
                                        // case 1: assigning drone for a parcel
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);

                                        Console.WriteLine("enter the new model of the drone: ");
                                        string model = Console.ReadLine();

                                        bl.SetNameForADrone(droneId, model);

                                    }
                                    break;
                                case 2:
                                    {
                                        // case 2: updating the picking up time of the parcel to be now
                                        Console.WriteLine("enter the number of the base station: ");
                                        int basStationID;
                                        int.TryParse(Console.ReadLine(), out basStationID);

                                        Console.WriteLine("enter the new name of the base station: ");
                                        string name = Console.ReadLine();

                                        Console.WriteLine("enter the new number of the slots: ");
                                        int slots;
                                        int.TryParse(Console.ReadLine(), out slots);

                                        bl.UpdateBaseStation(basStationID, name, slots);
                                    }
                                    break;
                                case 3:
                                    {
                                        // case 3: updating the delivering time of the parcel to be now
                                        Console.WriteLine("enter the number of the customer: ");
                                        int customerID;
                                        int.TryParse(Console.ReadLine(), out customerID);

                                        Console.WriteLine("enter the new nameof the customer: ");
                                        string name = Console.ReadLine();

                                        Console.WriteLine("enter the new phone of the customer: ");
                                        string phone = Console.ReadLine();

                                        bl.UpdateCustomer(customerID, name, phone);

                                    }
                                    break;
                                case 4:
                                    {
                                        //case 4: updating the charge of a drone
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);


                                        Console.WriteLine("pick up the number of the baseStation out of the avalible ones: ");
                                        Console.Write("the avalible bases are: ");
                                        IEnumerable<IDAL.DO.BaseStation> freeBaseStations = dalObject.GetFreeStations();
                                        foreach (IDAL.DO.BaseStation baseStation in freeBaseStations)
                                        {
                                            Console.Write(baseStation.id + " ");
                                        }
                                        Console.WriteLine();
                                        int baseID;
                                        int.TryParse(Console.ReadLine(), out baseID);

                                        dalObject.ChargeDrone(baseID, droneID);
                                    }
                                    break;
                                case 5:
                                    {
                                        //relicing a drone from charging
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);

                                        dalObject.UnChargeDrone(droneID);
                                    }
                                    break;

                            }

                        }
                        break;
                    //end of case 2 in the main switch


                    //case 3: shoing a cetain object.
                    case 3:
                        {
                            Console.WriteLine("enter your choice: \n" +
                                              "to show a base station: 1. \n" +
                                              "to show a drone: 2. \n" +
                                              "to show a custumer: 3.\n" +
                                              "to show a parcel: 4.\n");

                            int showChoice;
                            int.TryParse(Console.ReadLine(), out showChoice);

                            switch (showChoice)
                            {
                                case 1:
                                    {
                                        //to show a base station
                                        Console.WriteLine("enter the number of the base station: ");
                                        int baseID;
                                        int.TryParse(Console.ReadLine(), out baseID);

                                        IDAL.DO.BaseStation baseStation = dalObject.GetBaseStation(baseID);
                                        Console.WriteLine(baseStation);

                                    }
                                    break;
                                case 2:
                                    {
                                        // to show a drone
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);

                                        IDAL.DO.Drone drone = dalObject.GetDrone(droneID);
                                        Console.WriteLine(drone);
                                    }
                                    break;
                                case 3:
                                    {
                                        //to show a customer
                                        Console.WriteLine("enter the number of the customer: ");
                                        int customerID;
                                        int.TryParse(Console.ReadLine(), out customerID);


                                        IDAL.DO.Customer customer = dalObject.GetCustomer(customerID);
                                        Console.WriteLine(customer);
                                    }
                                    break;
                                case 4:
                                    {
                                        //to show a parcel
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int parcelID;
                                        int.TryParse(Console.ReadLine(), out parcelID);

                                        IDAL.DO.Parcel parcel = dalObject.GetParcel(parcelID);
                                        Console.WriteLine(parcel);
                                    }
                                    break;
                            }

                        }
                        break;
                    //end of case 3 in the main switch


                    //case 4: showing lists:
                    case 4:
                        {
                            Console.WriteLine("enter your choice: \n" +
                                              "to show the list of the base stations: 1. \n" +
                                              "to show the list of the drones: 2. \n" +
                                              "to show the list of the custumers: 3.\n" +
                                              "to show the list of the parcels: 4.\n" +
                                              "to show the list of the parcels that dont have a drone: 5.\n" +
                                              "to show the list of the base stations with available chrging slots: 6");

                            int showChoice;
                            int.TryParse(Console.ReadLine(), out showChoice);

                            switch (showChoice)
                            {
                                case 1:
                                    {
                                        //showing the list of base stations
                                        IEnumerable<IDAL.DO.BaseStation> baseStations = dalObject.GetBaseStations();
                                        foreach (IDAL.DO.BaseStation baseStation in baseStations)
                                        {
                                            Console.WriteLine(baseStation);
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        //showing the list of the drones
                                        IEnumerable<IDAL.DO.Drone> drones = dalObject.GetDrones();
                                        foreach (IDAL.DO.Drone drone in drones)
                                        {
                                            Console.WriteLine(drone);
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        // showing the list of the customers
                                        IEnumerable<IDAL.DO.Customer> customers = dalObject.GetCustomers();
                                        foreach (IDAL.DO.Customer customer in customers)
                                        {
                                            Console.WriteLine(customer);
                                        }
                                    }
                                    break;
                                case 4:
                                    {
                                        //showing the list of the parcels
                                        IEnumerable<IDAL.DO.Parcel> parcels = dalObject.GetParcels();
                                        foreach (IDAL.DO.Parcel parcel in parcels)
                                        {
                                            Console.WriteLine(parcel);
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        //shoing the list of the parcels that dont have a drine.
                                        IEnumerable<IDAL.DO.Parcel> noDroneParcels = dalObject.GetParcelToDrone();
                                        foreach (IDAL.DO.Parcel parcel in noDroneParcels)
                                        {
                                            Console.WriteLine(parcel);
                                        }
                                    }
                                    break;
                                case 6:
                                    {
                                        //shoing the base stations that have free charging slots
                                        IEnumerable<IDAL.DO.BaseStation> freebaseStations = dalObject.GetFreeStations();
                                        foreach (IDAL.DO.BaseStation baseStation in freebaseStations)
                                        {
                                            Console.WriteLine(baseStation);
                                        }
                                    }
                                    break;
                            }

                        }
                        break;
                        //end of case 4.


                }//end of switch

                choice = inputChoice();

            }//end of while 


        }//end of Main

        /// <summary>
        /// the function prints the menu for the main switch.
        /// </summary>
        /// <returns></returns>
        private static int inputChoice()
        {
            Console.WriteLine("enter your choice: \n" +
                "for adding options: 1. \n" +
                "for updating options: 2. \n" +
                "for showing certine object options: 3.\n" +
                "for showing a list of objects: 4.\n" +
                "for exit: 5.\n");

            int choice;
            int.TryParse(Console.ReadLine(), out choice);

            return choice;
        }




        //IBAL.BO.DroneForList drone = new IBAL.BO.DroneForList()
        //{
        //    Id = 200,
        //    Battary = 100,
        //    Location = new IBAL.BO.Location()
        //    {
        //        Lattitude = 10,
        //        Longitude = 20
        //    },
        //    Model = "AA11111",
        //    ParcelId = 3,
        //    Weight = 0,
        //    Status = (IBAL.BO.Enums.DroneStatuses)1
        //};

        //bl.AddDrone(drone);

        //foreach(IBAL.BO.DroneForList d in bl.GetDrones())
        //{
        //    Console.WriteLine(d.Id);
        //}

    
    }
}
