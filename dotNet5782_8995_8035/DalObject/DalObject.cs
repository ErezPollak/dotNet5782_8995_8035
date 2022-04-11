//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the student: Erez Polak 322768995


//the program is a public class for the namespace "DalObjects", that contains all the basic functions that can be done with the data structures.


using System;
using System.Collections.Generic;
using System.Linq;
using DalApi;
using DalFacade.Models;
using DO;


namespace Dal
{
    internal sealed class DalObject : IDal
    {

        #region Singalton

        /// <summary>
        /// private constructor for the dal class, for the singleton.
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// dal field intended to keep the instance of the bl that was created.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<DalObject> instance = new (() => new DalObject());
        
        /// <summary>
        /// needed for the return by  get function the instance property.
        /// </summary>
        public static DalObject Instance => instance.Value;

        #endregion

        # region add options

        /// <summary>
        /// the function creates new base station according to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="baseStation">to add baseStation</param>
        public bool AddBaseStation(BaseStation baseStation)
        {
            //checking that the number is not already in the list, in witch case exception will be thrown.
            if (DataSource.BaseStations.Any(b => b.Id == baseStation.Id))
                throw new IdAlreadyExistsException(baseStation.Id, "baseStation");

            //adding the base station to the list after no matching serial numbers was found.
            DataSource.BaseStations.Add(baseStation);

            return true;
        }

        /// <summary>
        /// the function creates new drone according to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="drone"></param>
        public bool AddDrone(Drone drone)
        {
            //checking that the number is not already in the list, in witch case exception will be thrown.
            if (DataSource.Drones.Exists(d => d.Id == drone.Id)) throw new IdAlreadyExistsException(drone.Id, "drone");

            //adding the base station to the list after no matching serial numbers was found.
            DataSource.Drones.Add(drone);

            return true;
        }

        /// <summary>
        /// the function gets a new customer and adding it to the list, if the id already exists an exception will be thrown.
        /// </summary>
        /// <param name="customer"></param>
        public bool AddCustomer(Customer customer)
        {
            //checking that the number is not already in the list, in witch case Exception will be thrown.

            if (DataSource.Customers.Any(c => c.Id == customer.Id))
                throw new IdAlreadyExistsException(customer.Id, "customer");

            //adding the base station to the list after no matching serial numbers was found.
            DataSource.Customers.Add(customer);

            return true;
        }

        /// <summary>
        ///  /// the function gets a new parcel and adding it to the list, if the id already exists an exception will be thrown.
        /// </summary>
        /// <param name="parcel"></param>
        public bool AddParcel(Parcel parcel)
        {
            //checking that the number is not already in the list, in witch case exception will be thrown.
            if (DataSource.Parcels.Any(p => p.Id == parcel.Id)) 
                throw new IdAlreadyExistsException(parcel.Id, "parcel");
            //adding the base station to the list after no matching serial numbers was found.
            DataSource.Parcels.Add(parcel);

            return true;
        }

        #endregion

        #region update options

        /// <summary>
        /// update the name of the drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateModelForADrone(int droneId, string model)
        {
            var index = DataSource.Drones.FindIndex(d => d.Id == droneId);

            if (index == -1) throw new IdDontExistsException(droneId, "drone");

            var drone = DataSource.Drones[index];

            drone.Model = model;

            DataSource.Drones[index] = drone;

            return true;
        }

        /// <summary>
        /// updates the properties of the station
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="name"></param>
        /// <param name="slots"></param>
        /// <returns></returns>
        public bool UpdateBaseStation(int baseStationId, string name, int slots)

        {
            var index = DataSource.BaseStations.FindIndex(b => b.Id == baseStationId);

            if (index == -1) throw new IdDontExistsException(baseStationId, "baseStation");

            var baseStation = DataSource.BaseStations[index];

            baseStation.Name = name;

            baseStation.ChargeSlots = slots;

            DataSource.BaseStations[index] = baseStation;

            return true;
        }

        /// <summary>
        /// update the properties of the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool UpdateCustomer(int customerId, string name, string phone)
        {
            var index = DataSource.Customers.FindIndex(d => d.Id == customerId);

            if (index == -1) throw new IdDontExistsException();

            var customer = DataSource.Customers[index];

            customer.Name = name;

            customer.Phone = phone;

            DataSource.Customers[index] = customer;

            return true;
        }

