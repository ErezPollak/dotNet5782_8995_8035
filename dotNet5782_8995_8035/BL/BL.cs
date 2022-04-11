using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BL.Abstracts;
using BL.Exceptions;
using BL.Models;
using DalApi;
using DO;
using BaseStation = BL.Models.BaseStation;
using Customer = BL.Models.Customer;
using Drone = BL.Models.Drone;
using IdAlreadyExistsException = BL.Exceptions.IdAlreadyExistsException;
using IdDontExistsException = BL.Exceptions.IdDontExistsException;
using Location = DO.Location;
using Parcel = BL.Models.Parcel;

namespace BL
{
    internal sealed class Bl : IBl
    {

        #region private fields

        private readonly IDal _dal = DalFactory.GetDal();

        //for the speed of the charge. percentage for hour.
        private double ChargingSpeed { get; }

        /// <summary>
        /// the list of the drones that are being held by the BL.
        /// </summary>
        private readonly List<DroneForList> _drones;

        /// <summary>
        /// random values
        /// </summary>
        private static readonly Random Random = new();

        /**
         * tolerance for not accurate values of the battery.
         */
        private const double Tolerance = 1;
        
        #endregion

        #region Singalton


        /// <summary>
        /// private constructor for the Bl class, for the singleton.
        /// </summary>
        private Bl()
        {
            var electricityUse = _dal.ElectricityUse();

            ChargingSpeed = electricityUse[4];

            var droneCharges = _dal.GetChargeDrones(_ => true);

            //translates all the drones from the data level to 
            _drones = (from dalDrone in _dal.GetDrones(_ => true)
                           select new DroneForList()
                           {
                               Id = dalDrone.Id,
                               Model = dalDrone.Model,
                               Weight = (Enums.WeightCategories)dalDrone.MaxWeight,
                               Status = droneCharges.Any(dc => dc.DroneId == dalDrone.Id) ? Enums.DroneStatuses.MAINTENANCE : Enums.DroneStatuses.FREE
                           }).ToList();

            //takes all the parcels that are assigned to a drone and was not delivered.
            var parcelsForUpdate = _dal.GetParcels(parcel => parcel.DroneId != 0 && parcel.DeliveryTime == null).ToList();

            //going through all the parcels that have a drone, and was not assigned to it.
            foreach (var parcel in parcelsForUpdate)
            {
                //assigning the parcel to the drone.
                _dal.AssignDroneToParcel(parcel.Id, parcel.DroneId);

                //calculate the distance of the delivery.
                double deliveryDistance = 0;
                //calculating the distance between the sender of the parcel, and the receiver. 
                deliveryDistance += Distance(LocationTranslate(_dal.GetCustomer(parcel.SenderId).Location),
                                             LocationTranslate(_dal.GetCustomer(parcel.TargetId).Location));
                //calculating the distance between the receiver of the parcel, and the clothes station to the receiver. 
                deliveryDistance += Distance(LocationTranslate(_dal.GetCustomer(parcel.TargetId).Location),
                                             LocationTranslate(_dal.GetBaseStation(_dal.GetClosestStation(parcel.TargetId)).Location));

                double minimumValue;

                //finding the index of the drone that.
                var index = _drones.FindIndex(d => d.Id == parcel.DroneId);

                //updates the status of the drone to be delivery. 
                _drones[index].Status = Enums.DroneStatuses.DELIVERY;
                //updates the parcel number to be the delivering parcel.
                _drones[index].ParcelId = parcel.Id;

                //if the parcel wasn't picked up.
                if (parcel.PickedUpTime == null)
                {
                    //setting the location of the drone to be in the clothes station to the sender.
                    _drones[index].Location = LocationTranslate(_dal
                        .GetBaseStation(_dal.GetClosestStation(parcel.SenderId)).Location);

                    // calculating the distance between the base station to the sender.
                    deliveryDistance +=
                        Distance(LocationTranslate(_dal.GetBaseStation(_dal.GetClosestStation(parcel.SenderId)).Location),
                                 LocationTranslate(_dal.GetCustomer(parcel.SenderId).Location));

                    // calculating the minimum percentage of battery the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance / (int)(electricityUse[(int)GetDrone(parcel.DroneId).MaxWeight + 1]);
                }
                else // if the parcel was picked up.
                {
                    //setting the location of the drone to be the location of the sender.
                    _drones[index].Location =
                        LocationTranslate(_dal.GetCustomer(parcel.SenderId).Location);

                    // calculating the minimum percentage of battery the drone needs in order to deliver the parcel and go to charge afterwards.
                    minimumValue = deliveryDistance / (int)(electricityUse[(int)GetDrone(parcel.DroneId).MaxWeight + 1]);
                }

                //if the percentage is ok, the value of the battery is being randomised between the minimum value to one hundred.
                _drones[index].Battery = minimumValue + Random.Next() % (100 - minimumValue);
            }

            //needed for the iteration.
            var parcels = _dal.GetParcels(_ => true);

            //going through all the drones.
            foreach (var drone in _drones)
            {
                switch (drone.Status)
                {
                    // if the drone wasn't changed in the last iteration, meaning that the status is either FREE or MAINTENANCE.
                    case Enums.DroneStatuses.FREE:
                    {
                        //getting random value from the list.
                        var enumerable = parcels.ToList();
                        var index = Random.Next() % enumerable.Count;

                        //setting the drones location the drone to be in a target location of the randomised parcel.
                        drone.Location = LocationTranslate(_dal.GetCustomer(enumerable.ElementAt(index).TargetId).Location);

                        //calculating the distance to the closest station.
                        var deliveryDistance =
                            Distance(
                                LocationTranslate(_dal
                                    .GetBaseStation(_dal.GetClosestStation(enumerable.ElementAt(index).TargetId)).Location),
                                LocationTranslate(_dal.GetCustomer(enumerable.ElementAt(index).TargetId).Location));

                        //calculating the battery consumption.
                        var battyConsumption = deliveryDistance / electricityUse[(int)drone.Weight + 1];

                        //setting the battery to be randomised between the minimum value to 100.
                        drone.Battery = (int)(battyConsumption + Random.NextDouble() * (100 - battyConsumption));
                        break;
                    }
                    case Enums.DroneStatuses.MAINTENANCE:
                    {
                        var availableBaseStations = _dal.GetBaseStations(b => b.ChargeSlots > 0);

                        //random number of a station.
                        var baseStations = availableBaseStations.ToList();
                        var stationIndex = Random.Next() % baseStations.Count;

                        //setting the location of the drone to be the location of the randomised station.
                        drone.Location = LocationTranslate(baseStations.ElementAt(stationIndex).Location);

                        // updating the battery to be a random value from 0 to 20.
                        drone.Battery = Random.Next() % 20;

                        if (_dal.GetChargeDrones(_ => true).All(dc => dc.DroneId != drone.Id))
                        {
                            //sending the drone to charge.
                            _dal.ChargeDrone(baseStations.ElementAt(stationIndex).Id, drone.Id);
                        }

                        break;
                    }
                    case Enums.DroneStatuses.DELIVERY:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        } //end BL ctor

        /// <summary>
        /// bl field intended to keep the instance of the bl that was created.
        /// </summary>
        private static readonly Lazy<IBl> Instance = new(() => new Bl());

        /// <summary>
        /// the function the creates new instance of bl only if it doesn't exists already.
        /// </summary>
        /// <returns></returns>
        public static IBl GetInstance()
        {
            return Instance.Value;
        }

        #endregion

        #region adding option

        /// <summary>
        /// the function gets a logical base station, converting it to a dal baseStation and adding it to the database. 
        /// </summary>
        /// <param name="newBaseStation">the new base station to add to the data base</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddBaseStation(BaseStation newBaseStation)
        {
            if (newBaseStation.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            var baseStation = new DalFacade.Models.BaseStation()
            {
                Id = newBaseStation.Id,
                Name = newBaseStation.Name,
                Location = LocationTranslate(newBaseStation.Location),
                ChargeSlots = newBaseStation.ChargeSlots
            };

            try
            {
                return _dal.AddBaseStation(baseStation);
            }
            catch (Exception e)
            {
                throw new IdAlreadyExistsException(newBaseStation.Id, "base station", e);
            }
        }

        /// <summary>
        /// the function gets a logical drone, converting it to a dal drone and adding it to the database. 
        /// </summary>
        /// <param name="newDrone">logical drone</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddDrone(Drone newDrone)
        {

            if (newDrone.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            //check if the drone exists int he drone list in the bl.
            if (_drones.Exists(d => d.Id == newDrone.Id))
            {
                throw new IdAlreadyExistsException(newDrone.Id, "drone");
            }

            //assigning the drone to a random available station

            //gets all the available stations(stations with free slots).
            IEnumerable<BaseStationForList> availableBaseStations = GetBaseStations(b => b.FreeChargingSlots > 0);
            if (!availableBaseStations.Any())
                throw new UnableToAddDroneException("No BaseStation Available");
            //rands one of the stations and puts the drone in there.
            var randomlyStationIndex = Random.Next(availableBaseStations.Count());
            newDrone.Location = GetBaseStation(availableBaseStations.ElementAt(randomlyStationIndex).Id).Location;
           

            //adding the drone to the database of drones in the bl.
            var balDrone = new DroneForList()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                Weight = newDrone.MaxWeight,
                Battery = newDrone.Battery,
                Location = newDrone.Location,
                Status = newDrone.Status,
                ParcelId = 0 // because every new drone is being added without a parcel.
            };
            _drones.Add(balDrone);


            //calling the dal function of adding drone.
            var dalDrone = new DO.Drone()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (WeightCategories)newDrone.MaxWeight
            };

            //no need for try, since we checked in the list there is no chance to have the same number of drone in the dal list.   
            var b = _dal.AddDrone(dalDrone);

            _dal.ChargeDrone(availableBaseStations.ElementAt(randomlyStationIndex).Id, newDrone.Id);

            return b;
        }

        /// <summary>
        /// the function gets a logical customer, converting it to a dal customer and adding it to the database. 
        /// </summary>
        /// <param name="newCustomer">logical customer</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddCustomer(Customer newCustomer)
        {
            if (newCustomer.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            var customer = new DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Location = LocationTranslate(newCustomer.Location)
            };

            try
            {
                return _dal.AddCustomer(customer);
            }
            catch (Exception e)
            {
                throw new IdAlreadyExistsException(newCustomer.Id, "customer", e);
            }
        }

        /// <summary>
        /// the function gets a logical parcel, converting it to a dal parcel and adding it to the database. 
        /// </summary>
        /// <param name="newParcel">logical parcel</param>
        /// <returns>true if the adding eas successful</returns>
        public bool AddParcel(Parcel newParcel)
        {

            if (newParcel.Id == 0)
                throw new IdZeroException("ID cannot be zero");

            var dalParcel = new DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Receiver.Id,
                Weight = (WeightCategories)newParcel.Weight,
                Priority = (Priorities)newParcel.Priority,
                DroneId = 0,
                DefinedTime = newParcel.DefinedTime,
                DeliveryTime = newParcel.DeliveringTime,
                AssignedTime = newParcel.AssignedTime,
                PickedUpTime = newParcel.PickupTime
            };

            try
            {
                return _dal.AddParcel(dalParcel);
            }
            catch (Exception e)
            {
                throw new IdAlreadyExistsException(newParcel.Id, "parcel", e);
            }
        }

        #endregion

        #region update options 

        /// <summary>
        /// calling the function from the dal that changes the name of the drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        /// <returns> true if the update completed successfully </returns>
        public bool UpdateNameForADrone(int droneId, string model)
        {

            if (model == "")
                throw new ModelEmptyException();

            var index = _drones.FindIndex(d => d.Id == droneId);

            if (index != -1)
                _drones[index].Model = model;

            try
            {
                return _dal.UpdateModelForADrone(droneId, model);
            }
            catch (Exception e)
            {
                throw new IdDontExistsException(droneId, "drone", e);
            }
        }

        /// <summary>
        /// calling the function from the dal that changes the name and number of slots of the base station.
        /// </summary>
        /// <param name="basStationId"></param>
        /// <param name="name"></param>
        /// <param name="slots"></param>
        /// <returns> true if the update completed successfully </returns>
        public bool UpdateBaseStation(int basStationId, string name, int slots)
        {
            try
            {
                return _dal.UpdateBaseStation(basStationId, name, slots);
            }
            catch (Exception e)
            {
                throw new IdDontExistsException(basStationId, "baseStation", e);
            }
        }

        /// <summary>
        ///  calling the function from the dal that changes the name and phone number of a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool UpdateCustomer(int customerId, string name, string phone)
        {
            try
            {
                return _dal.UpdateCustomer(customerId, name, phone);
            }
            catch (Exception e)
            {
                throw new IdDontExistsException(customerId, "customer", e);
            }
        }

        /// <summary>
        /// finding the best parcel and assigning it to the given drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool AssignParcelToADrone(int droneId)
        {
            //fining the index of the drone with that id.
            var droneIndex = _drones.FindIndex(d => d.Id == droneId);

            //if the index is -1 it means that no such is id in the database so an exception will be thrown.
            if (droneIndex == -1)
                throw new UnableToAssignParcelToTheDroneException(droneId, " drone is not in the database.",
                    new IdDontExistsException(droneId, "drone"));

            //checks if the drone is free, and if not exception will be thrown.
            if (_drones[droneIndex].Status != Enums.DroneStatuses.FREE)
                throw new UnableToAssignParcelToTheDroneException(droneId, " the drone is not free");

            //importing all the parcels and sorting them according to their priority.
            IEnumerable<DO.Parcel> parcels = _dal.GetParcels(p =>
                ((int)p.Weight <= (int)_drones[droneIndex].Weight) &&
                (
                   Distance(_drones[droneIndex].Location, LocationTranslate(_dal.GetCustomer(p.SenderId).Location)) +
                   Distance(LocationTranslate(_dal.GetCustomer(p.SenderId).Location), LocationTranslate(_dal.GetCustomer(p.TargetId).Location)) +
                   Distance(LocationTranslate(_dal.GetCustomer(p.TargetId).Location), GetBaseStation(_dal.GetClosestStation(p.TargetId)).Location)
                )
                <= _drones[droneIndex].Battery * _dal.ElectricityUse()[(int)_drones[droneIndex].Weight + 1] &&

                 p.DeliveryTime == null && p.AssignedTime == null)

                .OrderByDescending(p => (int)p.Priority)

                .ThenBy(p => Distance(_drones[droneIndex].Location, LocationTranslate(_dal.GetCustomer(p.SenderId).Location)));

            //if no parcel left in the list after the removing it means that no parcel can be sent be thi drone, so exception will be thrown.
            var enumerable = parcels.ToList();
            if (!enumerable.Any())
                throw new UnableToAssignParcelToTheDroneException(droneId,
                    " No parcel can be sent by this drone due to one of the following reasons:\n" +
                    " 1) all the parcels are too heavy. \n" +
                    " 2) too long distances.\n" +
                    " 3) there is no parcel that is waiting for delivery");

            //if there is a parcel that matches the needing of the drone update will happen.
            _drones[droneIndex].Status = Enums.DroneStatuses.DELIVERY;

            _drones[droneIndex].ParcelId = enumerable.First().Id;

            //update parcel times.
            _dal.AssignDroneToParcel(enumerable.First().Id, droneId);

            return true;
        }

        public bool PickingUpParcelToDrone(int droneId)
        {
            //if the drone is not in thr database, an exception will be thrown.
            if (GetDrone(droneId).Status != Enums.DroneStatuses.DELIVERY)
                throw new UnableToDeliverParcelFromTheDroneException(droneId,
                    "the drone is not delivering any parcel.");

            //Calculating the distance of the delivery.
            var distance = Distance(
                GetDrone(droneId).Location,
                GetLocationOfCustomer(droneId, Enums.CustomerEnum.SENDER));

            var index = _drones.FindIndex(d => d.Id == droneId);

            //battery update
            _drones[index].Battery -=
                distance / _dal.ElectricityUse()[(int)GetDrone(droneId).MaxWeight + 1];

            // location update
            _drones[index].Location = GetLocationOfCustomer(droneId, Enums.CustomerEnum.SENDER);

            //update the parcel from the dal.

            try
            {
                return _dal.PickingUpParcel(GetDrone(droneId).ParcelInDelivery.Id, droneId);
            }
            catch (Exception e)
            {
                throw new UnableToDeliverParcelFromTheDroneException(droneId, "parcel or drone is not exist", e);
            }
        }

        /// <summary>
        /// the function is delivering the parcel that the drone is delivering 
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

            //calculating the distance of the delivery.
            var deliveryDistance = Distance(
                GetLocationOfCustomer(droneId, Enums.CustomerEnum.SENDER),
                GetLocationOfCustomer(droneId, Enums.CustomerEnum.TARGET));

            var index = _drones.FindIndex(d => d.Id == droneId);

            //battery update
            _drones[index].Battery -=
                deliveryDistance / _dal.ElectricityUse()[(int)GetDrone(droneId).MaxWeight + 1];

            // location update
            _drones[index].Location = GetLocationOfCustomer(droneId, Enums.CustomerEnum.TARGET);

            //status update
            _drones[index].Status = Enums.DroneStatuses.FREE;

            var saveParcelId = _drones[index].ParcelId;

            //parcel id update.
            _drones[index].ParcelId = 0;

            //update the parcel from the dal.

            try
            {
                //return dalObject.DeliveringParcel(GetDrone(droneId).ParcelInDelivery.Id);
                return _dal.DeliveringParcel(saveParcelId);
            }
            catch (Exception e)
            {
                throw new UnableToDeliverParcelFromTheDroneException(droneId, "parcel or drone is not exist", e);
            }
        }

