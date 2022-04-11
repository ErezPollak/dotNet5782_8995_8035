//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the students: Erez Polak 322768995

//the function that show to the user the options to do and activate the relevant function.


using System;
using System.Linq;
using DalApi;
using DalFacade.Models;
using DO;

namespace ConsoleUI
{
    internal static class Program
    {

        public static void Main(string[] args)
        {

            var dalObject = DalFactory.GetDal();

            var choice = InputChoice();

            while (choice != 5)
            {
                //the main switch to choose the category of the needed action.
                switch (choice)
                {
                    //case 1: adding options
                    case 1:
                        {
                            Console.WriteLine("enter your choice: \n" +
                                               "for adding a base station to the list: .  1. \n" +
                                               "for adding a drone to the list:  . . . .  2. \n" +
                                               "for adding a customer to the list: . . .  3.\n" +
                                               "for adding a perches to the list:  . . .  4.\n");

                            int.TryParse(Console.ReadLine(), out var addingChoice);

                            switch (addingChoice)
                            {
                                case 1:
                                    {
                                        //case 1: adding a base station

                                        Console.Write("Enter the number of the station: ");
                                        int.TryParse(Console.ReadLine(), out var id);

                                        Console.Write("enter the name of the station: ");
                                        var name = Console.ReadLine();

                                        Console.Write("enter the longitude: ");
                                        double.TryParse(Console.ReadLine(), out var longitude);

                                        Console.Write("enter the latitude: ");
                                        double.TryParse(Console.ReadLine(), out var latitude);

                                        Console.Write("enter the number of charge slots: ");
                                        int.TryParse(Console.ReadLine(), out var chargeSlots);

                                        var baseStation = new BaseStation()
                                        {
                                            Id = id,
                                            Name = name,
                                            Location = new Location() { Longitude = longitude, Latitude = latitude },
                                            ChargeSlots = chargeSlots
                                        };
                                        
                                        dalObject.AddBaseStation(baseStation);

                                        Console.WriteLine();
                                    }
                                    break;
                                case 2:
                                    {

                                        //case 2: adding a drone

                                        Console.Write("Enter the number of the drone: ");
                                        int.TryParse(Console.ReadLine(), out var id);

                                        Console.Write("enter the model of the drone: ");
                                        var model = Console.ReadLine();

                                        Console.Write("enter the max weight of the drone: ");
                                        Enum.TryParse(Console.ReadLine(), out WeightCategories maxWeight);

                                        var dalDrone = new Drone()
                                        {
                                            Id = id,
                                            Model = model,
                                            MaxWeight = maxWeight
                                        };

                                        try
                                        {
                                            dalObject.AddDrone(dalDrone);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }

                                    }
                                    break;
                                case 3:
                                    {

                                        //case 3: adding a customer

                                        Console.Write("Enter the number of the customer: ");
                                        int.TryParse(Console.ReadLine(), out var id);

                                        Console.Write("enter the name of the customer: ");
                                        var name = Console.ReadLine();

                                        Console.Write("enter the phone: ");
                                        var phone = Console.ReadLine();

                                        Console.Write("enter the longitude: ");
                                        double.TryParse(Console.ReadLine(), out var longitude);

                                        Console.Write("enter the latitude: ");
                                        double.TryParse(Console.ReadLine(), out var latitude);

                                        var dalCustomer = new Customer()
                                        {
                                            Id = id,
                                            Name = name,
                                            Phone = phone,
                                            Location = new Location() { Longitude = longitude, Latitude = latitude }
                                        };


                                        try
                                        {
                                            dalObject.AddCustomer(dalCustomer);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                        
                                    }
                                    break;
                                case 4:
                                    {

                                        //case 4: adding a parcel

                                        Console.Write("Enter the number of the parcel: ");
                                        int.TryParse(Console.ReadLine(), out var id);

                                        Console.WriteLine("enter the sender ID: ");
                                        int.TryParse(Console.ReadLine(), out var senderId);

                                        Console.WriteLine("enter the target ID: ");
                                        int.TryParse(Console.ReadLine(), out var targetId);

                                        Console.WriteLine("enter the weight of the parcel: ");
                                        Enum.TryParse(Console.ReadLine(), out WeightCategories weight);

                                        Console.WriteLine("enter the priority of the parcel: ");
                                        Enum.TryParse(Console.ReadLine(), out Priorities priority);

                                        //setting the requested time to be now.
                                        var requested = DateTime.Now;

                                        Console.WriteLine("How many days from now you want to schedule your parcel?");
                                        int.TryParse(Console.ReadLine(), out var numOfDaysForDelivery);
                                        var scheduled = requested.AddDays(numOfDaysForDelivery);

                                        //dalObject.AddParcel(number, senderId, targetId, weight, priority, droneId, requested, scheduled);
                                        var dalParcel = new Parcel()
                                        {
                                            Id = id,
                                            SenderId = senderId,
                                            TargetId = targetId,
                                            Weight = weight,
                                            Priority = priority,
                                            DroneId = 0,
                                            DefinedTime = requested,
                                            DeliveryTime = scheduled,
                                            AssignedTime = null,
                                            PickedUpTime = null
                                        };

                                        try
                                        {
                                            dalObject.AddParcel(dalParcel);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                        
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
                                              "for updating the drone id for a parcel: . .  1. \n" +
                                              "for picking up a parcel by a drone: . . . .  2. \n" +
                                              "for delivering a parcel:  . . . . . . . . .  3.\n" +
                                              "for sending a drone to a base station: . . .  4.\n" +
                                              "for releasing a drone from a base station:.  5.\n");

                            int.TryParse(Console.ReadLine(), out var updatingChoice);

                            switch (updatingChoice)
                            {
                                case 1:
                                    {
                                        // case 1: assigning drone for a parcel
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int.TryParse(Console.ReadLine(), out var parcelId);

                                        Console.WriteLine("enter the number of the drone: ");
                                        int.TryParse(Console.ReadLine(), out var droneId);
                                        
                                        try
                                        {
                                            dalObject.AssignDroneToParcel(parcelId, droneId);                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }

                                    }
                                    break;
                                case 2:
                                    {
                                        // case 2: updating the picking up time of the parcel to be now
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int.TryParse(Console.ReadLine(), out var parcelId);
                                        
                                        Console.WriteLine("enter the number of the drone: ");
                                        int.TryParse(Console.ReadLine(), out var droneId);

                                        try
                                        {
                                            dalObject.PickingUpParcel(parcelId , droneId);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        // case 3: updating the delivering time of the parcel to be now
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int.TryParse(Console.ReadLine(), out var parcelId);

                                        try
                                        {
                                            dalObject.DeliveringParcel(parcelId);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }

                                    }
                                    break;
                                case 4:
                                    {
                                        //case 4: updating the charge of a drone
                                        Console.WriteLine("enter the number of the drone: ");
                                        int.TryParse(Console.ReadLine(), out var droneId);


                                        Console.WriteLine("pick up the number of the baseStation out of the available ones: ");
                                        Console.Write("the available bases are: ");
                                        dalObject.GetBaseStations(b => b.ChargeSlots > 0).ToList().ForEach(b => Console.WriteLine(b.Id));
                                        Console.WriteLine();
                                        int.TryParse(Console.ReadLine(), out var baseId);
                                        
                                        try
                                        {
                                            dalObject.ChargeDrone(baseId, droneId);                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        //realising a drone from charging
                                        Console.WriteLine("enter the number of the drone: ");
                                        int.TryParse(Console.ReadLine(), out var droneId);
                                        
                                        try
                                        {
                                            dalObject.UnChargeDrone(droneId);                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                        
                                    }
                                    break;

                            }

                        }
                        break;
                    //end of case 2 in the main switch


                    //case 3: showing a certain object.
                    case 3:
                        {
                            Console.WriteLine("enter your choice: \n" +
                                              "to show a base station:  . 1. \n" +
                                              "to show a drone: . . . . . 2. \n" +
                                              "to show a customer:  . . . 3.\n" +
                                              "to show a parcel:  . . . . 4.\n");

                            int.TryParse(Console.ReadLine(), out var showChoice);

                            switch (showChoice)
                            {
                                case 1:
                                    {
                                        //to show a base station
                                        Console.WriteLine("enter the number of the base station: ");
                                        int.TryParse(Console.ReadLine(), out var baseId);

                                        var baseStation = dalObject.GetBaseStation(baseId);
                                        Console.WriteLine(baseStation);

                                    }
                                    break;
                                case 2:
                                    {
                                        // to show a drone
                                        Console.WriteLine("enter the number of the drone: ");
                                        int.TryParse(Console.ReadLine(), out var droneId);

                                        var drone = dalObject.GetDrone(droneId);
                                        Console.WriteLine(drone);
                                    }
                                    break;
                                case 3:
                                    {
                                        //to show a customer
                                        Console.WriteLine("enter the number of the customer: ");
                                        int.TryParse(Console.ReadLine(), out var customerId);


                                        var customer = dalObject.GetCustomer(customerId);
                                        Console.WriteLine(customer);
                                    }
                                    break;
                                case 4:
                                    {
                                        //to show a parcel
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int.TryParse(Console.ReadLine(), out var parcelId);

                                        var parcel = dalObject.GetParcel(parcelId);
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
                                              "to show the list of the base stations:  . . . . . . . . . . . . . . . 1. \n" +
                                              "to show the list of the drones: . . . . . . . . . . . . . . . . . . . 2. \n" +
                                              "to show the list of the customers:  . . . . . . . . . . . . . . . . . 3.\n" +
                                              "to show the list of the parcels:  . . . . . . . . . . . . . . . . . . 4.\n" +
                                              "to show the list of the parcels that dont have a drone: . . . . . . . 5.\n" +
                                              "to show the list of the base stations with available charging slots:. 6");

                            int.TryParse(Console.ReadLine(), out var showChoice);

                            switch (showChoice)
                            {
                                case 1:
                                    {
                                        //showing the list of base stations
                                        dalObject.GetBaseStations(_ => true).ToList().ForEach(b => Console.WriteLine(b));
                                    }
                                    break;
                                case 2:
                                    {
                                        //showing the list of the drones
                                        dalObject.GetDrones(_ => true).ToList().ForEach(d => Console.WriteLine(d));
                                    }
                                    break;
                                case 3:
                                    {
                                        // showing the list of the customers
                                        dalObject.GetCustomers(_ => true).ToList().ForEach(d => Console.WriteLine(d));
                                    }
                                    break;
                                case 4:
                                    {
                                        //showing the list of the parcels
                                        dalObject.GetParcels(_ => true).ToList().ForEach(p => Console.WriteLine(p));
                                    }
                                    break;
                                case 5:
                                    {
                                        //showing the list of the parcels that dont have a drone.
                                        dalObject.GetParcels(p => p.DroneId == 0).ToList().ForEach(p => Console.WriteLine(p));
                                    }
                                    break;
                                case 6:
                                    {
                                        //showing the base stations that have free charging slots
                                        dalObject.GetBaseStations(bs => bs.ChargeSlots > 0).ToList().ForEach(bs => Console.WriteLine(bs));
                                    }
                                    break;
                            }

                        }
                        break;
                        //end of case 4.


                }//end of switch

                choice = InputChoice();

            }//end of while 


        }//end of Main

        /// <summary>
        /// the function prints the menu for the main switch.
        /// </summary>
        /// <returns></returns>
        private static int InputChoice()
        {
            Console.WriteLine("enter your choice: \n" +
                "for adding options: . . . . . . . . . .  1. \n" +
                "for updating options: . . . . . . . . .  2. \n" +
                "for showing certain object options: . .  3.\n" +
                "for showing a list of objects:  . . . .  4.\n" +
                "for exit: 5.\n");

            int.TryParse(Console.ReadLine(), out var choice);

            return choice;
        }

    }//end of class
}