        /// <summary>
        ///the function is giving the parcel the number of the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public bool AssignDroneToParcel(int parcelId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterating over the list again.
            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an exception will be thrown.

            var parcelIndex = DataSource.Parcels.FindIndex(p => parcelId == p.Id);
            if (parcelIndex == -1) //not found parcel in the list
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!DataSource.Drones.Exists(d => droneId == d.Id)) //check if the drone is exists in data source.
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the number of the drone in the DroneId field of the parcel to have the updated droneId number.
            //according to the number that was found while looking for an exception.  
            var updatedParcel = DataSource.Parcels[parcelIndex];
            updatedParcel.AssignedTime = DateTime.Now;
            updatedParcel.DroneId = droneId;
            DataSource.Parcels[parcelIndex] = updatedParcel;

            return true;
        }

        ///  <summary>
        /// updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        ///  </summary>
        ///  <param name="parcelId"></param>
        ///  <param name="droneId"></param>
        public bool PickingUpParcel(int parcelId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterating over the list again.

            //checking if the number of parcel that was provided exist in the database or not. if not an exception will be thrown.
            var parcelIndex = DataSource.Parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!DataSource.Drones.Exists(d => d.Id == droneId)) //check if the drone in the list
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the time of pickedUp field to be now.
            //according to the number that was found while looking for an exception.  
            var newParcel = DataSource.Parcels[parcelIndex];
            newParcel.PickedUpTime = DateTime.Now;
            newParcel.DroneId = droneId;
            DataSource.Parcels[parcelIndex] = newParcel;

            return true;
        }

        /// <summary>
        ///updating the time of delivering in the parcel, and changing the status of the drone to free.
        /// </summary>
        /// <param name="parcelId"></param>
        public bool DeliveringParcel(int parcelId)
        {
            //checking if the number of parcel that was provided exist in the database or not. if not an exception will be thrown.
            var parcelIndex = DataSource.Parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            //updating the time of delivered field to be now.
            //according to the number that was found while looking for an exception.  
            var newParcel = DataSource.Parcels[parcelIndex];
            newParcel.DroneId = 0;
            newParcel.DeliveryTime = DateTime.Now;
            DataSource.Parcels[parcelIndex] = newParcel;

            return true;
        }

        /// <summary>
        /// creating a droneCharge and charging the drone.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="droneId"></param>
        public bool ChargeDrone(int baseStationId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterating over the list again.
            var baseStationIndex = DataSource.BaseStations.FindIndex(b => b.Id == baseStationId);
            if (baseStationIndex == -1)
                throw new IdDontExistsException(baseStationId, "base station");


            if (DataSource.Drones.All(d => d.Id != droneId))
                throw new IdDontExistsException(droneId, "drone");


            //update the number of the free charge slots at the base station to be one less.
            var newBaseStation = DataSource.BaseStations[baseStationIndex];
            --newBaseStation.ChargeSlots;
            DataSource.BaseStations[baseStationIndex] = newBaseStation;

            //creating the charge drone ans adding it to the list of charges.
            var droneCharge = new DroneCharge() { DroneId = droneId, StationId = baseStationId, EntryIntoCharge = DateTime.Now.AddHours(-1) };
            DataSource.DroneCharges.Add(droneCharge);

            return true;
        }

        /// <summary>
        ///deleting the droneCharge from the list. 
        /// </summary>
        /// <param name="droneId"></param>
        public double UnChargeDrone(int droneId)
        {
            if (!DataSource.Drones.Exists(d => d.Id == droneId))
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //check if the name of the drone exist in the charges list. of not an exception will be thrown.
            var chargeIndex = DataSource.DroneCharges.FindIndex(cd => cd.DroneId == droneId);
            if (chargeIndex == -1)
            {
                throw new IdDontExistsException(droneId, "chargeDrone");
            }

            //updates the number of free charging slots int he base station.
            //finds the index of the station and update when finds, no need for exception search, because the station exists for sure.
            var baseStationsIndex =
                DataSource.BaseStations.FindIndex(b => b.Id == DataSource.DroneCharges[chargeIndex].StationId);
            if (baseStationsIndex == -1)
            {
                throw new IdDontExistsException(DataSource.DroneCharges[chargeIndex].StationId, "baseStations");
            }

            var entryIntoCharge = DataSource.DroneCharges[chargeIndex].EntryIntoCharge;
            if (entryIntoCharge == null) return 0;
            var ts = DateTime.Now.Subtract((DateTime)entryIntoCharge);
            var minutesInCharge = ts.Hours * 60 + ts.Minutes + (double)ts.Seconds / 60;

            var newBaseStation = DataSource.BaseStations[baseStationsIndex];
            newBaseStation.ChargeSlots++;
            DataSource.BaseStations[baseStationsIndex] = newBaseStation;

            //removing the charge from the list.
            DataSource.DroneCharges.Remove(DataSource.DroneCharges[chargeIndex]);

            return minutesInCharge;

        }

        #endregion

        #region show oobject options

        /// <summary>
        /// the function receives an ID number of  a base station and returns the relevant station.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public BaseStation GetBaseStation(int baseStationId)
        {
            var baseStationIndex = DataSource.BaseStations.FindIndex(b => b.Id == baseStationId);

            if (baseStationIndex == -1) throw new IdDontExistsException(baseStationId, "base station");

            return DataSource.BaseStations[baseStationIndex];
        }

        /// <summary>
        /// the function receives an ID number of a drone and returns the relevant drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            var droneIndex = DataSource.Drones.FindIndex(d => d.Id == droneId);

            if (droneIndex == -1) throw new IdDontExistsException(droneId, "drone");

            return DataSource.Drones[droneIndex];
        }

        /// <summary>
        /// the function receives an ID number of a customer and returns the relevant customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            var customerIndex = DataSource.Customers.FindIndex(c => c.Id == customerId);

            if (customerIndex == -1) throw new IdDontExistsException(customerId, "customer");

            return DataSource.Customers[customerIndex];
        }

        /// <summary>
        /// the function receives an ID number of a parcel and returns the relevant parcel.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            var parcelIndex = DataSource.Parcels.FindIndex(p => p.Id == parcelId);

            if (parcelIndex == -1) throw new IdDontExistsException(parcelId, "parcel");

            return DataSource.Parcels[parcelIndex];
        }

        #endregion

        # region get lists option

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> f)
        {
            return DataSource.BaseStations.Where(b => f(b)); //its like what you done just shorter.
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Drone> GetDrones(Predicate<Drone> f)
        {
            return DataSource.Drones.Where(d => f(d));
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> f)
        {
            return DataSource.Customers.Where(c => f(c));
        }

        /// <summary>
        /// returns the array of the purchases
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> f)
        {
            return DataSource.Parcels.Where(p => f(p));
        }

        public IEnumerable<DroneCharge> GetChargeDrones(Predicate<DroneCharge> f)
        {
            return DataSource.DroneCharges.Where(dc => f(dc));
        }

        #endregion

        #region operational functions

        /// <summary>
        /// returns an array with the information of charging drones.
        /// </summary>
        /// <returns></returns>
        public double[] ElectricityUse()
        {
            double[] chargingInformation =
            {
                DataSource.Config.Free, DataSource.Config.Light, DataSource.Config.Middle, DataSource.Config.Heavy,
                DataSource.Config.ChargingSpeed
            };
            return chargingInformation;
        }

        /// <summary>
        ///  the function gets an id of drone or a customer and returns the station that is closest to this drone or customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int GetClosestStation(int customerId)
        {
            var closestStation = 0;
            double minDistanceSquared = 1000000;

            var customer = GetCustomers(c => c.Id == customerId).First();
            foreach (var baseStation in DataSource.BaseStations)
            {
                var distanceSquared = Math.Pow(baseStation.Location.Latitude - customer.Location.Latitude, 2) +
                                     Math.Pow(baseStation.Location.Longitude - customer.Location.Longitude, 2);
                if (!(distanceSquared < minDistanceSquared)) continue;
                minDistanceSquared = distanceSquared;
                closestStation = baseStation.Id;
            }

            return closestStation;
        }

        public int GetSerialNumber()
        {
            return ++DataSource.Config.SerialNumber;
        }

        #endregion
    }
}