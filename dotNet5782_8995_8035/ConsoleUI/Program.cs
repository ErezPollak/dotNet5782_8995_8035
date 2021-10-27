﻿//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035

//the function that show to the user the options to do and activate the relevent function.


using System;
using System.Collections.Generic;
using DalObject;

namespace ConsoleUI
{
    class Program
    {
        public static int inputChoice()
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

        public static void Main(string[] args)
        { 

            DalObject.DalObject dalObject = new DalObject.DalObject();

            int choice = inputChoice();

            while (choice != 5)
            {
                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("enter your choice: \n" +
                                               "for adding a base station to the list: 1. \n" +
                                               "for adding a drone to the list: 2. \n" +
                                               "for adding a customer to the list: 3.\n" +
                                               "for adding a perches to the list: 4.\n");

                            int addingChoice;
                            int.TryParse(Console.ReadLine(), out addingChoice);

                            switch(addingChoice){
                                case 1: 
                                    {
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


                                        dalObject.AddBaseStation(name, longtude, lattitude, chargeslots);

                                        Console.WriteLine();
                                    }
                                    break;
                                case 2:
                                    {
                                        Console.Write("enter the model of the drone: ");
                                        string name = Console.ReadLine();
                                        
                                        Console.Write("enter the max weight of the drone: ");
                                        IDAL.DO.WeightCategories maxWeight;
                                        IDAL.DO.WeightCategories.TryParse(Console.ReadLine(), out maxWeight);
                                        
                                        Console.Write("enter the status of the drone: ");
                                        IDAL.DO.DroneStatuses status;
                                        IDAL.DO.DroneStatuses.TryParse(Console.ReadLine(), out status);
                                       
                                        Console.Write("enter the battary status of the drone: ");
                                        double battary;
                                        double.TryParse(Console.ReadLine(), out battary);


                                        dalObject.AddDrone(name , maxWeight , status , battary);

                                        Console.WriteLine();

                                    }
                                    break;
                                case 3:
                                    {
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

                                        dalObject.AddCustumer(name, phone , longtude, lattitude);
                                        Console.WriteLine();
                                    }
                                    break;
                                case 4:
                                    {
                                        Console.WriteLine("enter the sender ID: ");
                                        int senderId;
                                        int.TryParse(Console.ReadLine(), out senderId);
                                        
                                        Console.WriteLine("enter the target ID: ");
                                        int targetId;
                                        int.TryParse(Console.ReadLine(), out targetId);
                                        
                                        Console.WriteLine("enter the weight of the parcel: ");
                                        IDAL.DO.WeightCategories weight;
                                        IDAL.DO.WeightCategories.TryParse(Console.ReadLine(), out weight);
                                        
                                        Console.WriteLine("enter the praiority of the parcel: ");
                                        IDAL.DO.Priorities priority;
                                        IDAL.DO.Priorities.TryParse(Console.ReadLine(), out priority);
                                  
                                        //printing the ids of the relevent drones.
                                        Console.WriteLine("choose the drone id from the avaleble drones (not in Maintenance and can carry the weight of your parcel): ");
                                        Console.Write("the avaleble drones are: ");
                                        List<IDAL.DO.Drone> capableDrones = dalObject.GetDroneForParcel(weight);
                                        foreach(IDAL.DO.Drone drone in capableDrones)
                                        {
                                            Console.Write(drone.id + " ");
                                        }
                                        Console.WriteLine();
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);


                                        DateTime requested = DateTime.Now;
                                        
                                        Console.WriteLine("How many days from now you want to schaduel your pecel?");
                                        int numOfDaysForDelivery;
                                        int.TryParse(Console.ReadLine(), out numOfDaysForDelivery);
                                        DateTime scheduled  = requested.AddDays(numOfDaysForDelivery);

                                        dalObject.AddParcel(senderId, targetId, weight, priority, droneId, requested, scheduled);

                                        Console.WriteLine();
                                   }
                                    break;
                            }

                        }
                        break;
                    case 2:
                        {
                            Console.WriteLine("\n" +
                                              "enter your choice: \n" +
                                              "for updating the drone id for a parcel: 1. \n" +
                                              "for picking up a parcel by a drone: 2. \n" +
                                              "for delivering a parcel: 3.\n" +
                                              "for snding a drone to a base station: 4.\n"+
                                              "for releasing a drone from a base station: 5.\n");

                            int updatingChoice;
                            int.TryParse(Console.ReadLine(), out updatingChoice);

                            switch (updatingChoice)
                            {
                                case 1:
                                {
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int parcelID;
                                        int.TryParse(Console.ReadLine(), out parcelID);

                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);


                                        dalObject.UpdateDroneForAParcel(parcelID, droneID);

                                }break;
                                case 2:
                                {
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int parcelID;
                                        int.TryParse(Console.ReadLine(), out parcelID);

                                        dalObject.PickingUpParcel(parcelID);
                                    }
                                    break;
                                case 3:
                                {
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int parcelID;
                                        int.TryParse(Console.ReadLine(), out parcelID);

                                        dalObject.DeliveringParcel(parcelID);

                                }
                                break;
                                case 4:
                                {
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);


                                        Console.WriteLine("pick up the number of the baseStation out of the avalible ones: ");
                                        Console.Write("the avalible bases are: ");
                                        List<IDAL.DO.BaseStation> freeBaseStations = dalObject.GetFreeStations();
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
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);

