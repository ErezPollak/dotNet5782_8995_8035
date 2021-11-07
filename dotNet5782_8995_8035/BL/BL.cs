using System;
using System.Collections.Generic;
using DalObject;
using IDAL.DO;
using IBAL.BO;

namespace IBL
{
    public class BL : IBL
    {
            private IDAL.IDal dalObject;
            
            private double free; // for the electricity use of a free drone
            private double light; // for the electricity use of a drone that carrys a light wight.
            private double middel; // for the electricity use of a drone that carrys a middle wight.
            private double heavy; // for the electricity use of a drone that carrys a heavy wight.
            private double chargingSpeed;//for the speed of the charge. precentage for hour.

            private List<IBAL.BO.DroneForList> drones;


            //static DateTime nulldate = new DateTime();//needed for comperation


            public BL()
            {

                dalObject = new DalObject.DalObject();

                double[] electricityUse = dalObject.ElectricityUse();

                this.free = electricityUse[0];
                this.light = electricityUse[1];
                this.middel = electricityUse[2];
                this.heavy = electricityUse[3];
                this.chargingSpeed = electricityUse[4];

                
                foreach(IDAL.DO.Drone dalDrone in dalObject.GetDrones())
                {
                    
                    this.drones.Add(new DroneForList() { Id = dalDrone.Id, Model = dalDrone.Model, Weight = (IBAL.BO.Enums.WeightCategories)dalDrone.MaxWeight });    
                }
             
               foreach(IDAL.DO.Parcel parcel in dalObject.GetParcelToDrone())
                {
                    
                }


                //Dictionary<int, int> dronesToUpDate = dalObject.GetDronesToUpdate();
                //foreach (IDAL.DO.Drone drone in dalObject.GetDrones())
                //{
                //if (dronesToUpDate.ContainsKey(drone.Id))
                //{
                //    if (dalObject.GetParcel(dronesToUpDate[drone.Id]).PickedUp == nulldate)
                //    {
                //        int senderId = dalObject.GetParcel(dronesToUpDate[drone.Id]).SenderId;
                //        int clothestBaseStation = dalObject.GetClothestStation(senderId);
                //        global::IBAL.BO.Location baseLocation = new global::IBAL.BO.Location() { Longitude = dalObject.GetBaseStation(clothestBaseStation).longitude, Lattitude = dalObject.GetBaseStation(clothestBaseStation).lattitude };
                //        this.drones.Add(new global::IBAL.BO.Drone() { Id = drone.Id, Model = drone.Model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight, Status = Enums.DroneStatuses.DELIVERY, location = baseLocation });
                //    }
                //    else
                //    {
                //        int senderId = dalObject.GetParcel(dronesToUpDate[drone.Id]).SenderId;
                //        global::IBAL.BO.Location senderLocation = new global::IBAL.BO.Location() { Longitude = dalObject.GetCustomer(senderId).Longitude, Lattitude = dalObject.GetCustomer(senderId).Llattitude };
                //        this.drones.Add(new global::IBAL.BO.Drone() { Id = drone.Id, Model = drone.Model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight, Status = Enums.DroneStatuses.DELIVERY, location = senderLocation });
                //    }
                //}
                //else
                //{

                //}

                //this.drones.Add(new IBAL.BO.Drone() { id = drone.id, model = drone.model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight });

                //}

                //Console.WriteLine(this.drones[0]);

            }//end BL ctor



       ////**** adding option ****////


        public void AddBaseStation(IBAL.BO.BaseStation newBaseStation)
        {
            dalObject.AddBaseStation(newBaseStation.Id, newBaseStation.Name, newBaseStation.Location.Longitude, newBaseStation.Location.Lattitude , newBaseStation.ChargeSlots);
        }



        public void AddDrone(IBAL.BO.DroneForList newDrone)
        {
            foreach(IBAL.BO.DroneForList drone in this.drones)
            {
                if(drone.Id == newDrone.Id)
                {
                    throw new IdAlreadyExistsException(drone.Id , "drone");
                }
            }
            this.drones.Add(newDrone);
            dalObject.AddDrone(newDrone.Id, newDrone.Model , (IDAL.DO.WeightCategories)newDrone.Weight);
        }