        private Models.Location GetLocationOfCustomer(int droneId, Enums.CustomerEnum typeOfCustomer)
        {
            return typeOfCustomer switch
            {
                Enums.CustomerEnum.SENDER => LocationTranslate(_dal
                    .GetCustomer(_dal.GetParcel(GetDrone(droneId).ParcelInDelivery.Id).SenderId)
                    .Location),
                Enums.CustomerEnum.TARGET => LocationTranslate(_dal
                    .GetCustomer(_dal.GetParcel(GetDrone(droneId).ParcelInDelivery.Id).TargetId)
                    .Location),
                _ => throw new Exception("type of customer is not CustomerEnum")
            };
        }

        /// <summary>
        /// the function finds the base station and sending the drone to charge there by the dal function.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool ChargeDrone(int droneId)
        {
            var droneIndex = _drones.FindIndex(d => d.Id == droneId);

            if (droneIndex == -1)
                throw new IdDontExistsException(droneId, "drone");

            //checks if the drone is free, and if not exception will be thrown.
            if (_drones[droneIndex].Status != Enums.DroneStatuses.FREE)
                throw new UnAbleToSendDroneToChargeException(" the drone is not free");

            //getting the maximum distance the drone can make according to the level of battery multiplied by the number of kilometers the drone can do for one present do battery. 
            var maxDistance = _drones[droneIndex].Battery *
                              _dal.ElectricityUse()[(int)_drones[droneIndex].Weight + 1];

            //returns all the stations that the drone has enough fuel to get to, ordered by the closest base station.
            IEnumerable<DalFacade.Models.BaseStation> baseStations = _dal.GetBaseStations(b =>
                    (Distance(LocationTranslate(b.Location), GetDrone(droneId).Location) <= maxDistance) &&
                    (b.ChargeSlots > 0))
                .OrderBy(b => Distance(LocationTranslate(b.Location), GetDrone(droneId).Location));

            //if there is no suitable station an exception will be thrown.
            var enumerable = baseStations.ToList();
            if (!enumerable.Any())
                throw new UnAbleToSendDroneToChargeException(
                    " there is no station that matches the needs of this drone");

            var baseStation = enumerable.First();

            /////drone updates//////

            //update the battery status.
            _drones[droneIndex].Battery -=
                Distance(LocationTranslate(baseStation.Location), GetDrone(droneId).Location) /
                _dal.ElectricityUse()[(int)_drones[droneIndex].Weight + 1];

            //update the location of the drone to be the location of the base station.
            _drones[droneIndex].Location = LocationTranslate(baseStation.Location);

            //updating the status of the drone.
            _drones[droneIndex].Status = Enums.DroneStatuses.MAINTENANCE;

            //update the number of the charge slots and adding an element in the dal functions.
            return _dal.ChargeDrone(baseStation.Id, droneId);
        }