                                        dalObject.UnChargeDrone(droneID);
                                    }
                                break;

                            }
                            
                        }
                        break;
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
                                        Console.WriteLine("enter the number of the base station: ");
                                        int baseID;
                                        int.TryParse(Console.ReadLine(), out baseID);

                                        IDAL.DO.BaseStation baseStation = dalObject.GetBaseStation(baseID);
                                        Console.WriteLine(baseStation.ToString());

                                    }
                                    break;
                                case 2:
                                    {
                                        Console.WriteLine("enter the number of the drone: ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);

                                        IDAL.DO.Drone drone = dalObject.GetDrone(droneID);
                                        Console.WriteLine(drone.ToString());
                                    }
                                    break;
                                case 3:
                                    {
                                        Console.WriteLine("enter the number of the customer: ");
                                        int customerID;
                                        int.TryParse(Console.ReadLine(), out customerID);


                                        IDAL.DO.Customer customer = dalObject.GetCustomer(customerID);
                                        Console.WriteLine(customer.ToString());
                                    }
                                    break;
                                case 4:
                                    {
                                        Console.WriteLine("enter the number of the parcel: ");
                                        int parcelID;
                                        int.TryParse(Console.ReadLine(), out parcelID);

                                        IDAL.DO.Parcel parcel = dalObject.GetParcel(parcelID);
                                        Console.WriteLine(parcel.ToString());
                                    }
                                    break;
                            }

                            }
                        break;
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
                                        IDAL.DO.BaseStation[] baseStations = dalObject.GetBaseStations();
                                        foreach(IDAL.DO.BaseStation baseStation in baseStations)
                                        {
                                            Console.WriteLine(baseStation.ToString());
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        IDAL.DO.Drone[] drones = dalObject.GetDrones();
                                        foreach (IDAL.DO.Drone drone in drones)
                                        {
                                            Console.WriteLine(drone.ToString());
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        IDAL.DO.Customer[] customers = dalObject.GetCustomers();
                                        foreach (IDAL.DO.Customer customer in customers)
                                        {
                                            Console.WriteLine(customer.ToString());
                                        }
                                    }
                                    break;
                                case 4:
                                    {
                                        IDAL.DO.Parcel[] parcels = dalObject.GetParcheses();
                                        foreach (IDAL.DO.Parcel parcel in parcels)
                                        {
                                            Console.WriteLine(parcel.ToString());
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        List<IDAL.DO.Parcel> noDroneParcels = dalObject.GetParcelToDrone();
                                        foreach(IDAL.DO.Parcel parcel in noDroneParcels)
                                        {
                                            Console.WriteLine(parcel.ToString());
                                        }
                                    }
                                    break;
                                case 6:
                                    {
                                        List<IDAL.DO.BaseStation> freebaseStations = dalObject.GetFreeStations();
                                        foreach (IDAL.DO.BaseStation baseStation in freebaseStations)
                                        {
                                            Console.WriteLine(baseStation.ToString());
                                        }
                                    }
                                    break;
                            }

                        }
                        break;
                }

                choice = inputChoice();

            }

           
        }
    }
}
