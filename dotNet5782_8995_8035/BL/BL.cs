using System;
using System.Collections.Generic;
using DalObject;
using IDAL.DO;
using IBAL.BO;
using System.Linq;
using System.Runtime.Serialization;

namespace IBL
{
    public class BL : IBL
    {

        /// <summary>
        /// 
        /// </summary>
        private IDAL.IDal dalObject;

        private double free; // for the electricity use of a free drone
        private double light; // for the electricity use of a drone that carrys a light wight.
        private double middel; // for the electricity use of a drone that carrys a middle wight.
        private double heavy; // for the electricity use of a drone that carrys a heavy wight.
        private double chargingSpeed;//for the speed of the charge. precentage for hour.


        /// <summary>
        /// 
        /// </summary>
        private List<IBAL.BO.DroneForList> drones;


        /// <summary>
        /// 
        /// </summary>
        private static Random r = new Random();     // a static value for 

        //static DateTime nulldate = new DateTime();//needed for comperation

        
        /// <summary>
        /// constractor for the Bl class
        /// </summary>
        public BL()
        {

            dalObject = new DalObject.DalObject();

            drones = new List<DroneForList>();

            double[] electricityUse = dalObject.ElectricityUse();

            this.free = electricityUse[0];
            this.light = electricityUse[1];
            this.middel = electricityUse[2];
            this.heavy = electricityUse[3];
            this.chargingSpeed = electricityUse[4];

            //translats all the drones from the data lavel to 
            foreach (IDAL.DO.Drone dalDrone in dalObject.GetDrones(d => true).ToList())
            {
                this.drones.Add(new DroneForList()
                { 
                    Id = dalDrone.Id, 
                    Model = dalDrone.Model, 
                    Weight = (IBAL.BO.Enums.WeightCategories)dalDrone.MaxWeight, 
                    Status = (IBAL.BO.Enums.DroneStatuses)(r.Next() % 2 * 2) 
                });
            }

            //going through all the parcels that have a drone, and was not delivered.
            foreach (IDAL.DO.Parcel parcel in dalObject.GetParcels(parcel => parcel.DroneId != -1 && parcel.AcceptedTime == null))
            {
                //caculate the distance of the delivery.
                double deliveryDistance = 0;
                //caculating the distance between the sender of the parcel, and the reciver. 
                deliveryDistance += distance(locationTranslate(dalObject.GetCustomer(parcel.SenderId).Location), locationTranslate(dalObject.GetCustomer(parcel.TargetId).Location));
                //caculating the distance between the reciver of the parcel, and the clothest station to the reciver. 
                deliveryDistance += distance(locationTranslate(dalObject.GetCustomer(parcel.TargetId).Location), locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcel.TargetId)).Location));

                double minimumValue;

                //updates the status of the drone to be delivery. 
                this.GetDrone(parcel.DroneId).Status = IBAL.BO.Enums.DroneStatuses.DELIVERY;
                //updates the parcel number to be delivery.
                this.GetDrone(parcel.DroneId).ParcelId = parcel.Id;

