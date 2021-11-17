using IBL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI_BL
{
    class Program
    { 
        private static Random r = new Random();     // a static value for 

        public static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the ConsoleUI_BL program.\n");

            IBL.IBL bl;

            try
            {
              bl  = new BL();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            
            int choice = inputChoice();

            while (choice != 5)
            {
                //the main switch to choose the category of the needed action.
                switch (choice)
                {
                    //case 1: adding options
                    case 1:
                        {
                            Console.WriteLine("Adding Options: \n\n Enter your choice: \n\n" +
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

                                        Console.WriteLine("1 : adding a base station: \n");

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

                                        try
                                        {
                                            if (bl.AddBaseStation(baseStation))
                                            {
                                                Console.WriteLine($"\nbasestation {name} successfully added to the list.");
                                            }
                                        }catch(Exception e)
                                        {
                                            Console.WriteLine("\nException: ");
                                            printException(e);
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



                                        try
                                        {
                                            if (bl.AddDrone(drone))
                                            {
                                                Console.WriteLine($"\nthe drone {name} successfully added to the list.");
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("\n Exception: ");
                                            printException(e);
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
                                        { 
                                            Id = number, 
                                            Name = name, 
                                            Phone = phone,
                                            Location = new IBAL.BO.Location()
                                            { 
                                                Longitude = longtude, 
                                                Lattitude = lattitude 
                                            } 
                                        };

                                        try
                                        {
                                            if (bl.AddCustumer(customer))
                                            {
                                                Console.WriteLine($"\ncustomer {name} successfully added to the list.");
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("\n Exception: ");
                                            printException(e);
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
                                            RequestedTime = DateTime.Now,
                                            PickupTime = new DateTime(),
                                            AcceptedTime = new DateTime(),
                                            DeliveringTime = new DateTime()
                                        };


                                        try
                                        {
                                            if (bl.AddParcel(parcel))
                                            {
                                                Console.WriteLine($"\nparcel successfully added to the list.");
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("\n Exception: ");
                                            printException(e);
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
                                              "updationg option:\n\n" + "enter your choice: \n\n" +
                                              "for updating model of a drone: 1. \n" +
                                              "for updating ditails of a base station: 2. \n" +
                                              "for updating ditails of customer: 3.\n" +
                                              "for snding a drone to a base station: 4.\n" +
                                              "for releasing a drone from a base station: 5.\n" +
                                              "to assign a parcel to a drone: 6.\n" +
                                              "to deliver a parcel from a drone: 7.\n");

                            int updatingChoice;
                            int.TryParse(Console.ReadLine(), out updatingChoice);
                            Console.WriteLine();

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

                                        try
                                        {
                                            if (bl.UpdateNameForADrone(droneId, model))
                                            {
                                                Console.WriteLine($"\nthe name of the drone {droneId} updated to {model} successfully.");
                                            }
                                        }catch(Exception e)
                                        {
                                            printException(e);
                                        }
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

                                        try
                                        {
                                            if (bl.UpdateBaseStation(basStationID, name, slots))
                                            {
                                                Console.WriteLine($"\ndetails of the basStation {basStationID} updated successfully.");
                                            }
                                            
                                        }
                                        catch (Exception e)
                                        {
                                            printException(e);
                                        }
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

                                        try
                                        {
                                            if (bl.UpdateCustomer(customerID, name, phone))
                                            {
                                                Console.WriteLine($"\nthe name of the customer {customerID} update successfully.");
                                            }
                                         
                                        }
                                        catch (Exception e)
                                        {
                                            printException(e);
                                        }

                                    }
                                    break;
                                case 4:
                                    {
                                        //case 4: updating the charge of a drone
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);

                                        try
                                        {
                                            if (bl.ChargeDrone(droneId))
                                            {
                                                Console.WriteLine($"\nthe drone {droneId} send to charging successfully.");
                                            }
                                          
                                        }
                                        catch (Exception e)
                                        {
                                            printException(e);
                                        }

                                    }
                                    break;
                                case 5:
                                    {
                                        //relicing a drone from charging
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);

                                        Console.WriteLine("enter number of minutes that  the drone was charged: ");
                                        int minutes;
                                        int.TryParse(Console.ReadLine(), out minutes);

                                        try
                                        {
                                            if  (bl.UnChargeDrone(droneId, minutes))
                                            {
                                                Console.WriteLine($"\nthe drone {droneId} released from charging successfully.");
                                            }
                                           
                                        }
                                        catch (Exception e)
                                        {
                                            printException(e);
                                        }

                                       
                                    }
                                    break;
                                case 6:
                                    {
                                        //case 6: assigning a parcel to a drone.
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);

                                        try
                                        {
                                            if (bl.AssignParcelToADrone(droneId))
                                            {
                                                Console.WriteLine($"\nthe drone {droneId} assign to the persel {bl.GetDrone(droneId).ParcelId} successfully.");
                                            }
                                           
                                        }
                                        catch (Exception e)
                                        {
                                            printException(e);
                                        }

                                        
                                    }
                                    break;
                                case 7:
                                    {
                                        //case 7: delivering a parcel from a drone.
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);

                                        try
                                        {
                                            if (bl.DeliveringParcelFromADrone(droneID))
                                            {
                                                Console.WriteLine($"\nthe parcel {bl.GetDrone(droneID).ParcelId} delivered successfully.");
                                            }
                                            
                                        }
                                        catch (Exception e)
                                        {
                                            printException(e);
                                        }

                                        
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
                                              "to show a parcel: 4.\n\n");

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

                                        Console.WriteLine(bl.GetBaseStation(baseID));

                                    }
                                    break;
                                case 2:
                                    {
                                        // to show a drone
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);

                                        Console.WriteLine(bl.GetDrone(droneID));
                                    }
                                    break;
                                case 3:
                                    {
                                        //to show a customer
                                        Console.WriteLine("enter the number of the customer: ");
                                        int customerID;
                                        int.TryParse(Console.ReadLine(), out customerID);

                                        Console.WriteLine(bl.GetCustomer(customerID));
                                    }
                                    break;
                                case 4:
                                    {
                                        //to show a parcel
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int parcelID;
                                        int.TryParse(Console.ReadLine(), out parcelID);

                                        Console.WriteLine(bl.GetParcel(parcelID));
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
                                              "to show the list of the base stations with available chrging slots: 6\n\n");

                            int showChoice;
                            int.TryParse(Console.ReadLine(), out showChoice);

                            switch (showChoice)
                            {
                                case 1:
                                    {
                                        //showing the list of base stations
                                        bl.GetBaseStations(b => true).ToList().ForEach(b => Console.WriteLine(b));
                                        Console.WriteLine();
                                    }
                                    break;
                                case 2:
                                    {
                                        //showing the list of the drones
                                         bl.GetDrones(d => true).ToList().ForEach(d => Console.WriteLine(d));
                                        Console.WriteLine();
                                    }
                                    break;
                                case 3:
                                    {
                                        // showing the list of the customers
                                        bl.GetCustomers(c => true).ToList().ForEach(c => Console.WriteLine(c));
                                        Console.WriteLine();
                                    }
                                    break;
                                case 4:
                                    {
                                        //showing the list of the parcels
                                        bl.GetPacels(p => true).ToList().ForEach(p => Console.WriteLine(p));
                                        Console.WriteLine();
                                    }
                                    break;
                                case 5:
                                    {
                                        //shoing the list of the parcels that dont have a drine.
                                        bl.GetPacels(p => bl.GetParcel(p.Id).Drone.Id == -1).ToList().ForEach(p => Console.WriteLine(p));
                                        Console.WriteLine();
                                    }
                                    break;
                                case 6:
                                    {
                                        //shoing the base stations that have free charging slots
                                        bl.GetBaseStations(b => b.ChargeSlots >= 0).ToList().ForEach(c => Console.WriteLine(c));
                                        Console.WriteLine();
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
            Console.WriteLine("\nEnter your choice: \n\n" +
                "for adding options: 1. \n" +
                "for updating options: 2. \n" +
                "for showing certine object options: 3.\n" +
                "for showing a list of objects: 4.\n" +
                "for exit: 5.\n");

            int choice;
            int.TryParse(Console.ReadLine(), out choice);

            return choice;
        }


        private static void printException(Exception e)
        {
            if (e == null) return;

            printException(e.InnerException);

            Console.WriteLine(e.Message);
        }

    
    }
}