        /// <summary>
        /// the function plugging out a drone from charge in the base station.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool UnChargeDrone(int droneId)
        {
            var droneIndex = _drones.FindIndex(d => d.Id == droneId);
            if (droneIndex == -1)
                throw new IdDontExistsException(droneId, "drone");

            //checks if the drone is free, and if not exception will be thrown.
            if (_drones[droneIndex].Status != Enums.DroneStatuses.MAINTENANCE)
                throw new UnAbleToReleaseDroneFromChargeException(droneId, "the drone is not in maintenance");

            //returning from the dal the value of the time that the drone was in charge
            var minutes = _dal.UnChargeDrone(droneId);

            //update the battery status.
            _drones[droneIndex].Battery += minutes * ChargingSpeed;
            if (_drones[droneIndex].Battery > 100)
            {
                _drones[droneIndex].Battery = 100;
            }

            //updating the status of the drone.
            _drones[droneIndex].Status = Enums.DroneStatuses.FREE;

            return true;
        }

        #endregion

        #region show options 

        /// <summary>
        /// returns a base station according to its id number.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public BaseStation GetBaseStation(int baseStationId)
        {
            DalFacade.Models.BaseStation dalBaseStation;
            try
            {
                dalBaseStation = _dal.GetBaseStation(baseStationId);
            }
            catch (Exception e)
            {
                throw new IdDontExistsException(baseStationId, "base station", e);
            }

            var charges = ChargeDroneToDroneInCharge(_dal.GetChargeDrones(d => d.StationId == dalBaseStation.Id));

            return new BaseStation()
            {
                Id = dalBaseStation.Id,
                Name = dalBaseStation.Name,
                ChargeSlots = dalBaseStation.ChargeSlots,
                Location = LocationTranslate(dalBaseStation.Location),
                ChargingDrones = charges
            };
        }

