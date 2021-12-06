﻿using System;
using System.Collections.Generic;
using IDAL.DO;
using IBAL.BO;
using System.Linq;

namespace IBL
{
    public class BL : IBL
    {
        private readonly IDAL.IDal dalObject = new DalObject.DalObject();

        // for the electricity use of a free drone
        private double Free { get; }

        // for the electricity use of a drone that carrys a light wight.
        private double Light { get; }

        // for the electricity use of a drone that carrys a middle wight.
        private double Middel { get; }

        // for the electricity use of a drone that carrys a heavy wight.
        private double Heavy { get; }

        //for the speed of the charge. precentage for hour.
        private double ChargingSpeed { get; }


        private readonly List<DroneForList> drones = new();

        private static readonly Random Random = new();


        /// <summary>
        /// constructor for the Bl class
        /// </summary>
        public BL()
        {
            double[] electricityUse = dalObject.ElectricityUse();

            Free = electricityUse[0];
            Light = electricityUse[1];
            Middel = electricityUse[2];
            Heavy = electricityUse[3];
            ChargingSpeed = electricityUse[4];

            //translates all the drones from the data level to 
            foreach (IDAL.DO.Drone dalDrone in dalObject.GetDrones(_ => true).ToList())
            {
                drones.Add(new DroneForList()
                {
                    Id = dalDrone.Id,
                    Model = dalDrone.Model,
                    Weight = (IBAL.BO.Enums.WeightCategories) dalDrone.MaxWeight,
                    Status = (Enums.DroneStatuses) (Random.Next() % 2 * 2)
                });
            }

            //dalObject.GetParcels(parcel => parcel.DroneId != -1 && parcel.AcceptedTime == null).ToList().ForEach(c => Console.WriteLine(c.Id));

            //going through all the parcels that have a drone, and was not delivered.
            foreach (IDAL.DO.Parcel parcel in dalObject.GetParcels(parcel =>
                parcel.DroneId != -1 && parcel.AcceptedTime == null))
            {
                //caculate the distance of the delivery.
                double deliveryDistance = 0;
                //caculating the distance between the sender of the parcel, and the reciver. 
                deliveryDistance += Distance(LocationTranslate(dalObject.GetCustomer(parcel.SenderId).Location),
                    LocationTranslate(dalObject.GetCustomer(parcel.TargetId).Location));
                //caculating the distance between the reciver of the parcel, and the clothest station to the reciver. 
                deliveryDistance += Distance(LocationTranslate(dalObject.GetCustomer(parcel.TargetId).Location),
                    LocationTranslate(dalObject.GetBaseStation(dalObject.GetClosestStation(parcel.TargetId))
                        .Location));

                double minimumValue;

                //updates the status of the drone to be delivery. 
                GetDrone(parcel.DroneId).Status = Enums.DroneStatuses.DELIVERY;
                //updates the parcel number to be delivery.
                GetDrone(parcel.DroneId).ParcelInDelivery = new ParcelInDelivery() { 
                    Id = parcel.Id,
                    Priority = GetParcel(parcel.Id).Priority,
                    Sender = GetParcel(parcel.Id).Sender,
                    Receiver = GetParcel(parcel.Id).Reciver,
                    DeliveringLocation = GetCustomer(GetParcel(parcel.Id).Reciver.Id).Location,
                    PickupLocation = GetCustomer(GetParcel(parcel.Id).Sender.Id).Location,
                    Status = IBAL.BO.Enums.ParcelStatus.ASSIGNED,
                    Weight = GetParcel(parcel.Id).Weight,
                    Distance = Distance(GetCustomer(GetParcel(parcel.Id).Reciver.Id).Location , GetCustomer(GetParcel(parcel.Id).Sender.Id).Location)
                };

                //if the parcel wasnt picked up.
                if (parcel.PickedUpTime == null)
                {
                    //seting the location of the drone to be in the clothest station to the sender.
                    GetDrone(parcel.DroneId).Location = LocationTranslate(dalObject
                        .GetBaseStation(dalObject.GetClosestStation(parcel.SenderId)).Location);

                    // caculating the distance between the base station to the sender.
                    deliveryDistance +=
                        Distance(
                            LocationTranslate(dalObject.GetBaseStation(dalObject.GetClosestStation(parcel.SenderId))
                                .Location), LocationTranslate(dalObject.GetCustomer(parcel.SenderId).Location));

                    // caculating the minimum precentage of batteary the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance /
                                   (int) (dalObject.ElectricityUse()[(int) GetDrone(parcel.DroneId).MaxWeight + 1]);
                }
                else // if the parcel was picked up.
                {
                    //seting the location of the drone to be the location of the sender.
                    GetDrone(parcel.DroneId).Location =
                        LocationTranslate(dalObject.GetCustomer(parcel.SenderId).Location);

                    // caculating the minimum precentage of batteary the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance /
                                   (int) (dalObject.ElectricityUse()[(int) GetDrone(parcel.DroneId).MaxWeight + 1]);
                }

                //if the precentage is ok, the value of the battry is being randomiseied between the minimum value to one handred.
                GetDrone(parcel.DroneId).Battery = minimumValue + Random.Next() % (100 - minimumValue);
            }

            //needed for the iteration.
            List<IDAL.DO.Parcel> parcels = dalObject.GetParcels(_ => true).ToList();

            //going through all the drones.
            foreach (DroneForList drone in drones)
            {
                // if the drone wasn't changed in the last iteration, meaning that the status is either FREE or MAINTENANCE.
                if (drone.Status == Enums.DroneStatuses.FREE)
                {
                    //getting random value from the list.
                    int index = Random.Next() % (parcels.Count);

                    //seting the drones location the drone to be in a target location of the randomisied parcel.
                    drone.Location = LocationTranslate(dalObject.GetCustomer(parcels[index].TargetId).Location);

                    //caculating the distance to the clothest station.
                    double deliveryDistance =
                        Distance(
                            LocationTranslate(dalObject
                                .GetBaseStation(dalObject.GetClosestStation(parcels[index].TargetId)).Location),
                            LocationTranslate(dalObject.GetCustomer(parcels[index].TargetId).Location));

                    //caculating the battary consamption.
                    double battayConcamption = deliveryDistance / dalObject.ElectricityUse()[(int) drone.Weight + 1];

                    //the there is not enough battary, exception will be thrown.
                    //if (battayConcamption > 100) throw new IBAL.BO.BL_ConstaractorException($"the drone needs {battayConcamption} battary in order to complete to delivery. ");

                    //seting the battry to be randomised between the minimum value to 100.
                    drone.Battery = (int) (battayConcamption + Random.NextDouble() * (100 - battayConcamption));
                }
                else if (drone.Status == Enums.DroneStatuses.MAINTENANCE)
                {
                    List<IDAL.DO.BaseStation> avalibleBaseStations =
                        dalObject.GetBaseStations(b => b.ChargeSlots > 0).ToList();

                    //random number of a station.
                    int stationNumber = Random.Next() % avalibleBaseStations.Count;

                    //setting the location of the drone to be the location of the randomaised station.
                    drone.Location = LocationTranslate(avalibleBaseStations[stationNumber].Location);

                    // updating the battary to be a random value from 0 to 20.
                    drone.Battery = Random.Next() % 20;

                    //sending the drone to charge.
                    dalObject.ChargeDrone(stationNumber, drone.Id);
                }
            }
        } //end BL ctor


