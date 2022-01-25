using System;
using System.Collections.Generic;
using DO;
using BO;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BlApi
{
    internal sealed class BL : IBL
    {
        #region private fields

        private readonly DalApi.IDal dal = DalApi.DalFactory.GetDal();

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

        /// <summary>
        /// the list of the drones that are being held by the BL.
        /// </summary>
        private readonly List<DroneForList> drones = new();

        /// <summary>
        /// random values
        /// </summary>
        private static readonly Random Random = new();

        #endregion

        #region Singalton


        /// <summary>
        /// private constructor for the Bl class, for the singalton.
        /// </summary>
        private BL()
        {
            double[] electricityUse = dal.ElectricityUse();

            Free = electricityUse[0];
            Light = electricityUse[1];
            Middel = electricityUse[2];
            Heavy = electricityUse[3];
            ChargingSpeed = electricityUse[4];

            var droneCharges = dal.GetChargeDrones(_ => true);

            //translates all the drones from the data level to 
            foreach (DO.Drone dalDrone in dal.GetDrones(_ => true))
            {
                drones.Add(new DroneForList()
                {
                    Id = dalDrone.Id,
                    Model = dalDrone.Model,
                    Weight = (BO.Enums.WeightCategories)dalDrone.MaxWeight,
                    Status = droneCharges.Any(dc => dc.DroneId == dalDrone.Id) ? Enums.DroneStatuses.MAINTENANCE : Enums.DroneStatuses.FREE
                });
            }

            //takes all the parcels that are assigned to a drone and was not delivered.
            var parcelsForUpdate = dal.GetParcels(parcel => parcel.DroneId != 0 && parcel.DeliveryTime == null);

            //going through all the parcels that have a drone, and was not assigned to it.
            foreach (DO.Parcel parcel in parcelsForUpdate)
            {

                //caculate the distance of the delivery.
                double deliveryDistance = 0;
                //caculating the distance between the sender of the parcel, and the reciver. 
                deliveryDistance += Distance(LocationTranslate(dal.GetCustomer(parcel.SenderId).Location),
                                             LocationTranslate(dal.GetCustomer(parcel.TargetId).Location));
                //caculating the distance between the reciver of the parcel, and the clothest station to the reciver. 
                deliveryDistance += Distance(LocationTranslate(dal.GetCustomer(parcel.TargetId).Location),
                                             LocationTranslate(dal.GetBaseStation(dal.GetClosestStation(parcel.TargetId)).Location));

                double minimumValue;

                //finding the index of the drone that.
                int index = drones.FindIndex(d => d.Id == parcel.DroneId);

                //updates the status of the drone to be delivery. 
                drones[index].Status = Enums.DroneStatuses.DELIVERY;
                //updates the parcel number to be the delivering parcel.
                drones[index].ParcelId = parcel.Id;

                //if the parcel wasnt picked up.
                if (parcel.PickedUpTime == null)
                {
                    //seting the location of the drone to be in the clothest station to the sender.
                    drones[index].Location = LocationTranslate(dal
                        .GetBaseStation(dal.GetClosestStation(parcel.SenderId)).Location);

                    // caculating the distance between the base station to the sender.
                    deliveryDistance +=
                        Distance(LocationTranslate(dal.GetBaseStation(dal.GetClosestStation(parcel.SenderId)).Location),
                                 LocationTranslate(dal.GetCustomer(parcel.SenderId).Location));

                    // caculating the minimum precentage of batteary the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance / (int)(dal.ElectricityUse()[(int)GetDrone(parcel.DroneId).MaxWeight + 1]);
                }
                else // if the parcel was picked up.
                {
                    //seting the location of the drone to be the location of the sender.
                    drones[index].Location =
                        LocationTranslate(dal.GetCustomer(parcel.SenderId).Location);

                    // caculating the minimum precentage of batteary the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance / (int)(dal.ElectricityUse()[(int)GetDrone(parcel.DroneId).MaxWeight + 1]);
                }

                //if the precentage is ok, the value of the battry is being randomiseied between the minimum value to one handred.
                drones[index].Battery = minimumValue + Random.Next() % (100 - minimumValue);
            }

            //needed for the iteration.
            IEnumerable<DO.Parcel> parcels = dal.GetParcels(_ => true);

            //going through all the drones.
            foreach (DroneForList drone in drones)
            {
                // if the drone wasn't changed in the last iteration, meaning that the status is either FREE or MAINTENANCE.
                if (drone.Status == Enums.DroneStatuses.FREE)
                {
                    //getting random value from the list.
                    int index = Random.Next() % (parcels.Count());

                    //seting the drones location the drone to be in a target location of the randomisied parcel.
                    drone.Location = LocationTranslate(dal.GetCustomer(parcels.ElementAt(index).TargetId).Location);

                    //caculating the distance to the clothest station.
                    double deliveryDistance =
                        Distance(
                            LocationTranslate(dal
                                .GetBaseStation(dal.GetClosestStation(parcels.ElementAt(index).TargetId)).Location),
                            LocationTranslate(dal.GetCustomer(parcels.ElementAt(index).TargetId).Location));

                    //caculating the battary consamption.
                    double battayConcamption = deliveryDistance / dal.ElectricityUse()[(int)drone.Weight + 1];

                    //seting the battry to be randomised between the minimum value to 100.
                    drone.Battery = (int)(battayConcamption + Random.NextDouble() * (100 - battayConcamption));
                }
                else if (drone.Status == Enums.DroneStatuses.MAINTENANCE)
                {
                    IEnumerable<DO.BaseStation> avalibleBaseStations = dal.GetBaseStations(b => b.ChargeSlots > 0);

                    //random number of a station.
                    int stationIndex = Random.Next() % avalibleBaseStations.Count();

                    //setting the location of the drone to be the location of the randomaised station.
                    drone.Location = LocationTranslate(avalibleBaseStations.ElementAt(stationIndex).Location);

                    // updating the battary to be a random value from 0 to 20.
                    drone.Battery = Random.Next() % 20;

                    if (!dal.GetChargeDrones(_ => true).Any(dc => dc.DroneId == drone.Id))
                    {
                        //sending the drone to charge.
                        dal.ChargeDrone(avalibleBaseStations.ElementAt(stationIndex).Id, drone.Id);
                    }
                }
            }
        } //end BL ctor

        /// <summary>
        /// bl field intended to keep the insstance of the bl that was created.
        /// </summary>
        private static readonly Lazy<IBL> instance = new Lazy<IBL>(() => new BL());

        /// <summary>
        /// the function the creates new instance of bl only if it doesn't exists already.
        /// </summary>
        /// <returns></returns>
        public static IBL GetInstance()
        {
            return instance.Value;
        }

        #endregion

        #region adding option 

        /// <summary>
        /// the function gets a logical base station, converting it to a dal basestation and adding it to the database. 
        /// </summary>
        /// <param name="newBaseStation">logical basestation</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddBaseStation(BO.BaseStation newBaseStation)
        {
            if (newBaseStation.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            DO.BaseStation baseStation = new DO.BaseStation()
            {
                Id = newBaseStation.Id,
                Name = newBaseStation.Name,
                Location = LocationTranslate(newBaseStation.Location),
                ChargeSlots = newBaseStation.ChargeSlots
            };

            try
            {
                return dal.AddBaseStation(baseStation);
            }
            catch (Exception e)
            {
                throw new BO.IdAlreadyExistsException(newBaseStation.Id, "base station", e);
            }
        }

        /// <summary>
        /// the function gets a logical drone, converting it to a dal drone and adding it to the database. 
        /// </summary>
        /// <param name="newDrone">logical drone</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddDrone(BO.Drone newDrone)
        {

            if (newDrone.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            //check if the drone exists int he dronne list in the bl.
            if (drones.Exists(d => d.Id == newDrone.Id))
            {
                throw new BO.IdAlreadyExistsException(newDrone.Id, "droen");
            }

            //assinging the drone to a randomal avalible station

            //gets all the avalible stations(stations with free slots).
            IEnumerable<BaseStationForList> avalibaleBaseStations = GetBaseStations(b => b.FreeChargingSlots > 0);
            if (avalibaleBaseStations.Count() == 0)
                throw new UnableToAddDroneException("No BaseStation Avalible");
            //rands one of the stations and puts the drone in there.
            int randomalStationIndex = Random.Next(avalibaleBaseStations.Count());
            newDrone.Location = GetBaseStation(avalibaleBaseStations.ElementAt(randomalStationIndex).Id).Location;
           

            //adding the drone to the database of drones in the bl.
            DroneForList balDrone = new DroneForList()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                Weight = newDrone.MaxWeight,
                Battery = newDrone.Battery,
                Location = newDrone.Location,
                Status = newDrone.Status,
                ParcelId = 0 // because every new drone is being added without a parcel.
            };
            this.drones.Add(balDrone);


            //calling the dal function of adding drone.
            DO.Drone dalDrone = new DO.Drone()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (WeightCategories)newDrone.MaxWeight
            };

            //no need for try, since we checked in the list there is no chance to have the same number of drone in the dal list.   
            bool b = dal.AddDrone(dalDrone);

            dal.ChargeDrone(avalibaleBaseStations.ElementAt(randomalStationIndex).Id, newDrone.Id);

            return b;
        }

        /// <summary>
        /// the function gets a logical customer, converting it to a dal custamer and adding it to the database. 
        /// </summary>
        /// <param name="newCustomer">logical custumer</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddCustumer(BO.Customer newCustomer)
        {
            if (newCustomer.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            DO.Customer customer = new DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Location = LocationTranslate(newCustomer.Location)
            };

            try
            {
                return dal.AddCustumer(customer);
            }
            catch (Exception e)
            {
                throw new BO.IdAlreadyExistsException(newCustomer.Id, "customer", e);
            }
        }

        /// <summary>
        /// the function gets a logical parcel, converting it to a dal parcel and adding it to the database. 
        /// </summary>
        /// <param name="newParcel">logical parcel</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddParcel(BO.Parcel newParcel)
        {

            if (newParcel.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            DO.Parcel dalParcel = new DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Reciver.Id,
                Weight = (WeightCategories)newParcel.Weight,
                Priority = (Priorities)newParcel.Priority,
                DroneId = 0,
                DefinededTime = newParcel.DefinedTime,
                DeliveryTime = newParcel.DeliveringTime,
                AssigndedTime = newParcel.AssigedTime,
                PickedUpTime = newParcel.PickupTime
            };

            try
            {
                return dal.AddParcel(dalParcel);
            }
            catch (Exception e)
            {
                throw new BO.IdAlreadyExistsException(newParcel.Id, "parcel", e);
            }
        }

        #endregion

        #region update options 

        /// <summary>
        /// calling the function from the dal that changesd the name of the drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        /// <returns> true if the updaue complited successfully </returns>
        public bool UpdateNameForADrone(int droneId, string model)
        {

            if (model == "")
                throw new ModelEmptyException();

            int index = drones.FindIndex(d => d.Id == droneId);

            if (index != -1)
                drones[index].Model = model;

            try
            {
                return dal.UpdateModelForADrone(droneId, model);
            }
            catch (Exception e)
            {
                throw new BO.IdDontExistsException(droneId, "drone", e);
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
                return dal.UpdateBaseStation(basStationId, name, slots);
            }
            catch (Exception e)
            {
                throw new BO.IdDontExistsException(basStationId, "baseStation", e);
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
                return dal.UpdateCustomer(customerId, name, phone);
            }
            catch (Exception e)
            {
                throw new BO.IdDontExistsException(customerId, "customer", e);
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
                    new BO.IdDontExistsException(droneId, "drone"));

            //chacks if the drone is free, and if not exception will be thrown.
            if (drones[droneIndex].Status != Enums.DroneStatuses.FREE)
                throw new UnableToAssignParcelToTheDroneException(droneId, " the drone is not free");

            //importing all the parcels and sorting them aaccording to their praiority.
            IEnumerable<DO.Parcel> parcels = dal.GetParcels(p =>
                ((int)p.Weight <= (int)drones[droneIndex].Weight) &&
                (
                   Distance(drones[droneIndex].Location, LocationTranslate(dal.GetCustomer(p.SenderId).Location)) +
                   Distance(LocationTranslate(dal.GetCustomer(p.SenderId).Location), LocationTranslate(dal.GetCustomer(p.TargetId).Location)) +
                   Distance(LocationTranslate(dal.GetCustomer(p.TargetId).Location), GetBaseStation(dal.GetClosestStation(p.TargetId)).Location)
                )
                <= drones[droneIndex].Battery * dal.ElectricityUse()[(int)drones[droneIndex].Weight + 1] &&

                 p.DeliveryTime == null)

                .OrderByDescending(p => (int)p.Priority)

                .ThenBy(p => Distance(drones[droneIndex].Location, LocationTranslate(dal.GetCustomer(p.SenderId).Location)));

            //if no parcel left in the list after the removings it means that no parcel can be sent be thi drone, so exception will be thrown.
            if (parcels.Count() == 0)
                throw new UnableToAssignParcelToTheDroneException(droneId,
                    " No parcel can be sent by this drone due to one of the folloing resons:\n" +
                    " 1) all the parcels are too heavy. \n" +
                    " 2) too long distanses.\n" +
                    " 3) there is no parcel that is waiting for delivery");

            //if there is a parcel that matches the needings of the drone the rewwuaiered will happen.
            drones[droneIndex].Status = Enums.DroneStatuses.DELIVERY;

            drones[droneIndex].ParcelId = parcels.First().Id;

            //update paclel times.
            dal.AssignDroneToParcel(parcels.First().Id, droneId);

            return true;
        }

        public bool PickingUpParcelToDrone(int droneId)
        {
            //if the drone is not in thr database, an exception will be thrown.
            if (GetDrone(droneId).Status != Enums.DroneStatuses.DELIVERY)
                throw new UnableToDeliverParcelFromTheDroneException(droneId,
                    "the drone is not delivering any parcel.");

            //caculating the distance of the delivery.
            double distance = Distance(
                GetDrone(droneId).Location,
                GetLocationOfCustomer(droneId, Enums.CustomerEnum.SENDER));

            int index = drones.FindIndex(d => d.Id == droneId);

            //battary update
            drones[index].Battery -=
                distance / dal.ElectricityUse()[(int)GetDrone(droneId).MaxWeight + 1];

            // location update
            drones[index].Location = GetLocationOfCustomer(droneId, Enums.CustomerEnum.SENDER);

            //update the parcel from the dal.

            try
            {
                return dal.PickingUpParcel(GetDrone(droneId).ParcelInDelivery.Id, droneId);
            }
            catch (Exception e)
            {
                throw new UnableToDeliverParcelFromTheDroneException(droneId, "parcel or drone is not exist", e);
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

            int index = drones.FindIndex(d => d.Id == droneId);

            //battary update
            drones[index].Battery -=
                deliveryDistance / dal.ElectricityUse()[(int)GetDrone(droneId).MaxWeight + 1];

            // location update
            drones[index].Location = GetLocationOfCustomer(droneId, Enums.CustomerEnum.TARGET);

            //status update
            drones[index].Status = Enums.DroneStatuses.FREE;

            int saveParcelId = drones[index].ParcelId;

            //parcel id update.
            drones[index].ParcelId = 0;

            //update the parcel from the dal.

            try
            {
                //return dalObject.DeliveringParcel(GetDrone(droneId).ParcelInDelivery.Id);
                return dal.DeliveringParcel(saveParcelId);
            }
            catch (Exception e)
            {
                throw new UnableToDeliverParcelFromTheDroneException(droneId, "parcel or drone is not exist", e);
            }
        }

        private BO.Location GetLocationOfCustomer(int droneId, Enums.CustomerEnum typeOfCustomer)
        {
            switch (typeOfCustomer)
            {
                case Enums.CustomerEnum.SENDER:
                    return LocationTranslate(dal
                        .GetCustomer(dal.GetParcel(GetDrone(droneId).ParcelInDelivery.Id).SenderId).Location);
                case Enums.CustomerEnum.TARGET:
                    return LocationTranslate(dal
                        .GetCustomer(dal.GetParcel(GetDrone(droneId).ParcelInDelivery.Id).TargetId).Location);
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
                throw new BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (drones[droneIndex].Status != Enums.DroneStatuses.FREE)
                throw new UnAbleToSendDroneToChargeException(" the drone is not free");

            //geting the maximum distance the drone can make according to the level of battary multiplied by the number of kilometers the drone can do for one precent do battary. 
            double maxDistance = drones[droneIndex].Battery *
                                 dal.ElectricityUse()[(int)drones[droneIndex].Weight + 1];

            //returns all the stations that the drone has enough fuel to get to, ordered by the closest base station.
            IEnumerable<DO.BaseStation> baseStations = dal.GetBaseStations(b =>
                    (Distance(LocationTranslate(b.Location), GetDrone(droneId).Location) <= maxDistance) &&
                    (b.ChargeSlots > 0))
                .OrderBy(b => Distance(LocationTranslate(b.Location), GetDrone(droneId).Location));

            //if there is no suitable station an exception will be thrown.
            if (baseStations.Count() == 0)
                throw new UnAbleToSendDroneToChargeException(
                    " there is no station that matches the needs of this drone");

            DO.BaseStation baseStation = baseStations.First();

            /////drone updates//////

            //update the battary status.
            drones[droneIndex].Battery -=
                Distance(LocationTranslate(baseStation.Location), GetDrone(droneId).Location) /
                dal.ElectricityUse()[(int)drones[droneIndex].Weight + 1];

            //update the localtion of the drone to be the locatin of the base station.
            drones[droneIndex].Location = LocationTranslate(baseStation.Location);

            //updating the status of the drone.
            drones[droneIndex].Status = Enums.DroneStatuses.MAINTENANCE;

            //update the number of the charge slots and adding an element in the dal functions.
            return dal.ChargeDrone(baseStation.Id, droneId);
        }

        /// <summary>
        /// the function uncharging a drone from its base station.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public bool UnChargeDrone(int droneId)
        {
            int droneIndex = drones.FindIndex(d => d.Id == droneId);
            if (droneIndex == -1)
                throw new BO.IdDontExistsException(droneId, "drone");

            //chacks if the drone is free, and if not exception will be thrown.
            if (drones[droneIndex].Status != Enums.DroneStatuses.MAINTENANCE)
                throw new UnAbleToReleaseDroneFromChargeException(droneId, "the drone is not in maintanance");

            //returning from the dal the value of the time that the drone was in charge
            double minutes = dal.UnChargeDrone(droneId);

            //update the battary status.
            drones[droneIndex].Battery += minutes * dal.ElectricityUse()[4];
            if (drones[droneIndex].Battery > 100)
            {
                drones[droneIndex].Battery = 100;
            }

            //updating the status of the drone.
            drones[droneIndex].Status = Enums.DroneStatuses.FREE;

            return true;
        }

        #endregion

        #region show options 

        /// <summary>
        /// returns a base sattion according to its id number.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public BO.BaseStation GetBaseStation(int baseStationId)
        {
            DO.BaseStation dalBaseStation;
            try
            {
                dalBaseStation = dal.GetBaseStation(baseStationId);
            }
            catch (Exception e)
            {
                throw new BO.IdDontExistsException(baseStationId, "base station", e);
            }

            IEnumerable<DroneInCharge> charges = ChargeDroneToDroneInCharge(dal.GetChargeDrones(d => d.StationId == dalBaseStation.Id));

            return new BO.BaseStation()
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
        private IEnumerable<DroneInCharge> ChargeDroneToDroneInCharge(IEnumerable<DroneCharge> droneCharges)
        {
            return droneCharges.Select(dc => new DroneInCharge()
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
        public BO.Drone GetDrone(int droneId)
        {
            int index = drones.FindIndex(d => d.Id == droneId);

            if (index == -1)
                throw new BO.IdDontExistsException(droneId, "drone",
                    new DO.IdDontExistsException(droneId, "drone"));

            BO.ParcelInDelivery parcelInDelivery;

            if (drones[index].ParcelId != 0)
            {
                BO.Parcel parcel = GetParcel(drones[index].ParcelId);
                BO.Customer sender = GetCustomer(parcel.Sender.Id);
                BO.Customer reciver = GetCustomer(parcel.Reciver.Id);

                parcelInDelivery = new ParcelInDelivery()
                {
                    Id = parcel.Id,
                    Priority = parcel.Priority,//  GetParcel(parcel.Id).Priority,
                    Sender = parcel.Sender,// GetParcel(parcel.Id).Sender,
                    Receiver = parcel.Reciver, // GetParcel(parcel.Id).Reciver,
                    DeliveringLocation = sender.Location,
                    PickupLocation = reciver.Location,
                    Status = Enums.ParcelStatus.ASSIGNED,
                    Weight = parcel.Weight, // GetParcel(parcel.Id).Weight,
                    Distance = Distance(sender.Location, reciver.Location)
                };
            }
            else
            {
                parcelInDelivery = null;
            }

            return new BO.Drone()
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
        public BO.Customer GetCustomer(int customerId)
        {
            DO.Customer dalCustomer;

            try
            {
                dalCustomer = dal.GetCustomer(customerId);
            }
            catch (DO.IdDontExistsException e)
            {
                throw new BO.IdDontExistsException(customerId, "customer", e);
            }

            return new BO.Customer()
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = LocationTranslate(dalCustomer.Location),
                FromCustomer = ParcelToParcelByCustumerList(GetPacels(p => p.SenderName == dalCustomer.Name)).ToList(),
                ToCustomer = ParcelToParcelByCustumerList(GetPacels(p => p.ReceiverName == dalCustomer.Name)).ToList()

            };
        }

        /// <summary>
        /// returns a parcel according to its id number.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public BO.Parcel GetParcel(int parcelId)
        {
            try
            {
                DO.Parcel dalParcel = dal.GetParcel(parcelId);

                return new BO.Parcel()
                {
                    Id = dalParcel.Id,

                    Sender = (from cust in dal.GetCustomers(_ => true)
                              where cust.Id == dalParcel.TargetId
                              select new CoustomerForParcel
                              {
                                  Id = cust.Id,
                                  CustomerName = cust.Name
                              }).FirstOrDefault(),

                    Reciver = (from cust in dal.GetCustomers(_ => true)
                               where cust.Id == dalParcel.TargetId
                               select new CoustomerForParcel
                               {
                                   Id = cust.Id,
                                   CustomerName = cust.Name
                               }).FirstOrDefault(),

                    Drone = new DroneForParcel()
                    {
                        Id = dalParcel.DroneId
                    },

                    Priority = (BO.Enums.Priorities)dalParcel.Priority,
                    Weight = (BO.Enums.WeightCategories)dalParcel.Weight,
                    AssigedTime = dalParcel.AssigndedTime,
                    DefinedTime = dalParcel.DefinededTime,
                    DeliveringTime = dalParcel.DeliveryTime,
                    PickupTime = dalParcel.PickedUpTime
                };
            }
            catch (DO.IdDontExistsException e)
            {
                throw new BO.IdDontExistsException(parcelId, "parcel", e);
            }
        }


        /// <summary>
        /// returns the list of base stations that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<BaseStationForList> GetBaseStations(Predicate<BaseStationForList> f)
        {
            return new ObservableCollection<BaseStationForList>(dal.GetBaseStations(_ => true)
                .Select(db =>
                    new BaseStationForList()
                    {
                        Id = db.Id,
                        Name = db.Name,
                        FreeChargingSlots = db.ChargeSlots,
                        TakenCharingSlots = dal.GetChargeDrones(cd => cd.StationId == db.Id).Count()
                    })
                .Where(bs => f(bs)));
        }

        /// <summary>
        ///  returns the list of drones that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<DroneForList> GetDrones(Predicate<DroneForList> f)
        {
            ObservableCollection<DroneForList> balDrones = new ObservableCollection<DroneForList>();

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

            return balDrones;
        }

        /// <summary>
        ///  returns the list of customers that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<CustomerForList> GetCustomers(Predicate<CustomerForList> f)
        {
            return new ObservableCollection<CustomerForList>(dal.GetCustomers(_ => true)
                .Select(c =>
                    new CustomerForList()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Phone = c.Phone,
                        //caculating the number of parcels that were sent to this customer that were sent and deliverd. 
                        SentToAndDeliverd = dal.GetParcels(p =>
                            p.TargetId == c.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                        //caculating the number of parcels that were sent to this customer that were sent and not deliverd. 
                        SentToAnDNotDelivered = dal.GetParcels(p =>
                            p.TargetId == c.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                        //caculating the number of parcels that were sent from this customer that were sent and deliverd. 
                        SentFromAndDeliverd = dal.GetParcels(p =>
                            p.SenderId == c.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                        //caculating the number of parcels that were sent from this customer that were sent and not deliverd. 
                        SentFromAndNotDeliverd = dal.GetParcels(p =>
                            p.SenderId == c.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                    })
                .Where(c => f(c)));
        }

        /// <summary>
        ///  returns the list of parcels that return true for the prediacte f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<ParcelForList> GetPacels(Predicate<ParcelForList> f)
        {
            //return new ObservableCollection<ParcelForList>(dal.GetParcels(_ => true)
            //    .Select(dalParcel =>
            //        new ParcelForList()
            //        {
            //            Id = dalParcel.Id,
            //            Priority = (BO.Enums.Priorities) dalParcel.Priority,
            //            SenderName = dal.GetCustomer(dalParcel.SenderId).Name,
            //            ReceiverName = dal.GetCustomer(dalParcel.TargetId).Name,
            //            Weight = (Enums.WeightCategories) dalParcel.Weight,
            //            Status = 
            //        })
            //    .Where(dp => f(dp)));


            var parselsForList = from parcel in dal.GetParcels(_ => true)
                                 select new ParcelForList()
                                 {
                                     Id = parcel.Id,
                                     Priority = (BO.Enums.Priorities)parcel.Priority,
                                     SenderName = dal.GetCustomer(parcel.SenderId).Name,
                                     ReceiverName = dal.GetCustomer(parcel.TargetId).Name,
                                     Weight = (Enums.WeightCategories)parcel.Weight,
                                     Status = parcel.AssigndedTime == null ? Enums.ParcelStatus.DEFINED :
                                              (parcel.PickedUpTime == null ? Enums.ParcelStatus.ASSIGNED :
                                              (parcel.DeliveryTime == null ? Enums.ParcelStatus.PICKEDUP :
                                              Enums.ParcelStatus.DELIVERED))
                                 };

            return new ObservableCollection<ParcelForList>(from parcel in parselsForList
                                                           where f(parcel)
                                                           select parcel);
        }


        #endregion

        #region Public Calls for Lists Functions

        /// <summary>
        /// returns the list of base stations.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public ObservableCollection<BaseStationForList> GetBaseStations()
        {
            return GetBaseStations(_ => true);
        }

        /// <summary>
        ///  returns the list of drones.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public ObservableCollection<DroneForList> GetDrones()
        {
            return GetDrones(_ => true);
        }

        /// <summary>
        ///  returns the list of customers.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public ObservableCollection<CustomerForList> GetCustomers()
        {
            return GetCustomers(_ => true);
        }

        /// <summary>
        ///  returns the list of parcels.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public ObservableCollection<ParcelForList> GetPacels()
        {
            return GetPacels(_ => true);
        }

        /// <summary>
        /// returns the list of parcels that involve some customer as a sender or a rceiver.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ObservableCollection<ParcelForList> GetPacelsThatIncludeTheCustomer(int customerId)
        {
            return GetPacels(p => GetParcel(p.Id).Sender.Id == customerId || GetParcel(p.Id).Reciver.Id == customerId);
        }

        /// <summary>
        /// returns the list of customers that included in some customer.
        /// </summary>
        /// <param name="parcelList"></param>
        /// <returns></returns>
        public ObservableCollection<CustomerForList> GetCustomersThatIncludeTheCustomer(ObservableCollection<BO.ParcelForList> parcelList)
        {
            return GetCustomers(c => parcelList.Any(p => p.SenderName == c.Name) || parcelList.Any(p => p.ReceiverName == c.Name));
        }
        
        /// <summary>
        /// get drones for thwe selectors.
        /// </summary>
        /// <param name="whight"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ObservableCollection<DroneForList> GetDronesForSelectors(string whight, string status)
        {
            return GetDrones(d =>
                    (d.Weight.ToString() == whight || whight == "Show All") &&
                    (d.Status.ToString() == status || status == "Show All"));
        }
        
        /// <summary>
        /// get parcels according to the selectors.
        /// </summary>
        /// <param name="parcelStaus"></param>
        /// <returns></returns>
        public ObservableCollection<ParcelForList> GetPacelsForSalector(string parcelStaus)
        {
            return GetPacels(b =>
                b.Status.ToString() == parcelStaus || parcelStaus == "Show All"
            );
        }
        
        /// <summary>
        /// get the base stations according to the selectors.
        /// </summary>
        /// <param name="openSlots"></param>
        /// <returns></returns>
        public ObservableCollection<BaseStationForList> GetBaseStationsForSelector(string openSlots)
        {
            return GetBaseStations(b =>
                ((b.FreeChargingSlots > 0) && (openSlots == "Has Open Charging Slots")) || (openSlots == "Show All") || openSlots == null);

        }


        #endregion

        #region getserial numbers

        /// <summary>
        /// geting all the ditails from the class config to the BL.
        /// </summary>
        /// <returns></returns>
        public int GetNextSerialNumberForParcel()
        {
            return dal.GetSerialNumber();
        }

        #endregion

        #region operation functions

        /// <summary>
        /// function that gets two locations and returns the destance between them.
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private double Distance(BO.Location l1, BO.Location l2)
        {
            int R = 6371;

            double f1 = ConvertToRadians(l1.Latitude);
            double f2 = ConvertToRadians(l2.Latitude);

            double df = ConvertToRadians(l1.Latitude - l2.Latitude);
            double dl = ConvertToRadians(l1.Longitude - l2.Longitude);

            double a = Math.Sin(df / 2) * Math.Sin(df / 2) +
            Math.Cos(f1) * Math.Cos(f2) *
            Math.Sin(dl / 2) * Math.Sin(dl / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance.
            double d = R * c;

            return d;



            //var baseRad = Math.PI * l1.Latitude / 180;
            //var targetRad = Math.PI * l2.Latitude / 180;
            //var theta = l1.Longitude - l2.Longitude;
            //var thetaRad = Math.PI * theta / 180;

            //double dist =
            //    Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
            //    Math.Cos(targetRad) * Math.Cos(thetaRad);
            //dist = Math.Acos(dist);

            //dist = dist * 180 / Math.PI;
            //dist = dist * 9 * 1.1515; // the size in not the original size of earth.

            //return dist;
        }

        /// <summary>
        /// function that converts radiens to degries for the distance caculation.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        /// <summary>
        /// the function gets idal locatin and transforms it into bal location by coping the values.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static BO.Location LocationTranslate(DO.Location location)
        {
            return new BO.Location() { Longitude = location.Longitude, Latitude = location.Latitude };
        }

        /// <summary>
        /// the function gets bl locatin and transforms it into idal location by coping the values.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static DO.Location LocationTranslate(BO.Location location)
        {
            return new DO.Location() { Longitude = location.Longitude, Latitude = location.Latitude };
        }

        /// <summary>
        /// simple converts the parcel to parcel for list.
        /// </summary>
        /// <param name="parcels"></param>
        /// <returns></returns>
        private IEnumerable<ParcelByCustomer> ParcelToParcelByCustumerList(IEnumerable<BO.ParcelForList> parcels)
        {
            return parcels.Select(p => new ParcelByCustomer()
            {
                Id = p.Id,
                Priority = p.Priority,
                Status = p.Status,
                Weight = p.Weight
            });
        }


        #endregion

        #region Automatic

        /// <summary>
        /// the functionthat is being added to the Do_Work event.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="droneId"></param>
        /// <param name="chargingLength"></param>
        public void AutomaticOperation(BackgroundWorker worker, int droneId, int chargingLength)
        {
            //as long as the flag of canceling is turned off.
            while (worker.CancellationPending != true)
            {
                try
                {
                    //sends ther drone to delivery.
                    SendToDelivery(worker, droneId);
                }
                // if there is no parrcel to assign or the battary is too low.
                catch (BO.UnableToAssignParcelToTheDroneException e)
                {
                    //sends the drone to charge.
                    if (!(GetDrone(droneId).Battery == 100))
                    {
                        AutomaticCharging(worker, droneId, chargingLength);
                    }
                    else
                    {
                        //no more parcels for the drone to deliver.
                        //every 10 secs it is serching for a new parcel.
                        System.Threading.Thread.Sleep(10000);
                    }
                }
            }
        }

        /// <summary>
        /// the function that sends the drone to delivery.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="droneId"></param>
        private void SendToDelivery(BackgroundWorker worker, int droneId)
        {
            //assign the parcel to the drone then picking it up then delivering it the reciver.
            System.Threading.Thread.Sleep(1000);
            AssignParcelToADrone(droneId);
            worker.ReportProgress(0);
            System.Threading.Thread.Sleep(1000);
            PickingUpParcelToDrone(droneId);
            worker.ReportProgress(0);
            System.Threading.Thread.Sleep(1000);
            DeliveringParcelFromADrone(droneId);
            worker.ReportProgress(0);
        }

        /// <summary>
        /// the function that recharge the drone and shows the progress of the charging.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="droneId"></param>
        /// <param name="chargingLength"></param>
        private void AutomaticCharging(BackgroundWorker worker, int droneId, int chargingLength)
        {
            ChargeDrone(droneId);

            int index = this.drones.FindIndex(d => d.Id == droneId);

            for (int i = 1; i <= chargingLength; i++)
            {
                // Perform a time consuming operation and report progress.
                System.Threading.Thread.Sleep(50);
                if (updateBattary(index))
                    worker.ReportProgress(0);
                else
                    break;
            }
            UnChargeDrone(droneId);
        }

        /// <summary>
        /// updates the battary by adding ine to it every time.
        /// </summary>
        /// <param name="droneIndex"></param>
        /// <returns></returns>
        private bool updateBattary(int droneIndex)
        {

            if (drones[droneIndex].Battery < 100)
            {
                ++drones[droneIndex].Battery;
                return true;
            }
            else
            {
                drones[droneIndex].Battery = 100;
                return false;
            }
        }

        #endregion

    } //END BL class

} //end IBAL