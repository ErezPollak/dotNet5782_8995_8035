using System;
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



            dalObject.showDrones();

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


                                        dalObject.addBaseStation(name, longtude, lattitude, chargeslots);

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


                                        dalObject.addDrone(name , maxWeight , status , battary);

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

                                        dalObject.addCustumer(name, phone , longtude, lattitude);
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
                                  
                                        Console.WriteLine("choose the drone id from the avaleble drones (not in Maintenance and can carry the weight of your parcel): ");
                                        Console.Write("the avaleble drones are: ");
                                        dalObject.showListOfDronesForPercel(weight);
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);


                                        DateTime requested = DateTime.Now;
                                        
                                        Console.WriteLine("How many days from now you want to schaduel your pecel?");
                                        int numOfDaysForDelivery;
                                        int.TryParse(Console.ReadLine(), out numOfDaysForDelivery);
                                        DateTime scheduled  = requested.AddDays(numOfDaysForDelivery);

                                        dalObject.addParcel(senderId, targetId, weight, priority, droneId, requested, scheduled);

                                        Console.WriteLine();
                                   }
                                    break;
                            }

                        }
                        break;
                    case 2:
                        {

                        }
                        break;
                    case 3:
                        {

                        }
                        break;
                    case 4:
                        {

                        }
                        break;
                }

                choice = inputChoice();

            }

           
        }
    }
}