        public void AddCustumer(IBAL.BO.Customer newCustomer)
        {
            dalObject.AddCustumer(newCustomer.Id , newCustomer.Name , newCustomer.Phone , newCustomer.Location.Longitude , newCustomer.Location.Lattitude);
        }


        public void AddParcel(IBAL.BO.Parcel parcel)
        {
            dalObject.AddParcel(parcel.Id, parcel.Reciver.Id , parcel.Sender.Id , (IDAL.DO.WeightCategories)parcel.Weight,  (IDAL.DO.Priorities)parcel.Priority , parcel.Drone.Id , parcel.CreationTime, parcel.DeliveringTime);
        }


        ////**** update options ****////


        public void UpdateDroneForAParcel(int parcelId, int droneId)
        {
            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach (IDAL.DO.Parcel parcel in dalObject.GetParcels())
            {
                if (parcel.Id == parcelId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IBAL.BO.IdDontExistsException(parcelId, "parcel");

            isNameExists = false;
            foreach (IDAL.DO.Drone drone in dalObject.GetDrones())
            {
                if (drone.Id == droneId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            //if the id numbers were found in the lists we can call the function from Idal.
            dalObject.UpdateDroneForAParcel(parcelId, droneId);
        }



        public void PickingUpParcel(int parcelId)
        {

            bool isNameExists = false;
            foreach (IDAL.DO.Parcel parcel in dalObject.GetParcels())
            {
                if (parcel.Id == parcelId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IBAL.BO.IdDontExistsException(parcelId, "parcel");

            dalObject.PickingUpParcel(parcelId);
        }

        public void DeliveringParcel(int parcelId)
        {

            bool isNameExists = false;
            foreach (IDAL.DO.Parcel parcel in dalObject.GetParcels())
            {
                if (parcel.Id == parcelId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IBAL.BO.IdDontExistsException(parcelId, "parcel");

            dalObject.DeliveringParcel(parcelId);
        }

        public void ChargeDrone(int baseStationId, int droneId)
        {

            bool isNameExists = false;
            foreach (IDAL.DO.BaseStation baseStation in dalObject.GetBaseStations())
            {
                if (baseStation.id == baseStationId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IBAL.BO.IdDontExistsException(baseStationId, "base station");

            isNameExists = false;
            foreach (IDAL.DO.Drone drone in dalObject.GetDrones())
            {
                if (drone.Id == droneId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            dalObject.ChargeDrone(baseStationId, droneId);
        }





        public void UnChargeDrone(int droneId)
        {

            bool isNameExists = false;
            foreach (IDAL.DO.Drone drone in dalObject.GetDrones())
            {
                if (drone.Id == droneId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "drone");

            dalObject.UnChargeDrone(droneId);
        }


        ////**** getting options ****////
        
        /////// !!!!!!!Exceptions!!!!!!!!!!!!!    ///////////////////

        public IBAL.BO.BaseStation GetBaseStation(int baseStationId)
        {
            IDAL.DO.BaseStation dalBaseStation = dalObject.GetBaseStation(baseStationId);
            return new IBAL.BO.BaseStation()
            {
                Id = dalBaseStation.id,
                Name = dalBaseStation.name,
                ChargeSlots = dalBaseStation.chargeSlots,
                Location = new IBAL.BO.Location()
                {
                    Longitude = dalBaseStation.longitude,
                    Lattitude = dalBaseStation.lattitude
                }
            };
        }

        public IBAL.BO.DroneForList GetDrone(int droneId)
        {
            for (int i = 0; i < this.drones.Count; i++)
            {
                if(drones[i].Id == droneId)
                {
                    return drones[i];
                }
            }
            throw new IBAL.BO.IdDontExistsException(droneId, "drone");
        }

        public IBAL.BO.Customer GetCustomer(int customerId)
        {
            IDAL.DO.Customer dalCustomer = dalObject.GetCustomer(customerId);
            return new IBAL.BO.Customer()
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = new Location() {
                    Longitude = dalCustomer.Longitude,
                    Lattitude = dalCustomer.Llattitude 
                }
            };
        }

        public IBAL.BO.Parcel GetParcel(int parcelId)
        {
            IDAL.DO.Parcel dalParcel = dalObject.GetParcel(parcelId);
            IBAL.BO.CoustomerForParcel sender = new CoustomerForParcel() { Id = dalParcel.SenderId, CustomerName = dalObject.GetCustomer(dalParcel.SenderId).Name };
            IBAL.BO.CoustomerForParcel reciver = new CoustomerForParcel() { Id = dalParcel.TargetId, CustomerName = dalObject.GetCustomer(dalParcel.TargetId).Name };
            IBAL.BO.DroneForParcel drone = new DroneForParcel() { Id = dalParcel.DroneId };
          

            return new IBAL.BO.Parcel()
            {
                Id = dalParcel.Id,
                Sender = sender,
                Reciver = reciver,
                Drone = drone,
                Priority = (IBAL.BO.Enums.Priorities)dalParcel.Priority,
                AssigningTime = DateTime.Now,
                CreationTime = dalParcel.Requested,
                DeliveringTime = dalParcel.Scheduled,
                PickupTime = dalParcel.PickedUp
            };
        }

        public IEnumerable<IBAL.BO.BaseStation> GetBaseStations()
        {
            List<IBAL.BO.BaseStation> baseStations = new List<IBAL.BO.BaseStation>();
            foreach(IDAL.DO.BaseStation baseStation in dalObject.GetBaseStations())
            {
                baseStations.Add(new IBAL.BO.BaseStation()
                {
                    Id = baseStation.id,
                    Name = baseStation.name,
                    ChargeSlots = baseStation.chargeSlots,
                    Location = new IBAL.BO.Location()
                    {
                        Longitude = baseStation.longitude,
                        Lattitude = baseStation.lattitude
                    }
                });
            }
            return baseStations;
        }

        public IEnumerable<IBAL.BO.DroneForList> GetDrones()
        {
            return this.drones;
        }

        public IEnumerable<IBAL.BO.Customer> GetCustomers()
        {
            List<IBAL.BO.Customer> customers = new List<IBAL.BO.Customer>();
            foreach(IDAL.DO.Customer customer in dalObject.GetCustomers())
            {
               customers.Add( new IBAL.BO.Customer()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Location = new Location()
                    {
                        Longitude = customer.Longitude,
                        Lattitude = customer.Llattitude
                    }
                });
            }
            return customers;
        }

        public IEnumerable<IBAL.BO.Parcel> GetPacelss()
        {
            List<IBAL.BO.Parcel> parcels = new List<IBAL.BO.Parcel>();
            foreach(IDAL.DO.Parcel dalParcel in dalObject.GetParcels())
            {
                IBAL.BO.CoustomerForParcel sender = new CoustomerForParcel() { Id = dalParcel.SenderId, CustomerName = dalObject.GetCustomer(dalParcel.SenderId).Name };
                IBAL.BO.CoustomerForParcel reciver = new CoustomerForParcel() { Id = dalParcel.TargetId, CustomerName = dalObject.GetCustomer(dalParcel.TargetId).Name };
                IBAL.BO.DroneForParcel drone = new DroneForParcel() { Id = dalParcel.DroneId };

                parcels.Add( new IBAL.BO.Parcel()
                {
                    Id = dalParcel.Id,
                    Sender = sender,
                    Reciver = reciver,
                    Drone = drone,
                    Priority = (IBAL.BO.Enums.Priorities)dalParcel.Priority,
                    AssigningTime = DateTime.Now,
                    CreationTime = dalParcel.Requested,
                    DeliveringTime = dalParcel.Scheduled,
                    PickupTime = dalParcel.PickedUp
                });
            }
            return parcels;
        }

        //public IEnumerable<IBAL.BO.Parcel> GetParcelToDrone()
        //{
        //    List<IBAL.BO.Parcel> parcels = new List<IBAL.BO.Parcel>();
        //    foreach (IDAL.DO.Parcel parcel in dalObject.GetParcelToDrone())
        //    {
        //        parcels.Add(new IBAL.BO.Parcel()
        //        {
        //            Id = parcel.Id,
        //            SenderId = parcel.SenderId,
        //            TargetId = parcel.TargetId,
        //            DroneId = parcel.DroneId,
        //            Requested = parcel.Requested,
        //            Scheduled = parcel.Scheduled,
        //            priority = (IBAL.BO.Enums.Priorities)parcel.Priority,
        //            Weight = (IBAL.BO.Enums.WeightCategories)parcel.Weight,
        //            PickedUp = parcel.PickedUp,
        //            Delivered = parcel.Delivered
        //        });
        //    }
        //    return parcels;
        //}



    }//END BL class
}//end IBAL