        /// <summary>
        /// returns a list with all the drone charges converted to drones in charge
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
        public Drone GetDrone(int droneId)
        {
            var index = _drones.FindIndex(d => d.Id == droneId);

            if (index == -1)
                throw new IdDontExistsException(droneId, "drone",
                    new DO.IdDontExistsException(droneId, "drone"));

            ParcelInDelivery parcelInDelivery;

            if (_drones[index].ParcelId != 0)
            {
                var parcel = GetParcel(_drones[index].ParcelId);
                var sender = GetCustomer(parcel.Sender.Id);
                var receiver = GetCustomer(parcel.Receiver.Id);

                parcelInDelivery = new ParcelInDelivery()
                {
                    Id = parcel.Id,
                    Priority = parcel.Priority,
                    Sender = parcel.Sender,
                    Receiver = parcel.Receiver, 
                    DeliveringLocation = sender.Location,
                    PickupLocation = receiver.Location,
                    Status = Enums.ParcelStatus.ASSIGNED,
                    Weight = parcel.Weight, 
                    Distance = Distance(sender.Location, receiver.Location)
                };
            }
            else
            {
                parcelInDelivery = null;
            }

            return new Drone()
            {
                Id = _drones[index].Id,
                Model = _drones[index].Model,
                Location = _drones[index].Location,
                Status = _drones[index].Status,
                Battery = _drones[index].Battery,
                MaxWeight = _drones[index].Weight,
                ParcelInDelivery = parcelInDelivery
            };
        }