        ////**** adding option ****////


        /// <summary>
        /// the function gets a logical base station, converting it to a dal basestation and adding it to the database. 
        /// </summary>
        /// <param name="newBaseStation">logical basestation</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddBaseStation(IBAL.BO.BaseStation newBaseStation)
        {
            IDAL.DO.BaseStation baseStation = new IDAL.DO.BaseStation()
            {
                Id = newBaseStation.Id,
                Name = newBaseStation.Name,
                Location = LocationTranslate(newBaseStation.Location),
                ChargeSlots = newBaseStation.ChargeSlots
            };

            try
            {
                return dalObject.AddBaseStation(baseStation);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdAlreadyExistsException(newBaseStation.Id, "base station", e);
            }
        }


        /// <summary>
        /// the function gets a logical drone, converting it to a dal drone and adding it to the database. 
        /// </summary>
        /// <param name="newDrone">logical drone</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddDrone(IBAL.BO.Drone newDrone)
        {
            if (drones.Exists(d => d.Id == newDrone.Id))
            {
                throw new IBAL.BO.IdAlreadyExistsException(newDrone.Id, "droen");
            }

            int parcelId;
            
            if (newDrone.ParcelInDelivery == null)
            {
                parcelId = -1;
            }
            else
            {
                parcelId = newDrone.ParcelInDelivery.Id;
            }

            if (newDrone.Location == null)
            {
                List<BaseStationForList> avalibaleBaseStations =
                    GetBaseStations(b => b.FreeChargingSlots > 0).ToList();
                if (avalibaleBaseStations.Count == 0)
                    throw new UnableToAddDroneException("No BaseStation Avalible");
                newDrone.Location = GetBaseStation(avalibaleBaseStations[Random.Next(avalibaleBaseStations.Count)].Id)
                    .Location;
            }

            DroneForList balDrone = new DroneForList()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                Weight = newDrone.MaxWeight,
                Battery = newDrone.Battery,
                Location = newDrone.Location,
                Status = newDrone.Status,
                ParcelId = parcelId
            };

