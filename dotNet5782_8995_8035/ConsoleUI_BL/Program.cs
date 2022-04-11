using System;
using System.Collections.Generic;
using System.Linq;
using BL;
using BL.Abstracts;
using BL.Exceptions;
using BL.Models;
using static System.Int32;

namespace ConsoleUI_BL
{
    internal static class Program
    {

        private static readonly Random R = new Random(); // a static value for 

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the ConsoleUI_BL program.\n");

            var bl = BlFactory.GetBl();

            var choice = InputChoice();

            while (choice != 5)
            {
                //the main switch to choose the category of the needed action.
                switch (choice)
                {
                    //case 1: adding options
                    case 1:
                    {
                        Console.WriteLine("Adding Options: \n Enter your choice: \n\n" +
                                          "for adding a base station to the list: . . .  1. \n" +
                                          "for adding a drone to the list:  . . . . . .  2. \n" +
                                          "for adding a customer to the list: . . . . .  3.\n" +
                                          "for adding a perches to the list:  . . . . .  4.\n");

                        TryParse(Console.ReadLine(), out var addingChoice);

                        switch (addingChoice)
                        {
                            case 1:
                            {
                                //case 1: adding a base station

                                Console.WriteLine("1 : adding a base station: \n");

                                Console.Write("Enter the number of the station: ");
                                TryParse(Console.ReadLine(), out var number);

                                Console.Write("enter the name of the station: ");
                                var name = Console.ReadLine();

                                Console.Write("enter the longitude: ");
                                double.TryParse(Console.ReadLine(), out var longitude);

                                Console.Write("enter the latitude: ");
                                double.TryParse(Console.ReadLine(), out var latitude);

                                Console.Write("enter the number of charge slots: ");
                                TryParse(Console.ReadLine(), out var chargeSlots);

                                try
                                {
                                    if (longitude is > 90 or < -90)
                                    {
                                        throw new WrongInoutException("longitude has to be between -90 to 90");
                                    }

                                    if (latitude is > 180 or < -180)
                                    {
                                        throw new WrongInoutException("latitude has to be between -180 to 180");
                                    }

                                    if (chargeSlots <= 0)
                                    {
                                        throw new WrongInoutException(
                                            "number of charging slots has to be bigger then zero.");
                                    }

                                    var baseStation = new BaseStation()
                                    {
                                        Id = number,
                                        Name = name,
                                        ChargingDrones = new List<DroneInCharge>(),
                                        ChargeSlots = chargeSlots,
                                        Location = new Location()
                                        {
                                            Longitude = longitude,
                                            Latitude = latitude
                                        }
                                    };

                                    if (bl.AddBaseStation(baseStation))
                                    {
                                        Console.WriteLine($"\nbaseStation {name} successfully added to the list.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\nException: ");
                                    PrintException(e);
                                }
                                finally
                                {
                                    Console.WriteLine();
                                }
                            }
                                break;
                            case 2:
                            {
                                //case 2: adding a drone

                                Console.Write("Enter the serial number of the drone: ");
                                TryParse(Console.ReadLine(), out var number);

                                Console.Write("enter the model of the drone: ");
                                var name = Console.ReadLine();

                                Console.Write("enter the max weight of the drone: ");
                                Enum.TryParse(Console.ReadLine(), out Enums.WeightCategories maxWeight);

                                Console.WriteLine(
                                    "enter the number if the station to put the drone for initial charge: ");
                                TryParse(Console.ReadLine(), out var stationNumber);

                                try
                                {
                                    var drone = new DroneForList()
                                    {
                                        Id = number,
                                        Battery = R.Next() % 20 + 20,
                                        Model = name,
                                        Status = Enums.DroneStatuses.FREE,
                                        ParcelId = -1,
                                        Weight = maxWeight,
                                        Location = bl.GetBaseStation(stationNumber).Location
                                    };


                                    if (bl.AddDrone(new Drone()
                                    {
                                        Id = drone.Id,
                                        Battery = drone.Battery,
                                        Location = drone.Location,
                                        MaxWeight = drone.Weight,
                                        Model = drone.Model,
                                        Status = drone.Status
                                    }))
                                    {
                                        Console.WriteLine($"\nthe drone {name} successfully added to the list.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\nException: ");
                                    PrintException(e);
                                }
                                finally
                                {
                                    Console.WriteLine();
                                }
                            }
                                break;
                            case 3:
                            {
                                //case 3: adding a customer

                                Console.Write("Enter the number of the customer: ");
                                TryParse(Console.ReadLine(), out var number);

                                Console.Write("enter the name of the customer: ");
                                var name = Console.ReadLine();

                                Console.Write("enter the phone: ");
                                var phone = Console.ReadLine();

                                Console.Write("enter the longitude: ");
                                double.TryParse(Console.ReadLine(), out var longitude);

                                Console.Write("enter the latitude: ");
                                double.TryParse(Console.ReadLine(), out var latitude);

                                try
                                {
                                    if (longitude is > 90 or < -90)
                                    {
                                        throw new WrongInoutException("longitude has to be between -90 to 90");
                                    }

                                    if (latitude is > 180 or < -180)
                                    {
                                        throw new WrongInoutException("latitude has to be between -180 to 180");
                                    }

                                    var customer = new Customer()
                                    {
                                        Id = number,
                                        Name = name,
                                        Phone = phone,
                                        Location = new Location()
                                        {
                                            Longitude = longitude,
                                            Latitude = latitude
                                        }
                                    };


                                    if (bl.AddCustomer(customer))
                                    {
                                        Console.WriteLine($"\ncustomer {name} successfully added to the list.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\n Exception: ");
                                    PrintException(e);
                                }
                                finally
                                {
                                    Console.WriteLine();
                                }
                            }
                                break;
                            case 4:
                            {
                                //case 4: adding a parcel

                                Console.WriteLine("enter the sender ID: ");
                                TryParse(Console.ReadLine(), out var senderId);

                                Console.WriteLine("enter the target ID: ");
                                TryParse(Console.ReadLine(), out var targetId);

                                Console.WriteLine("enter the weight of the parcel: ");
                                Enum.TryParse(Console.ReadLine(), out Enums.WeightCategories weight);

                                Console.WriteLine("enter the priority of the parcel: ");
                                Enum.TryParse(Console.ReadLine(), out Enums.Priorities priority);


                                try
                                {
                                    var parcel = new Parcel()
                                    {
                                        Id = bl.GetNextSerialNumberForParcel(),
                                        Sender = new CoustomerForParcel()
                                        {
                                            Id = bl.GetCustomer(senderId).Id,
                                            CustomerName = bl.GetCustomer(senderId).Name
                                        },
                                        Receiver = new CoustomerForParcel()
                                        {
                                            Id = bl.GetCustomer(targetId).Id,
                                            CustomerName = bl.GetCustomer(targetId).Name
                                        },
                                        Priority = priority,
                                        Drone = null,
                                        Weight = weight,
                                        DefinedTime = DateTime.Now,
                                        PickupTime = new DateTime(),
                                        AssignedTime = new DateTime(),
                                        DeliveringTime = DateTime.Now.AddDays(10)
                                    };


                                    if (bl.AddParcel(parcel))
                                    {
                                        Console.WriteLine($"\nparcel successfully added to the list.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\n Exception: ");
                                    PrintException(e);
                                }
                                finally
                                {
                                    Console.WriteLine();
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
                                          "updating option:\n\n" + "enter your choice: \n\n" +
                                          "for updating model of a drone: . . . . . . . . . .  1. \n" +
                                          "for updating details of a base station:  . . . . .  2. \n" +
                                          "for updating details of customer:  . . . . . . . .  3.\n" +
                                          "for sending a drone to a base station:  . . . . . .  4.\n" +
                                          "for releasing a drone from a base station: . . . .  5.\n" +
                                          "to assign a parcel to a drone: . . . . . . . . . .  6.\n" +
                                          "to deliver a parcel from a drone:  . . . . . . . .  7.\n");

                        TryParse(Console.ReadLine(), out var updatingChoice);
                        Console.WriteLine();

                        switch (updatingChoice)
                        {
                            case 1:
                            {
                                // case 1: assigning drone for a parcel
                                Console.WriteLine("enter the number of the drone: ");
                                TryParse(Console.ReadLine(), out var droneId);

                                Console.WriteLine("enter the new model of the drone: ");
                                var model = Console.ReadLine();

                                try
                                {
                                    if (bl.UpdateNameForADrone(droneId, model))
                                    {
                                        Console.WriteLine(
                                            $"\nthe name of the drone {droneId} updated to {model} successfully.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 2:
                            {
                                // case 2: updating the picking up time of the parcel to be now
                                Console.WriteLine("enter the number of the base station: ");
                                TryParse(Console.ReadLine(), out var basStationId);

                                Console.WriteLine("enter the new name of the base station: ");
                                var name = Console.ReadLine();

                                Console.WriteLine("enter the new number of the slots: ");
                                TryParse(Console.ReadLine(), out var slots);

                                try
                                {
                                    if (bl.UpdateBaseStation(basStationId, name, slots))
                                    {
                                        Console.WriteLine(
                                            $"\ndetails of the basStation {basStationId} updated successfully.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 3:
                            {
                                // case 3: updating the delivering time of the parcel to be now

                                Console.WriteLine("enter the number of the customer: ");
                                TryParse(Console.ReadLine(), out var customerId);

                                Console.WriteLine("enter the new nameof the customer: ");
                                var name = Console.ReadLine();

                                Console.WriteLine("enter the new phone of the customer: ");
                                var phone = Console.ReadLine();

                                try
                                {
                                    if (bl.UpdateCustomer(customerId, name, phone))
                                    {
                                        Console.WriteLine(
                                            $"\nthe name of the customer {customerId} update successfully.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 4:
                            {
                                //case 4: updating the charge of a drone
                                Console.WriteLine("enter the number of the drone: ");
                                TryParse(Console.ReadLine(), out var droneId);

                                try
                                {
                                    if (bl.ChargeDrone(droneId))
                                    {
                                        Console.WriteLine($"\nthe drone {droneId} send to charging successfully.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 5:
                            {
                                //reliving a drone from charging
                                Console.WriteLine("enter the number of the drone: ");
                                TryParse(Console.ReadLine(), out var droneId);

                                try
                                {
                                    if (bl.UnChargeDrone(droneId))
                                    {
                                        Console.WriteLine(
                                            $"\nthe drone {droneId} released from charging successfully.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 6:
                            {
                                //case 6: assigning a parcel to a drone.
                                Console.WriteLine("enter the number of the drone: ");
                                TryParse(Console.ReadLine(), out var droneId);

                                try
                                {
                                    if (bl.AssignParcelToADrone(droneId))
                                    {
                                        Console.WriteLine(
                                            $"\nthe drone {droneId} assign to the parcel {bl.GetDrone(droneId).ParcelInDelivery.Id} successfully.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 7:
                            {
                                //case 7: delivering a parcel from a drone.
                                Console.WriteLine("enter the number of the drone: ");
                                TryParse(Console.ReadLine(), out var droneId);

                                try
                                {
                                    if (bl.DeliveringParcelFromADrone(droneId))
                                    {
                                        Console.WriteLine(
                                            $"\nthe parcel {bl.GetDrone(droneId).ParcelInDelivery.Id} delivered successfully.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
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
                                          "to show a base station: . . 1. \n" +
                                          "to show a drone:  . . . . . 2. \n" +
                                          "to show a customer: . . . . 3.\n" +
                                          "to show a parcel: . . . . . 4.\n\n");

                        TryParse(Console.ReadLine(), out var showChoice);

                        switch (showChoice)
                        {
                            case 1:
                            {
                                //to show a base station
                                Console.WriteLine("enter the number of the base station: ");
                                TryParse(Console.ReadLine(), out var baseId);

                                try
                                {
                                    Console.WriteLine(bl.GetBaseStation(baseId));
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 2:
                            {
                                // to show a drone
                                Console.WriteLine("enter the number of the drone: ");
                                TryParse(Console.ReadLine(), out var droneId);

                                try
                                {
                                    Console.WriteLine(bl.GetDrone(droneId));
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 3:
                            {
                                //to show a customer
                                Console.WriteLine("enter the number of the customer: ");
                                TryParse(Console.ReadLine(), out var customerId);

                                try
                                {
                                    Console.WriteLine(bl.GetCustomer(customerId));
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
                            }
                                break;
                            case 4:
                            {
                                //to show a parcel
                                Console.WriteLine("enter the number of the parcel: ");
                                TryParse(Console.ReadLine(), out var parcelId);

                                try
                                {
                                    Console.WriteLine(bl.GetParcel(parcelId));
                                }
                                catch (Exception e)
                                {
                                    PrintException(e);
                                }
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
                                          "to show the list of the base stations: . . . . . . . . . . . . . . . . .  1. \n" +
                                          "to show the list of the drones:  . . . . . . . . . . . . . . . . . . . .  2. \n" +
                                          "to show the list of the customers: . . . . . . . . . . . . . . . . . . .  3.\n" +
                                          "to show the list of the parcels: . . . . . . . . . . . . . . . . . . . .  4.\n" +
                                          "to show the list of the parcels that dont have a drone:  . . . . . . . .  5.\n" +
                                          "to show the list of the base stations with available charging slots:  . .  6\n\n");

                        TryParse(Console.ReadLine(), out var showChoice);

                        switch (showChoice)
                        {
                            case 1:
                            {
                                //showing the list of base stations
                                bl.GetBaseStations().ToList().ForEach(Console.WriteLine);
                                Console.WriteLine();
                            }
                                break;
                            case 2:
                            {
                                //showing the list of the drones
                                bl.GetDrones().ToList().ForEach(Console.WriteLine);
                                Console.WriteLine();
                            }
                                break;
                            case 3:
                            {
                                // showing the list of the customers
                                bl.GetCustomers().ToList().ForEach(Console.WriteLine);
                                Console.WriteLine();
                            }
                                break;
                            case 4:
                            {
                                //showing the list of the parcels
                                bl.GetParcels().ToList().ForEach(Console.WriteLine);
                                Console.WriteLine();
                            }
                                break;
                            case 5:
                            {
                                //showing the list of the parcels that dont have a drone.
                                bl.GetParcelsForSelector().ToList()
                                    .ForEach(Console.WriteLine);
                                Console.WriteLine();
                            }
                                break;
                            case 6:
                            {
                                //showing the base stations that have free charging slots
                                bl.GetBaseStationsForSelector().ToList().ForEach(Console.WriteLine);
                                Console.WriteLine();
                            }
                                break;
                        }
                    }
                        break;
                    //end of case 4.
                } //end of switch

                choice = InputChoice();
            } //end of while 
        } //end of Main

        /// <summary>
        /// the function prints the menu for the main switch.
        /// </summary>
        /// <returns></returns>
        private static int InputChoice()
        {
            Console.WriteLine("\nEnter your choice: \n\n" +
                              "for adding options: . . . . . . . . . . . .  1. \n" +
                              "for updating options: . . . . . . . . . . .  2. \n" +
                              "for showing created object options: . . . .  3.\n" +
                              "for showing a list of objects:  . . . . . .  4.\n" +
                              "for exit: 5.\n");

            TryParse(Console.ReadLine(), out var choice);

            return choice;
        }


        private static void PrintException(Exception e)
        {
            if (e == null) return;

            PrintException(e.InnerException);

            Console.WriteLine(e.Message + " " + e.Source);
        }
    }
}