                //if the parcel wasnt picked up.
                if (parcel.PickedUpTime == null)
                {
                    //seting the location of the drone to be in the clothest station to the sender.
                    this.GetDrone(parcel.DroneId).Location = locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcel.SenderId)).Location);

                    // caculating the distance between the base station to the sender.
                    deliveryDistance += distance(locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcel.SenderId)).Location), locationTranslate(dalObject.GetCustomer(parcel.SenderId).Location));

                    // caculating the minimum precentage of batteary the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance / (int)(dalObject.ElectricityUse()[(int)this.GetDrone(parcel.DroneId).Weight + 1]);

                }
                else // if the parcel was picked up.
                {
                    //seting the location of the drone to be the location of the sender.
                    this.GetDrone(parcel.DroneId).Location = locationTranslate(dalObject.GetCustomer(parcel.SenderId).Location);

                    // caculating the minimum precentage of batteary the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance / (int)(dalObject.ElectricityUse()[(int)this.GetDrone(parcel.DroneId).Weight + 1]);

                }
                
                //if the precentage is ok, the value of the battry is being randomiseied between the minimum value to one handred.
                this.GetDrone(parcel.DroneId).Battary = minimumValue + r.Next() % (100 - minimumValue);

            }

            //needed for the iteration.
            List<IDAL.DO.Parcel> parcels = dalObject.GetParcels(p => true).ToList();

            //going through all the drones.
            foreach (IBAL.BO.DroneForList drone in this.drones)
            {
                // if the drone wasn't changed in the last iteration, meaning that the status is either FREE or MAINTENANCE.
                if (drone.Status == Enums.DroneStatuses.FREE)
                {
                    //getting random value from the list.
                    int index = r.Next() % (parcels.Count);

                    //seting the drones location the drone to be in a target location of the randomisied parcel.
                    drone.Location = locationTranslate(dalObject.GetCustomer(parcels[index].TargetId).Location);

                    //caculating the distance to the clothest station.
                    double deliveryDistance = distance(locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcels[index].TargetId)).Location), locationTranslate(dalObject.GetCustomer(parcels[index].TargetId).Location));

                    //caculating the battary consamption.
                    double battayConcamption = deliveryDistance / dalObject.ElectricityUse()[(int)drone.Weight + 1];

                    //the there is not enough battary, exception will be thrown.
                    //if (battayConcamption > 100) throw new IBAL.BO.BL_ConstaractorException($"the drone needs {battayConcamption} battary in order to complete to delivery. ");

                    //seting the battry to be randomised between the minimum value to 100.
                    drone.Battary = (int)(battayConcamption + r.NextDouble() * (100 - battayConcamption));

                }
                else if (drone.Status == Enums.DroneStatuses.MAINTENANCE)
                {

                    List<IDAL.DO.BaseStation> avalibleBaseStations = dalObject.GetBaseStations(b => b.ChargeSlots > 0).ToList();

                    //random number of a station.
                    int stationNumber = r.Next() % avalibleBaseStations.Count;
                    
                    //setting the location of the drone to be the location of the randomaised station.
                    drone.Location = locationTranslate(avalibleBaseStations[stationNumber].Location);

                    // updating the battary to be a random value from 0 to 20.
                    drone.Battary = r.Next() % 20;

                    //sending the drone to charge.
                    dalObject.ChargeDrone(stationNumber, drone.Id);

                }

            }



        }//end BL ctor



        ////**** adding option ****////
        
        
        /// <summary>
        /// the function gets a logical base station, converting it to a dal basestation and adding it to the database. 
        /// </summary>
        /// <param name="lpgical basestation"></param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddBaseStation(IBAL.BO.BaseStation newBaseStation)
        {
            IDAL.DO.BaseStation baseStation = new IDAL.DO.BaseStation()
            {
                Id = newBaseStation.Id,
                Name = newBaseStation.Name,
                Location = locationTranslate(newBaseStation.Location),
                ChargeSlots = newBaseStation.ChargeSlots
            };

            try
            {
                return dalObject.AddBaseStation(baseStation);
            }catch(Exception e)
            {
                throw new IBAL.BO.IdAlreadyExistsException(newBaseStation.Id, "base station", e);
            }
        }


        /// <summary>
        /// the function gets a logical drone, converting it to a dal drone and adding it to the database. 
        /// </summary>
        /// <param name="lpgical dronre"></param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddDrone(IBAL.BO.Drone newDrone)
        {
            
            if (this.drones.Any(d => d.Id == newDrone.Id)) throw new IBAL.BO.IdAlreadyExistsException(newDrone.Id, "droen");

            int parcel;
            if(newDrone.ParcelInDelivery == null)
            {
                parcel = -1;
            }
            else
            {
                parcel = newDrone.ParcelInDelivery.Id;
            }

            if(newDrone.Location == null)
            {
                List<IBAL.BO.BaseStationForList> avalibaleBaseStations =  GetBaseStations(b => b.ChargeSlots > 0).ToList();
                if (avalibaleBaseStations.Count == 0) throw new IBAL.BO.UnableToAddDroneException("No BaseStation Avalible");
                newDrone.Location = GetBaseStation(avalibaleBaseStations[r.Next(avalibaleBaseStations.Count)].Id).Location;
            }

            IBAL.BO.DroneForList balDrone = new IBAL.BO.DroneForList()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                Weight = newDrone.MaxWeight,
                Battary = newDrone.Battery,
                Location = newDrone.Location,
                Status = newDrone.Status,
                ParcelId = parcel
            };

            this.drones.Add(balDrone);
            
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight
            };

            //since we checked in the list there is no chance to have the same number of drone in the dal list.   
            bool b = dalObject.AddDrone(dalDrone);

            this.ChargeDrone(newDrone.Id);

            return b;
        }


        /// <summary>
        /// the function gets a logical customer, converting it to a dal custamer and adding it to the database. 
        /// </summary>
        /// <param name="lpgical custumer"></param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddCustumer(IBAL.BO.Customer newCustomer)
        {
            IDAL.DO.Customer customer = new IDAL.DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Location = locationTranslate(newCustomer.Location)
            };

            return dalObject.AddCustumer(customer);
        }


        /// <summary>
        /// the function gets a logical parcel, converting it to a dal parcel and adding it to the database. 
        /// </summary>
        /// <param name="lpgical basestation"></param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddParcel(IBAL.BO.Parcel newParcel)
        {
            IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Reciver.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Priority,
                DroneId = -1,
                RequestedTime = newParcel.RequestedTime,
                DeliveryTime = newParcel.DeliveringTime,
                AcceptedTime = newParcel.AcceptedTime,
                PickedUpTime = newParcel.PickupTime
            };

            return dalObject.AddParcel(dalParcel);
        
        }


        ////**** update options ****////

        
        /// <summary>
        /// calling the function from the dal that changesd the name of the drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        /// <returns> true if the updaue complited successfully </returns>
        public bool UpdateNameForADrone(int droneId, string model)
        {

            this.GetDrone(droneId).Model = model;

            try
            {
                return dalObject.UpdateNameForADrone(droneId, model);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdDontExistsException(droneId, "drone", e);
            }

        }


        /// <summary>
        /// calling the function from the dal that changesd the name and number of slots of the base sation.
        /// </summary>
        /// <param name="basStationID"></param>
        /// <param name="name"></param>
        /// <param name="slots"></param>
        /// <returns> true if the updaue complited successfully </returns>
        public bool UpdateBaseStation(int basStationID, string name, int slots)
        {
            try
            {
                return dalObject.UpdateBaseStation(basStationID, name, slots);
            }
            catch(Exception e)
            {
                throw new IBAL.BO.IdDontExistsException(basStationID, "baseStation" , e);
            }

        }

        /// <summary>
        ///  calling the function from the dal that changesd the name and phone number of a customer.
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool UpdateCustomer(int customerID, string name, string phone)
        {
            try
            {
                return dalObject.UpdateCustomer(customerID, name, phone);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdDontExistsException(customerID, "customer", e);
            }
        }

       
        /// <summary>
        /// finding the best parcel and assining it to the given drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool AssignParcelToADrone(int droneId)
        {
            //fining the index of the drone with that id.
            int droneIndex = this.drones.FindIndex(d => d.Id == droneId);

            //if the index is -1 it means that no such is id in the database so an exception will be thrown.
            if (droneIndex == -1) throw new IBAL.BO.UnableToAssignParcelToTheDroneException(droneId , " drone is not in the database." , new IBAL.BO.IdDontExistsException(droneId , "drone"));//IBAL.BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (this.drones[droneIndex].Status != Enums.DroneStatuses.FREE) throw new IBAL.BO.UnableToAssignParcelToTheDroneException(droneId , " the drone is not free");

            //importing all the parcels and sorting them aaccording to their praiority.
            List<IDAL.DO.Parcel> parcels = dalObject.GetParcels(p => true).OrderBy(p => (int)p.Priority).ToList();

            //removing from the list all the parcels that wight more than the drone can carry.
            parcels.RemoveAll(p => (int)p.Weight > (int)drones[droneIndex].Weight);

            //if no parcel left in the list after the removings it means that no parcel can be sent be thi drone, so exception will be thrown.
            if (parcels.Count == 0) throw new UnableToAssignParcelToTheDroneException(droneId, " there is no parcel that can be sent by this drone due to: all the parcels are too heavy.");

            //sorting by the distanse between the drone's locaation and the parcel's location.
            parcels = parcels.OrderBy(p => distance(drones[droneIndex].Location, locationTranslate(dalObject.GetCustomer(p.SenderId).Location))).ToList();

            //removing all the drones that dont have enough battary for the jerny from the sender to the reciver and to the clothest base station.
            parcels.RemoveAll(p => distance(drones[droneIndex].Location, locationTranslate(dalObject.GetCustomer(p.SenderId).Location))  /*adding the distance between the reciver to the base station*/ >= drones[droneIndex].Battary * dalObject.ElectricityUse()[(int)drones[droneIndex].Weight + 1]);

            //if no parcel left in the list after the removings it means that no parcel can be sent be thi drone, so exception will be thrown.
            if (parcels.Count == 0) throw new UnableToAssignParcelToTheDroneException(droneId , " there is no parcel that can be sent by this drone due to: too long distanses");

            //if there is a parcel that matches the needings of the drone the rewwuaiered will happen.
            this.drones[droneIndex].Status = Enums.DroneStatuses.DELIVERY;

            this.drones[droneIndex].ParcelId = parcels.First().Id;

            //calling to the function from the dal to make the changes in the data level.
            return dalObject.PickingUpParcel(parcels.First().Id, droneId);

        }
        
        
        /// <summary>
        /// the function is deliveriong the parcel that the drone is delivering 
        /// and updating the properties according to the distance of the delivery.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool DeliveringParcelFromADrone(int droneId)
        {
            
            //if the drone is not in thr database, an exception will be thrown.
            if (this.GetDrone(droneId).Status != Enums.DroneStatuses.DELIVERY) throw new UnableToDeliverParcelFromTheDroneException(droneId , "the drone is not delivering any parcel.");

            //caculating the distance of the delivery.
            double deliveryDistance = distance(locationTranslate(dalObject.GetCustomer(dalObject.GetParcel(this.GetDrone(droneId).ParcelId).SenderId).Location), locationTranslate(dalObject.GetCustomer(dalObject.GetParcel(this.GetDrone(droneId).ParcelId).TargetId).Location));

            //battary update
            this.GetDrone(droneId).Battary -= deliveryDistance / dalObject.ElectricityUse()[(int)this.GetDrone(droneId).Weight + 1];

            // location update
            this.GetDrone(droneId).Location = locationTranslate(dalObject.GetCustomer(dalObject.GetParcel(this.GetDrone(droneId).ParcelId).TargetId).Location);/////adapt to function.

            //status update
            this.GetDrone(droneId).Status = Enums.DroneStatuses.FREE;

            //update the parcel from the dal.
            return dalObject.DeliveringParcel(this.GetDrone(droneId).ParcelId);
            
        }

        /// <summary>
        /// the function finds the base station and sending the drone to charge there by the dal function.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool ChargeDrone(int droneId)
        {
            int droneIndex = this.drones.FindIndex(d => d.Id == droneId);

            if (droneIndex == -1) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (this.drones[droneIndex].Status != Enums.DroneStatuses.FREE) throw new UnAbleToSendDroneToChargeException(" the drone is not free");

            //geting the maximum distance the drone can make according to the level of battary multiplied by the number of kilometers the drone can do for one precent do battary. 
            double maxDistance = this.drones[droneIndex].Battary * dalObject.ElectricityUse()[(int)this.drones[droneIndex].Weight + 1];

            //returns all the stations that the drone has enough fuel to get to.
            List<IDAL.DO.BaseStation> baseStations = dalObject.GetBaseStations(delegate (IDAL.DO.BaseStation b) { return distance(locationTranslate(b.Location), GetDrone(droneId).Location) <= maxDistance; }).ToList();

            //sorting the stations according to the distance from the drone.
            baseStations = baseStations.OrderBy(b => distance(locationTranslate(b.Location), GetDrone(droneId).Location)).ToList();

            //gives the first station in the list that has free slots for charging.
            //IDAL.DO.BaseStation baseStation = baseStations.First(b => b.ChargeSlots > 0);

            baseStations.RemoveAll(b => b.ChargeSlots <= 0);

            //if there is no suitable station an exception will be thrown.
            if (baseStations.Count == 0) throw new UnAbleToSendDroneToChargeException(" there is no station that matches the needs of this drone");

            IDAL.DO.BaseStation baseStation = baseStations.First();

            /////drone updates//////

            //update the battary status.
            this.drones[droneIndex].Battary -= distance(locationTranslate(baseStation.Location), GetDrone(droneId).Location) / dalObject.ElectricityUse()[(int)this.drones[droneIndex].Weight + 1];

            //update the localtion of the drone to be the locatin of the base station.
            this.drones[droneIndex].Location = locationTranslate(baseStation.Location);

            //updating the status of the drone.
            this.drones[droneIndex].Status = Enums.DroneStatuses.MAINTENANCE;

            //update the number of the charge slots and adding an element in the dal functions.
            return dalObject.ChargeDrone(baseStation.Id, droneId);

        }

        /// <summary>
        /// the function uncharging a drone from its base station.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public bool UnChargeDrone(int droneId , int minutes)
        {
            int droneIndex = this.drones.FindIndex(d => d.Id == droneId);
            if (droneIndex == -1) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (this.drones[droneIndex].Status != Enums.DroneStatuses.MAINTENANCE) throw new UnAbleToReleaseDroneFromChargeException(droneId ,"the drone is not in maintanance");

            //update the battary status.
            this.drones[droneIndex].Battary += minutes * dalObject.ElectricityUse()[4];
            if(this.drones[droneIndex].Battary > 100)
            {
                this.drones[droneIndex].Battary = 100;
            }

            //updating the status of the drone.
            this.drones[droneIndex].Status = Enums.DroneStatuses.FREE;

            return dalObject.UnChargeDrone(droneId);
        }


        ////**** show options ****////

        /// <summary>
        /// returns a base sattion according to its id number.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public IBAL.BO.BaseStation GetBaseStation(int baseStationId)
        {
            try
            {
                IDAL.DO.BaseStation dalBaseStation = dalObject.GetBaseStation(baseStationId);

                List<IBAL.BO.DroneInCharge> charges = cargeDroneToDroneInCharge(dalObject.GetChargeDrones(d => d.StationId == dalBaseStation.Id).ToList());
                
                return new IBAL.BO.BaseStation()
                {
                    Id = dalBaseStation.Id,
                    Name = dalBaseStation.Name,
                    ChargeSlots = dalBaseStation.ChargeSlots,
                    Location = locationTranslate(dalBaseStation.Location),
                    ChargingDrones = charges
                };
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdDontExistsException(baseStationId, "base station", e);
            }
        }

        private List<DroneInCharge> cargeDroneToDroneInCharge(List<DroneCharge> droneCharges)
        {
            List<DroneInCharge> droneInCharges = new List<DroneInCharge>();
            droneCharges.ForEach(dc => droneInCharges.Add(new DroneInCharge() { Id = dc.DroneId, Battary = GetDrone(dc.DroneId).Battary }));
            return droneInCharges;
        }

        /// <summary>
        /// returns a drone according to its id number.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public IBAL.BO.DroneForList GetDrone(int droneId)
        {
            int index = this.drones.FindIndex(d => d.Id == droneId);

            if (index == -1) throw new IBAL.BO.IdDontExistsException(droneId, "drone", new IDAL.DO.IdDontExistsException(droneId, "drone"));

            return this.drones[index];
        }

        /// <summary>
        /// returns a customer according to its id number.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IBAL.BO.Customer GetCustomer(int customerId)
        {
            try
            {
                IDAL.DO.Customer dalCustomer = dalObject.GetCustomer(customerId);
                return new IBAL.BO.Customer()
                {
                    Id = dalCustomer.Id,
                    Name = dalCustomer.Name,
                    Phone = dalCustomer.Phone,
                    Location = locationTranslate(dalCustomer.Location)
                };
            }
            catch (IDAL.DO.IdDontExistsException e)
            {
                throw new IBAL.BO.IdDontExistsException(customerId, "customer", e);
            }
        }

        /// <summary>
        /// returns a parcel according to its id number.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public IBAL.BO.Parcel GetParcel(int parcelId)
        {
            try
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
                    Weight = (IBAL.BO.Enums.WeightCategories)dalParcel.Weight,
                    AcceptedTime = dalParcel.AcceptedTime,
                    RequestedTime = dalParcel.RequestedTime,
                    DeliveringTime = dalParcel.DeliveryTime,
                    PickupTime = dalParcel.PickedUpTime
                };
            }
            catch (IDAL.DO.IdDontExistsException e)
            {
                throw new IBAL.BO.IdDontExistsException(parcelId, "parcel", e);
            }
        }


        /// <summary>
        /// returns the list of base stations that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<IBAL.BO.BaseStationForList> GetBaseStations(Predicate<IDAL.DO.BaseStation> f)
        {

            List<IBAL.BO.BaseStationForList> baseStations = new List<IBAL.BO.BaseStationForList>();
            foreach (IDAL.DO.BaseStation baseStation in dalObject.GetBaseStations(f))
            {
                baseStations.Add(new IBAL.BO.BaseStationForList()
                {
                    Id = baseStation.Id,
                    Name = baseStation.Name,
                    FreeChargingSlots = baseStation.ChargeSlots, //- dalObject.GetChargeDrones(p => true).ToList().Count(p => p.StationId == baseStation.Id),
                    TakenCharingSlots = dalObject.GetChargeDrones(cd => cd.StationId == baseStation.Id).Count()
                });
            }
            return baseStations;
        }

        /// <summary>
        ///  returns the list of drones that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<IBAL.BO.DroneForList> GetDrones(Predicate<IBAL.BO.DroneForList> f)
        {
            List<IBAL.BO.DroneForList> balDrones = new List<DroneForList>();

            this.drones.FindAll(d => f(d)).ForEach(d => balDrones.Add(new DroneForList()
            {
                Id = d.Id,
                Model = d.Model,
                Battary = d.Battary,
                Location = d.Location, 
                Weight = d.Weight,
                Status = d.Status ,
                ParcelId = d.ParcelId
            }));

            return balDrones;//return dalObject.GetDrones(f);   //this.drones.FindAll(f);
        }

        /// <summary>
        ///  returns the list of customers that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<IBAL.BO.CustomerForList> GetCustomers(Predicate<IDAL.DO.Customer> f)
        {
            List<IBAL.BO.CustomerForList> customers = new List<IBAL.BO.CustomerForList>();
            foreach (IDAL.DO.Customer customer in dalObject.GetCustomers(f))
            {
                customers.Add(new IBAL.BO.CustomerForList()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    //caculating the number of parcels that were sent to this customer that were sent and deliverd. 
                    SentToAndDeliverd = dalObject.GetParcels(p => p.TargetId == customer.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                    //caculating the number of parcels that were sent to this customer that were sent and not deliverd. 
                    SentToAnDNotDelivered = dalObject.GetParcels(p => p.TargetId == customer.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                    //caculating the number of parcels that were sent from this customer that were sent and deliverd. 
                    SentFromAndDeliverd = dalObject.GetParcels(p => p.SenderId == customer.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                    //caculating the number of parcels that were sent from this customer that were sent and not deliverd. 
                    SentFromAndNotDeliverd = dalObject.GetParcels(p => p.SenderId == customer.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                    
                });
            }
            return customers;
        }

        /// <summary>
        ///  returns the list of parcels that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<IBAL.BO.ParcelForList> GetPacels(Predicate<IDAL.DO.Parcel> f)
        {
            List<IBAL.BO.ParcelForList> parcels = new List<IBAL.BO.ParcelForList>();
            foreach (IDAL.DO.Parcel dalParcel in dalObject.GetParcels(p => true))
            {
                //IBAL.BO.CoustomerForParcel sender = new CoustomerForParcel() { Id = dalParcel.SenderId, CustomerName = dalObject.GetCustomer(dalParcel.SenderId).Name };
                //IBAL.BO.CoustomerForParcel reciver = new CoustomerForParcel() { Id = dalParcel.TargetId, CustomerName = dalObject.GetCustomer(dalParcel.TargetId).Name };
                //IBAL.BO.DroneForParcel drone = new DroneForParcel() { Id = dalParcel.DroneId };

                parcels.Add(new IBAL.BO.ParcelForList()
                {
                    Id = dalParcel.Id,
                    //Sender = sender,
                    //Reciver = reciver,
                    //Drone = drone,
                    Priority = (IBAL.BO.Enums.Priorities)dalParcel.Priority,
                    //Status = dalParcel.   need to caculate
                    SenderName = dalObject.GetCustomer(dalParcel.SenderId).Name,
                    ReciverName = dalObject.GetCustomer(dalParcel.TargetId).Name,
                    Weight = (Enums.WeightCategories)dalParcel.Weight
                    //AcceptedTime = DateTime.Now,
                    //RequestedTime = dalParcel.RequestedTime,
                    //DeliveringTime = dalParcel.DeliveryTime,
                    //PickupTime = dalParcel.PickedUpTime
                });
            }
            return parcels;
        }


        //operation functions.

        /// <summary>
        /// function that gets
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private double distance(IBAL.BO.Location l1, IBAL.BO.Location l2)
        {
            var baseRad = Math.PI * l1.Latitude / 180;
            var targetRad = Math.PI * l2.Latitude / 180;
            var theta = l1.Longitude - l2.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 9 * 1.1515; // the size in not the original size of earth.

            return dist;
        }

        /// <summary>
        /// the function gets idal locatin and transforms it into bal location by coping the values.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private IBAL.BO.Location locationTranslate(IDAL.DO.Location location)
        {
            return new IBAL.BO.Location() { Longitude = location.Longitude, Latitude = location.Longitude };
        }

        /// <summary>
        /// the function gets bl locatin and transforms it into idal location by coping the values.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private IDAL.DO.Location locationTranslate(IBAL.BO.Location location)
        {
            return new IDAL.DO.Location() { Longitude = location.Longitude, Lattitude = location.Longitude };
        }

       

        /// <summary>
        /// geting all the ditails from the class config to the BL.
        /// </summary>
        /// <returns></returns>
        public int GetNextSerialNumberForParcel()
        {
            return dalObject.GetSerialNumber();
        }

        
    }//END BL class

    
}//end IBAL