            drones.Add(balDrone);

            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (WeightCategories) newDrone.MaxWeight
            };

            //since we checked in the list there is no chance to have the same number of drone in the dal list.   
            bool b = dalObject.AddDrone(dalDrone);

            ChargeDrone(newDrone.Id);

            return b;
        }


        /// <summary>
        /// the function gets a logical customer, converting it to a dal custamer and adding it to the database. 
        /// </summary>
        /// <param name="newCustomer">logical custumer</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddCustumer(IBAL.BO.Customer newCustomer)
        {
            IDAL.DO.Customer customer = new IDAL.DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Location = LocationTranslate(newCustomer.Location)
            };

            try
            {
                return dalObject.AddCustumer(customer);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdAlreadyExistsException(newCustomer.Id, "customer", e);
            }
        }


        /// <summary>
        /// the function gets a logical parcel, converting it to a dal parcel and adding it to the database. 
        /// </summary>
        /// <param name="newParcel">logical parcel</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddParcel(IBAL.BO.Parcel newParcel)
        {
            IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Reciver.Id,
                Weight = (WeightCategories) newParcel.Weight,
                Priority = (Priorities) newParcel.Priority,
                DroneId = -1,
                RequestedTime = newParcel.RequestedTime,
                DeliveryTime = newParcel.DeliveringTime,
                AcceptedTime = newParcel.AcceptedTime,
                PickedUpTime = newParcel.PickupTime
            };

            try
            {
                return dalObject.AddParcel(dalParcel);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdAlreadyExistsException(newParcel.Id, "parcel", e);
            }
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
            int index = drones.FindIndex(d => d.Id == droneId);

            if(index != -1)
                 drones[index].Model = model;

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
        /// <param name="basStationId"></param>
        /// <param name="name"></param>
        /// <param name="slots"></param>
        /// <returns> true if the updaue complited successfully </returns>
        public bool UpdateBaseStation(int basStationId, string name, int slots)
        {
            try
            {
                return dalObject.UpdateBaseStation(basStationId, name, slots);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdDontExistsException(basStationId, "baseStation", e);
            }
        }

        /// <summary>
        ///  calling the function from the dal that changesd the name and phone number of a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool UpdateCustomer(int customerId, string name, string phone)
        {
            try
            {
                return dalObject.UpdateCustomer(customerId, name, phone);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdDontExistsException(customerId, "customer", e);
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
            int droneIndex = drones.FindIndex(d => d.Id == droneId);

            //if the index is -1 it means that no such is id in the database so an exception will be thrown.
            if (droneIndex == -1)
                throw new UnableToAssignParcelToTheDroneException(droneId, " drone is not in the database.",
                    new IBAL.BO.IdDontExistsException(droneId,
                        "drone")); //IBAL.BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (drones[droneIndex].Status != Enums.DroneStatuses.FREE)
                throw new UnableToAssignParcelToTheDroneException(droneId, " the drone is not free");

            //importing all the parcels and sorting them aaccording to their praiority.
            List<IDAL.DO.Parcel> parcels = dalObject.GetParcels(p =>
                    ((int) p.Weight <= (int) drones[droneIndex].Weight)
                    && (Distance(drones[droneIndex].Location,
                            LocationTranslate(dalObject
                                .GetCustomer(p.SenderId)
                                .Location)) /*adding the distance between the reciver to the base station*/
                        >= drones[droneIndex].Battery *
                        dalObject.ElectricityUse()[
                            (int) drones[droneIndex].Weight + 1]))
                .OrderByDescending(p => (int) p.Priority)
                .ThenBy(p => Distance(drones[droneIndex].Location,
                    LocationTranslate(dalObject.GetCustomer(p.SenderId).Location))).ToList();

            //if no parcel left in the list after the removings it means that no parcel can be sent be thi drone, so exception will be thrown.
            if (parcels.Count == 0)
                throw new UnableToAssignParcelToTheDroneException(droneId,
                    " there is no parcel that can be sent by this drone due to: all the parcels are too heavy or too long distanses.");

            //if there is a parcel that matches the needings of the drone the rewwuaiered will happen.
            drones[droneIndex].Status = Enums.DroneStatuses.DELIVERY;

            drones[droneIndex].ParcelId = parcels.First().Id;

            //calling to the function from the dal to make the changes in the data level.
            try
            {
                return dalObject.PickingUpParcel(parcels.First().Id, droneId);
            }
            catch (Exception e)
            {
                throw new UnableToAssignParcelToTheDroneException(droneId, "parcel or drone is not exist", e);
            }
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
            if (GetDrone(droneId).Status != Enums.DroneStatuses.DELIVERY)
                throw new UnableToDeliverParcelFromTheDroneException(droneId,
                    "the drone is not delivering any parcel.");

            //caculating the distance of the delivery.
            double deliveryDistance = Distance(
                GetLocationOfCustomer(droneId, Enums.CustomerEnum.SENDER),
                GetLocationOfCustomer(droneId, Enums.CustomerEnum.TARGET));

            //battary update
            GetDrone(droneId).Battery -=
                deliveryDistance / dalObject.ElectricityUse()[(int) GetDrone(droneId).MaxWeight + 1];

            // location update
            GetDrone(droneId).Location = GetLocationOfCustomer(droneId, Enums.CustomerEnum.TARGET);

            //status update
            GetDrone(droneId).Status = Enums.DroneStatuses.FREE;

            //update the parcel from the dal.
            return dalObject.DeliveringParcel(GetDrone(droneId).ParcelInDelivery.Id);
        }

        private IBAL.BO.Location GetLocationOfCustomer(int droneId, Enums.CustomerEnum typeOfCustomer)
        {
            switch (typeOfCustomer)
            {
                case Enums.CustomerEnum.SENDER:
                    return LocationTranslate(dalObject
                        .GetCustomer(dalObject.GetParcel(GetDrone(droneId).ParcelInDelivery.Id).SenderId).Location);
                case Enums.CustomerEnum.TARGET:
                    return LocationTranslate(dalObject
                        .GetCustomer(dalObject.GetParcel(GetDrone(droneId).ParcelInDelivery.Id).TargetId).Location);
                default:
                    throw new Exception("type of cusomer is not CustomerEnum");
            }
        }

        /// <summary>
        /// the function finds the base station and sending the drone to charge there by the dal function.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool ChargeDrone(int droneId)
        {
            int droneIndex = drones.FindIndex(d => d.Id == droneId);

            if (droneIndex == -1)
                throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (drones[droneIndex].Status != Enums.DroneStatuses.FREE)
                throw new UnAbleToSendDroneToChargeException(" the drone is not free");

            //geting the maximum distance the drone can make according to the level of battary multiplied by the number of kilometers the drone can do for one precent do battary. 
            double maxDistance = drones[droneIndex].Battery *
                                 dalObject.ElectricityUse()[(int) drones[droneIndex].Weight + 1];

            //returns all the stations that the drone has enough fuel to get to, ordered by the closest base station.
            List<IDAL.DO.BaseStation> baseStations = dalObject.GetBaseStations(b =>
                    (Distance(LocationTranslate(b.Location), GetDrone(droneId).Location) <= maxDistance) &&
                    (b.ChargeSlots > 0))
                .OrderBy(b => Distance(LocationTranslate(b.Location), GetDrone(droneId).Location)).ToList();

            //if there is no suitable station an exception will be thrown.
            if (baseStations.Count == 0)
                throw new UnAbleToSendDroneToChargeException(
                    " there is no station that matches the needs of this drone");

            IDAL.DO.BaseStation baseStation = baseStations.First();

            /////drone updates//////

            //update the battary status.
            drones[droneIndex].Battery -=
                Distance(LocationTranslate(baseStation.Location), GetDrone(droneId).Location) /
                dalObject.ElectricityUse()[(int) drones[droneIndex].Weight + 1];

            //update the localtion of the drone to be the locatin of the base station.
            drones[droneIndex].Location = LocationTranslate(baseStation.Location);

            //updating the status of the drone.
            drones[droneIndex].Status = Enums.DroneStatuses.MAINTENANCE;

            //update the number of the charge slots and adding an element in the dal functions.
            return dalObject.ChargeDrone(baseStation.Id, droneId);
        }

        /// <summary>
        /// the function uncharging a drone from its base station.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public bool UnChargeDrone(int droneId, int minutes)
        {
            int droneIndex = drones.FindIndex(d => d.Id == droneId);
            if (droneIndex == -1) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (drones[droneIndex].Status != Enums.DroneStatuses.MAINTENANCE)
                throw new UnAbleToReleaseDroneFromChargeException(droneId, "the drone is not in maintanance");

            //update the battary status.
            drones[droneIndex].Battery += minutes * dalObject.ElectricityUse()[4];
            if (drones[droneIndex].Battery > 100)
            {
                drones[droneIndex].Battery = 100;
            }

            //updating the status of the drone.
            drones[droneIndex].Status = Enums.DroneStatuses.FREE;

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
            IDAL.DO.BaseStation dalBaseStation;
            try
            {
                dalBaseStation = dalObject.GetBaseStation(baseStationId);
            }
            catch (Exception e)
            {
                throw new IBAL.BO.IdDontExistsException(baseStationId, "base station", e);
            }

            List<DroneInCharge> charges =
                ChargeDroneToDroneInCharge(dalObject.GetChargeDrones(d => d.StationId == dalBaseStation.Id).ToList());

            return new IBAL.BO.BaseStation()
            {
                Id = dalBaseStation.Id,
                Name = dalBaseStation.Name,
                ChargeSlots = dalBaseStation.ChargeSlots,
                Location = LocationTranslate(dalBaseStation.Location),
                ChargingDrones = charges
            };
        }

        /// <summary>
        /// returns a list with all the drone charges converted to drons in charge
        /// </summary>
        /// <param name="droneCharges"></param>
        /// <returns></returns>
        private List<DroneInCharge> ChargeDroneToDroneInCharge(List<DroneCharge> droneCharges)
        {
            return droneCharges.ConvertAll(dc => new DroneInCharge()
            {
                Id = dc.DroneId,
                Battery = GetDrone(dc.DroneId).Battery
            });
        }

        /// <summary>
        /// returns a drone according to its id number.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public IBAL.BO.Drone GetDrone(int droneId)
        {
            int index = drones.FindIndex(d => d.Id == droneId);

            if (index == -1)
                throw new IBAL.BO.IdDontExistsException(droneId, "drone",
                    new IDAL.DO.IdDontExistsException(droneId, "drone"));

            IBAL.BO.ParcelInDelivery parcelInDelivery;

            if (drones[index].ParcelId != -1)
            {
                IBAL.BO.Parcel parcel = GetParcel(drones[index].ParcelId);

                 parcelInDelivery = new ParcelInDelivery()
                {
                    Id = parcel.Id,
                    Priority = GetParcel(parcel.Id).Priority,
                    Sender = GetParcel(parcel.Id).Sender,
                    Receiver = GetParcel(parcel.Id).Reciver,
                    DeliveringLocation = GetCustomer(GetParcel(parcel.Id).Reciver.Id).Location,
                    PickupLocation = GetCustomer(GetParcel(parcel.Id).Sender.Id).Location,
                    Status = IBAL.BO.Enums.ParcelStatus.ASSIGNED,
                    Weight = GetParcel(parcel.Id).Weight,
                    Distance = Distance(GetCustomer(GetParcel(parcel.Id).Reciver.Id).Location, GetCustomer(GetParcel(parcel.Id).Sender.Id).Location)
                };
            }
            else
            {
                parcelInDelivery = null;
            }

            return new IBAL.BO.Drone()
            {
                Id = drones[index].Id,
                Model = drones[index].Model,
                Location = drones[index].Location,
                Status = drones[index].Status,
                Battery = drones[index].Battery,
                MaxWeight = drones[index].Weight,
                ParcelInDelivery = parcelInDelivery
            };
        }

        /// <summary>
        /// returns a customer according to its id number.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IBAL.BO.Customer GetCustomer(int customerId)
        {
            IDAL.DO.Customer dalCustomer;

            try
            {
                dalCustomer = dalObject.GetCustomer(customerId);
            }
            catch (IDAL.DO.IdDontExistsException e)
            {
                throw new IBAL.BO.IdDontExistsException(customerId, "customer", e);
            }

            return new IBAL.BO.Customer()
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = LocationTranslate(dalCustomer.Location)
            };
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
                CoustomerForParcel sender = new CoustomerForParcel()
                {
                    Id = dalParcel.SenderId,
                    CustomerName = dalObject.GetCustomer(dalParcel.SenderId).Name
                };

                CoustomerForParcel reciver = new CoustomerForParcel()
                {
                    Id = dalParcel.TargetId,
                    CustomerName = dalObject.GetCustomer(dalParcel.TargetId).Name
                };

                DroneForParcel drone = new DroneForParcel()
                {
                    Id = dalParcel.DroneId
                };

                return new IBAL.BO.Parcel()
                {
                    Id = dalParcel.Id,
                    Sender = sender,
                    Reciver = reciver,
                    Drone = drone,
                    Priority = (IBAL.BO.Enums.Priorities) dalParcel.Priority,
                    Weight = (IBAL.BO.Enums.WeightCategories) dalParcel.Weight,
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
        public IEnumerable<BaseStationForList> GetBaseStations(Predicate<BaseStationForList> f)
        {
            return dalObject.GetBaseStations(_ => true)
                .Select(db =>
                    new BaseStationForList()
                    {
                        Id = db.Id,
                        Name = db.Name,
                        FreeChargingSlots = db.ChargeSlots,
                        TakenCharingSlots = dalObject.GetChargeDrones(cd => cd.StationId == db.Id).Count()
                    })
                .Where(bs => f(bs));
        }

        /// <summary>
        ///  returns the list of drones that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<DroneForList> GetDrones(Predicate<DroneForList> f)
        {
            List<DroneForList> balDrones = new List<DroneForList>();

            drones.FindAll(d => f(d)).ForEach(d => balDrones.Add(new DroneForList()
            {
                Id = d.Id,
                Model = d.Model,
                Battery = d.Battery,
                Location = d.Location,
                Weight = d.Weight,
                Status = d.Status,
                ParcelId = d.ParcelId
            }));

            return balDrones; //return dalObject.GetDrones(f);   //this.drones.FindAll(f);
        }

        /// <summary>
        ///  returns the list of customers that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<CustomerForList> GetCustomers(Predicate<CustomerForList> f)
        {
            return dalObject.GetCustomers(_ => true)
                .Select(c =>
                    new CustomerForList()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Phone = c.Phone,
                        //caculating the number of parcels that were sent to this customer that were sent and deliverd. 
                        SentToAndDeliverd = dalObject.GetParcels(p =>
                            p.TargetId == c.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                        //caculating the number of parcels that were sent to this customer that were sent and not deliverd. 
                        SentToAnDNotDelivered = dalObject.GetParcels(p =>
                            p.TargetId == c.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                        //caculating the number of parcels that were sent from this customer that were sent and deliverd. 
                        SentFromAndDeliverd = dalObject.GetParcels(p =>
                            p.SenderId == c.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                        //caculating the number of parcels that were sent from this customer that were sent and not deliverd. 
                        SentFromAndNotDeliverd = dalObject.GetParcels(p =>
                            p.SenderId == c.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                    })
                .Where(c => f(c));
        }

        /// <summary>
        ///  returns the list of parcels that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<ParcelForList> GetPacels(Predicate<ParcelForList> f)
        {
            return dalObject.GetParcels(_ => true)
                .Select(dalParcel =>
                    new ParcelForList()
                    {
                        Id = dalParcel.Id,
                        Priority = (IBAL.BO.Enums.Priorities) dalParcel.Priority,
                        SenderName = dalObject.GetCustomer(dalParcel.SenderId).Name,
                        ReceiverName = dalObject.GetCustomer(dalParcel.TargetId).Name,
                        Weight = (Enums.WeightCategories) dalParcel.Weight
                    })
                .Where(dp => f(dp));
        }


        //operation functions.

        /// <summary>
        /// function that gets
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private static double Distance(IBAL.BO.Location l1, IBAL.BO.Location l2)
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
        private static IBAL.BO.Location LocationTranslate(IDAL.DO.Location location)
        {
            return new IBAL.BO.Location() {Longitude = location.Longitude, Latitude = location.Longitude};
        }

        /// <summary>
        /// the function gets bl locatin and transforms it into idal location by coping the values.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static IDAL.DO.Location LocationTranslate(IBAL.BO.Location location)
        {
            return new IDAL.DO.Location() {Longitude = location.Longitude, Lattitude = location.Longitude};
        }


        /// <summary>
        /// geting all the ditails from the class config to the BL.
        /// </summary>
        /// <returns></returns>
        public int GetNextSerialNumberForParcel()
        {
            return dalObject.GetSerialNumber();
        }
    } //END BL class
} //end IBAL