        /// <summary>
        /// returns a customer according to its id number.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            DO.Customer dalCustomer;

            try
            {
                dalCustomer = _dal.GetCustomer(customerId);
            }
            catch (DO.IdDontExistsException e)
            {
                throw new IdDontExistsException(customerId, "customer", e);
            }

            return new Customer()
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = LocationTranslate(dalCustomer.Location),
                FromCustomer = ParcelToParcelByCustomerList(GetParcels(p => p.SenderName == dalCustomer.Name)).ToList(),
                ToCustomer = ParcelToParcelByCustomerList(GetParcels(p => p.ReceiverName == dalCustomer.Name)).ToList()

            };
        }

        /// <summary>
        /// returns a parcel according to its id number.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            try
            {
                var dalParcel = _dal.GetParcel(parcelId);

                return new Parcel()
                {
                    Id = dalParcel.Id,

                    Sender = (from customer in _dal.GetCustomers(_ => true)
                              where customer.Id == dalParcel.TargetId
                              select new CoustomerForParcel
                              {
                                  Id = customer.Id,
                                  CustomerName = customer.Name
                              }).FirstOrDefault(),

                    Receiver = (from customer in _dal.GetCustomers(_ => true)
                               where customer.Id == dalParcel.TargetId
                               select new CoustomerForParcel
                               {
                                   Id = customer.Id,
                                   CustomerName = customer.Name
                               }).FirstOrDefault(),

                    Drone = new DroneForParcel()
                    {
                        Id = dalParcel.DroneId
                    },

                    Priority = (Enums.Priorities)dalParcel.Priority,
                    Weight = (Enums.WeightCategories)dalParcel.Weight,
                    AssignedTime = dalParcel.AssignedTime,
                    DefinedTime = dalParcel.DefinedTime,
                    DeliveringTime = dalParcel.DeliveryTime,
                    PickupTime = dalParcel.PickedUpTime
                };
            }
            catch (DO.IdDontExistsException e)
            {
                throw new IdDontExistsException(parcelId, "parcel", e);
            }
        }
        
        public ObservableCollection<BaseStationForList> GetBaseStations()
        {
            return GetBaseStations(_ => true);
        }


        /// <summary>
        /// returns the list of base stations that return true for the predicate f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<BaseStationForList> GetBaseStations(Predicate<BaseStationForList> f)
        {
            return new ObservableCollection<BaseStationForList>(_dal.GetBaseStations(_ => true)
                .Select(db =>
                    new BaseStationForList()
                    {
                        Id = db.Id,
                        Name = db.Name,
                        FreeChargingSlots = db.ChargeSlots,
                        TakenCharingSlots = _dal.GetChargeDrones(cd => cd.StationId == db.Id).Count()
                    })
                .Where(bs => f(bs)));
        }

        /// <summary>
        ///  returns the list of drones that return true for the predicate f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<DroneForList> GetDrones(Predicate<DroneForList> f)
        {
            var balDrones = new ObservableCollection<DroneForList>();

            _drones.FindAll(f).ForEach(d => balDrones.Add(new DroneForList()
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
        ///  returns the list of customers that return true for the predicate f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<CustomerForList> GetCustomers(Predicate<CustomerForList> f)
        {
            return new ObservableCollection<CustomerForList>(_dal.GetCustomers(_ => true)
                .Select(c =>
                    new CustomerForList()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Phone = c.Phone,
                        //calculating the number of parcels that were sent to this customer that were sent and delivered. 
                        SentToAndDeliverd = _dal.GetParcels(p =>
                            p.TargetId == c.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                        //calculating the number of parcels that were sent to this customer that were sent and not delivered. 
                        SentToAnDNotDelivered = _dal.GetParcels(p =>
                            p.TargetId == c.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                        //calculating the number of parcels that were sent from this customer that were sent and delivered. 
                        SentFromAndDeliverd = _dal.GetParcels(p =>
                            p.SenderId == c.Id && p.PickedUpTime != null && p.DeliveryTime != null).Count(),
                        //calculating the number of parcels that were sent from this customer that were sent and not delivered. 
                        SentFromAndNotDeliverd = _dal.GetParcels(p =>
                            p.SenderId == c.Id && p.PickedUpTime != null && p.DeliveryTime == null).Count(),
                    })
                .Where(c => f(c)));
        }

        /// <summary>
        ///  returns the list of parcels that return true for the predicate f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ObservableCollection<ParcelForList> GetParcels(Predicate<ParcelForList> f)
        {

            var parcelsForList = from parcel in _dal.GetParcels(_ => true)
                                 select new ParcelForList()
                                 {
                                     Id = parcel.Id,
                                     Priority = (Enums.Priorities)parcel.Priority,
                                     SenderName = _dal.GetCustomer(parcel.SenderId).Name,
                                     ReceiverName = _dal.GetCustomer(parcel.TargetId).Name,
                                     Weight = (Enums.WeightCategories)parcel.Weight,
                                     Status = parcel.AssignedTime == null ? Enums.ParcelStatus.DEFINED :
                                              (parcel.PickedUpTime == null ? Enums.ParcelStatus.ASSIGNED :
                                              (parcel.DeliveryTime == null ? Enums.ParcelStatus.PICKED_UP :
                                              Enums.ParcelStatus.DELIVERED))
                                 };

            return new ObservableCollection<ParcelForList>(from parcel in parcelsForList
                                                           where f(parcel)
                                                           select parcel);
        }


        #endregion

        #region Public Calls for Lists Functions

        /// <summary>
        /// returns the list of base stations.
        /// </summary>
        /// <param name="unknown"></param>
        /// <returns></returns>
        public ObservableCollection<BaseStationForList> GetBaseStations(object unknown)
        {
            return GetBaseStations(_ => true);
        }

        /// <summary>
        ///  returns the list of drones.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<DroneForList> GetDrones()
        {
            return GetDrones(_ => true);
        }

        /// <summary>
        ///  returns the list of customers.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<CustomerForList> GetCustomers()
        {
            return GetCustomers(_ => true);
        }

        /// <summary>
        ///  returns the list of parcels.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ParcelForList> GetParcels()
        {
            return GetParcels(_ => true);
        }

        /// <summary>
        /// returns the list of parcels that involve some customer as a sender or a receiver.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ObservableCollection<ParcelForList> GetParcelsThatIncludeTheCustomer(int customerId)
        {
            return GetParcels(p => GetParcel(p.Id).Sender.Id == customerId || GetParcel(p.Id).Receiver.Id == customerId);
        }

        /// <summary>
        /// returns the list of customers that included in some customer.
        /// </summary>
        /// <param name="parcelList"></param>
        /// <returns></returns>
        public ObservableCollection<CustomerForList> GetCustomersThatIncludeTheCustomer(ObservableCollection<ParcelForList> parcelList)
        {
            return GetCustomers(c => parcelList.Any(p => p.SenderName == c.Name) || parcelList.Any(p => p.ReceiverName == c.Name));
        }
        
        /// <summary>
        /// get drones for the selectors.
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ObservableCollection<DroneForList> GetDronesForSelectors(string weight, string status)
        {
            return GetDrones(d =>
                    (d.Weight.ToString() == weight || weight == "Show All") &&
                    (d.Status.ToString() == status || status == "Show All"));
        }
        
        /// <summary>
        /// get parcels according to the selectors.
        /// </summary>
        /// <param name="parcelStatus"></param>
        /// <returns></returns>
        public ObservableCollection<ParcelForList> GetParcelsForSelector(string parcelStatus = "DEFINED")
        {
            return GetParcels(b =>
                b.Status.ToString() == parcelStatus || parcelStatus == "Show All"
            );
        }
        
        /// <summary>
        /// get the base stations according to the selectors.
        /// </summary>
        /// <param name="openSlots"></param>
        /// <returns></returns>
        public ObservableCollection<BaseStationForList> GetBaseStationsForSelector(string openSlots = "Has Open Charging Slots")
        {
            return GetBaseStations(b =>
                ((b.FreeChargingSlots > 0) && (openSlots == "Has Open Charging Slots")) || (openSlots == "Show All") || openSlots == null);

        }


        #endregion

        #region getserial numbers

        /// <summary>
        /// getting all the details from the class config to the BL.
        /// </summary>
        /// <returns></returns>
        public int GetNextSerialNumberForParcel()
        {
            return _dal.GetSerialNumber();
        }

        #endregion

        #region operation functions

        /// <summary>
        /// function that gets two locations and returns the distance between them.
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private double Distance(Models.Location l1, Models.Location l2)
        {
            const int r = 6371;

            var f1 = ConvertToRadians(l1.Latitude);
            var f2 = ConvertToRadians(l2.Latitude);

            var df = ConvertToRadians(l1.Latitude - l2.Latitude);
            var dl = ConvertToRadians(l1.Longitude - l2.Longitude);

            var a = Math.Sin(df / 2) * Math.Sin(df / 2) +
                    Math.Cos(f1) * Math.Cos(f2) *
                    Math.Sin(dl / 2) * Math.Sin(dl / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance.
            var d = r * c;

            return d;
        }

        /// <summary>
        /// function that converts radians to degrees for the distance calculation.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        /// <summary>
        /// the function gets idal location and transforms it into bal location by coping the values.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static Models.Location LocationTranslate(Location location)
        {
            return new Models.Location() { Longitude = location.Longitude, Latitude = location.Latitude };
        }

        /// <summary>
        /// the function gets bl location and transforms it into idal location by coping the values.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static Location LocationTranslate(Models.Location location)
        {
            var (longitude, latitude) = location;
            return new Location() { Longitude = longitude, Latitude = latitude };
        }

        /// <summary>
        /// simple converts the parcel to parcel for list.
        /// </summary>
        /// <param name="parcels"></param>
        /// <returns></returns>
        private IEnumerable<ParcelByCustomer> ParcelToParcelByCustomerList(IEnumerable<ParcelForList> parcels)
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
        /// the function that is being added to the Do_Work event.
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
                    //sends the drone to delivery.
                    SendToDelivery(worker, droneId);
                }
                // if there is no parcel to assign or the battery is too low.
                catch (UnableToAssignParcelToTheDroneException)
                {
                    //sends the drone to charge.
                    if (!(Math.Abs(GetDrone(droneId).Battery - 100) < Tolerance))
                    {
                        AutomaticCharging(worker, droneId, chargingLength);
                    }
                    else
                    {
                        //no more parcels for the drone to deliver.
                        //every 10 secs it is searching for a new parcel.
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
            //assign the parcel to the drone then picking it up then delivering it the receiver.
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

            var index = _drones.FindIndex(d => d.Id == droneId);

            for (var i = 1; i <= chargingLength; i++)
            {
                // Perform a time consuming operation and report progress.
                System.Threading.Thread.Sleep(50);
                if (UpdateBattery(index))
                    worker.ReportProgress(0);
                else
                    break;
            }
            UnChargeDrone(droneId);
        }

        /// <summary>
        /// updates the battery by adding ine to it every time.
        /// </summary>
        /// <param name="droneIndex"></param>
        /// <returns></returns>
        private bool UpdateBattery(int droneIndex)
        {

            if (_drones[droneIndex].Battery < 100)
            {
                ++_drones[droneIndex].Battery;
                return true;
            }
            else
            {
                _drones[droneIndex].Battery = 100;
                return false;
            }
        }

        #endregion

    } //END BL class

} //end IBAL