using System;
using System.Collections.Generic;
using DalObject;

namespace IBL
{
    namespace BO
    {
        public class BL : IBL
        {
            private IDAL.IDal dalObject;
            
            private double free; // for the electricity use of a free drone
            private double light; // for the electricity use of a drone that carrys a light wight.
            private double middel; // for the electricity use of a drone that carrys a middle wight.
            private double heavy; // for the electricity use of a drone that carrys a heavy wight.
            private double chargingSpeed;//for the speed of the charge. precentage for hour.

            private List<IBAL.BO.Drone> drones;

            public BL()
            {
                dalObject = new DalObject.DalObject();

                double[] electricityUse = dalObject.ElectricityUse();

                this.free = electricityUse[0];
                this.light = electricityUse[1];
                this.middel = electricityUse[2];
                this.heavy = electricityUse[3];
                this.chargingSpeed = electricityUse[4];

                DateTime nulldate = new DateTime();//needed for comperation

                Dictionary<int, int> dronesToUpDate = dalObject.GetDronesToUpdate();

                foreach (IDAL.DO.Drone drone in dalObject.GetDrones())
                {
                    if(dronesToUpDate.ContainsKey(drone.id)){
                        if(dalObject.GetParcel(dronesToUpDate[drone.id]).pickedUp == nulldate)
                        {
                            int senderId = dalObject.GetParcel(dronesToUpDate[drone.id]).senderId;
                            int clothestBaseStation = dalObject.GetClothestStation(senderId);
                            IBAL.BO.Location baseLocation = new IBAL.BO.Location() { longitude = dalObject.GetBaseStation(clothestBaseStation).longitude, lattitude = dalObject.GetBaseStation(clothestBaseStation).lattitude };
                            this.drones.Add(new IBAL.BO.Drone() { id = drone.id, model = drone.model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight , Status = Enums.DroneStatuses.DELIVERY , location = baseLocation});
                        }
                        else
                        {
                            int senderId = dalObject.GetParcel(dronesToUpDate[drone.id]).senderId;
                            IBAL.BO.Location senderLocation = new IBAL.BO.Location() { longitude = dalObject.GetCustomer(senderId).longitude , lattitude = dalObject.GetCustomer(senderId).lattitude };
                            this.drones.Add(new IBAL.BO.Drone() { id = drone.id, model = drone.model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight, Status = Enums.DroneStatuses.DELIVERY, location = senderLocation});
                        }
                    }
                    else
                    {

                    }

                    this.drones.Add(new IBAL.BO.Drone() { id =  drone.id, model = drone.model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight });

                }


                

                Console.WriteLine(this.drones[0]);

                

            }//end BL ctor

        }//END BL class
    }//end BO
}//end IBAL
