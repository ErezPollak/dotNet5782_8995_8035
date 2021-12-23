//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035


//the program is a public class for the namespace "DalObjects", that contains all the basic functions that can be done with the data structures.


using DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal
{
    internal class DalObject : DalApi.IDal
    {

        #region Singalton

        /// <summary>
        /// private constructor for the dal class, for the singalton.
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// dal field intended to keep the insstance of the bl that was created.
        /// </summary>
        private static DalObject instance = null;

        /// <summary>
        /// an object intanded to lock the code of creating the new DAL so it does not happen twice.
        /// </summary>
        private static readonly object _lock = new object();

        // <summary>
        /// the function the creates new instance of DAL only if it doesn't exists already.
        /// </summary>
        /// <returns></returns>
        public static DalObject GetInstance()
        {
            if(instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new DalObject();
                    }
                }
            }
            return instance;
        }

        #endregion

        # region add options

        /// <summary>
        /// the function creates new base station acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="baseStation">to add baseStation</param>
        public bool AddBaseStation(BaseStation baseStation)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            if (DataSource.baseStations.Any(b => b.Id == baseStation.Id))
                throw new IdAlreadyExistsException(baseStation.Id, "baseSattion");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.baseStations.Add(baseStation);

            return true;
        }

        /// <summary>
        /// the function creates new drone acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="drone"></param>
        public bool AddDrone(Drone drone)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            if (DataSource.drones.Exists(d => d.Id == drone.Id)) throw new IdAlreadyExistsException(drone.Id, "drone");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.drones.Add(drone);

            return true;
        }

        /// <summary>
        /// the function gets a new customer and adding it to the list, if the id alraedy exists an excption will be thrown.
        /// </summary>
        /// <param name="customer"></param>
        public bool AddCustumer(Customer customer)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.

            if (DataSource.customers.Any(c => c.Id == customer.Id))
                throw new IdAlreadyExistsException(customer.Id, "customer");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.customers.Add(customer);

            return true;
        }

        /// <summary>
        ///  /// the function gets a new patcel and adding it to the list, if the id alraedy exists an excption will be thrown.
        /// </summary>
        /// <param name="parcel"></param>
        public bool AddParcel(Parcel parcel)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            if (DataSource.parcels.Any(p => p.Id == parcel.Id)) throw new IdAlreadyExistsException(parcel.Id, "parcel");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.parcels.Add(parcel);

            return true;
        }

        #endregion

        #region update options


        public bool UpdateNameForADrone(int droneId, string model)
        {
            int index = DataSource.drones.FindIndex(d => d.Id == droneId);

            if (index == -1) throw new IdDontExistsException(droneId, "drone");

            Drone drone = DataSource.drones[index];

            drone.Model = model;

            DataSource.drones[index] = drone;

            return true;
        }

        public bool UpdateBaseStation(int baseStationId, string name, int slots)

        {
            int index = DataSource.baseStations.FindIndex(b => b.Id == baseStationId);

            if (index == -1) throw new IdDontExistsException(baseStationId, "baseStation");

            BaseStation baseStation = DataSource.baseStations[index];

            baseStation.Name = name;

            baseStation.ChargeSlots = slots;

            DataSource.baseStations[index] = baseStation;

            return true;
        }

        public bool UpdateCustomer(int customerId, string name, string phone)
        {
            int index = DataSource.customers.FindIndex(d => d.Id == customerId);

            if (index == -1) throw new IdDontExistsException();

            Customer customer = DataSource.customers[index];

            customer.Name = name;

            customer.Phone = phone;

            DataSource.customers[index] = customer;

            return true;
        }

        /// <summary>
        ///the function is givig the parcel the number of the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public bool UpdateDroneForAParcel(int parcelId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an excption will be thrown.

            int parcelIndex = DataSource.parcels.FindIndex(p => parcelId == p.Id);
            if (parcelIndex == -1) //not found parcel in the list
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!DataSource.drones.Exists(d => droneId == d.Id)) //check if the drone is exists in data sorse.
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the number of the drone in the DroneId field of the parcel to have the updated droneId number.
            //according to the number that was found while looking for an exception.  
            Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.DroneId = droneId;
            DataSource.parcels[parcelIndex] = newParcel;

            return true;
        }

        ///  <summary>
        /// updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        ///  </summary>
        ///  <param name="parcelId"></param>
        ///  <param name="droneId"></param>
        public bool PickingUpParcel(int parcelId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.
            int parcelIndex = DataSource.parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!DataSource.drones.Exists(d => d.Id == droneId)) //check if the dron in the list
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the time of pickedUp field to be now.
            //according to the number that was found while looking for an exception.  
            Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.PickedUpTime = DateTime.Now;
            newParcel.DroneId = droneId;
            DataSource.parcels[parcelIndex] = newParcel;

            return true;
        }

        /// <summary>
        ///updating the time of delivering in the parcel, and changing the status of the drone to free.
        /// </summary>
        /// <param name="parcelId"></param>
        public bool DeliveringParcel(int parcelId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.
            int parcelIndex = DataSource.parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            //updating the time of delivered field to be now.
            //according to the number that was found while looking for an exception.  
            Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.DeliveryTime = DateTime.Now;
            DataSource.parcels[parcelIndex] = newParcel;

            return true;
        }

        /// <summary>
        /// creating a droneCharge and chrging the drone.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="droneId"></param>
        public bool ChargeDrone(int baseStationId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            int baseStationIndex = DataSource.baseStations.FindIndex(b => b.Id == baseStationId);
            if (baseStationIndex == -1)
                throw new IdDontExistsException(baseStationId, "base station");


            if (DataSource.drones.All(d => d.Id != droneId))
                throw new IdDontExistsException(droneId, "drone");


            //update the number of the free charge slots at the base station to be one less.
            BaseStation newBaseStation = DataSource.baseStations[baseStationIndex];
            --newBaseStation.ChargeSlots;
            DataSource.baseStations[baseStationIndex] = newBaseStation;

            //creating the charge drone ans adding it to the list of charges.
            DroneCharge droneCharge = new DroneCharge() { DroneId = droneId, StationId = baseStationId, EntryIntoCharge = DateTime.Now.AddHours(-1) };
            DataSource.droneCharges.Add(droneCharge);

            return true;
        }

        /// <summary>
        ///deleting the droneCharge from the list. 
        /// </summary>
        /// <param name="droneId"></param>
        public double UnChargeDrone(int droneId)
        {
            if (!DataSource.drones.Exists(d => d.Id == droneId))
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //check if the name of the drone exist in the charges list. of not an excption will be thrown.
            int chargeIndex = DataSource.droneCharges.FindIndex(cd => cd.DroneId == droneId);
            if (chargeIndex == -1)
            {
                throw new IdDontExistsException(droneId, "chargeDrone");
            }

            //updates the number of free charging slots int he base station.
            //finds the index of the station and update when finds, no need for excption search, because the station exists for sure.
            int baseStationsIndex =
                DataSource.baseStations.FindIndex(b => b.Id == DataSource.droneCharges[chargeIndex].StationId);
            if (baseStationsIndex == -1)
            {
                throw new IdDontExistsException(DataSource.droneCharges[chargeIndex].StationId, "baseStations");
            }

            TimeSpan ts = DateTime.Now.Subtract((DateTime)DataSource.droneCharges[chargeIndex].EntryIntoCharge);
            double mintesInCharge = ts.Hours * 60 + ts.Minutes + (double)ts.Seconds / 60;

            BaseStation newBaseStation = DataSource.baseStations[baseStationsIndex];
            newBaseStation.ChargeSlots++;
            DataSource.baseStations[baseStationsIndex] = newBaseStation;

            //removing the cahrge from the list.
            DataSource.droneCharges.Remove(DataSource.droneCharges[chargeIndex]);

            return mintesInCharge;
        }

        #endregion

        #region show oobject options

        /// <summary>
        /// the function recives an ID number of  a base station and returns the relevent station.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public BaseStation GetBaseStation(int baseStationId)
        {
            int baseStationIndex = DataSource.baseStations.FindIndex(b => b.Id == baseStationId);

            if (baseStationIndex == -1) throw new IdDontExistsException(baseStationId, "base station");

            return DataSource.baseStations[baseStationIndex];
        }

        /// <summary>
        /// the function recives an ID number of a drone and returns the relevent drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            int droneIndex = DataSource.drones.FindIndex(d => d.Id == droneId);

            if (droneIndex == -1) throw new IdDontExistsException(droneId, "drone");

            return DataSource.drones[droneIndex];
        }

        /// <summary>
        /// the function recives an ID number of a customer and returns the relevent customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            int customerIndex = DataSource.customers.FindIndex(c => c.Id == customerId);

            if (customerIndex == -1) throw new IdDontExistsException(customerId, "customer");

            return DataSource.customers[customerIndex];
        }

        /// <summary>
        /// the function recives an ID number of a parcel and returns the relevent parcel.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            int parcelIndex = DataSource.parcels.FindIndex(p => p.Id == parcelId);

            if (parcelIndex == -1) throw new IdDontExistsException(parcelId, "parcel");

            return DataSource.parcels[parcelIndex];
        }

        #endregion

        # region get lists option

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> f)
        {
            return DataSource.baseStations.Where(b => f(b)); //its like what you done just shorter.
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Drone> GetDrones(Predicate<Drone> f)
        {
            return DataSource.drones.Where(d => f(d));
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> f)
        {
            return DataSource.customers.Where(c => f(c));
        }

        /// <summary>
        /// returns the array of the parcheses
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> f)
        {
            return DataSource.parcels.Where(p => f(p));
        }

        public IEnumerable<DroneCharge> GetChargeDrones(Predicate<DroneCharge> f)
        {
            return DataSource.droneCharges.Where(dc => f(dc));
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
                DataSource.Config.Free, DataSource.Config.Light, DataSource.Config.Middel, DataSource.Config.Heavy,
                DataSource.Config.ChargingSpeed
            };
            return chargingInformation;
        }

        /// <summary>
        ///  the function gets an id of drone or a customer and returns the station that is closst to this drone or customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int GetClosestStation(int customerId)
        {
            int closestStation = 0;
            double distanseSqered = 0, minDistanceSqered = 1000000;

            Customer customer = GetCustomers(c => c.Id == customerId).First();
            foreach (BaseStation baseStation in DataSource.baseStations)
            {
                distanseSqered = Math.Pow(baseStation.Location.Latitude - customer.Location.Latitude, 2) +
                                 Math.Pow(baseStation.Location.Longitude - customer.Location.Longitude, 2);
                if (distanseSqered < minDistanceSqered)
                {
                    minDistanceSqered = distanseSqered;
                    closestStation = baseStation.Id;
                }
            }

            return closestStation;
        }

        /// <summary>
        /// to delete....
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetBaseStationId(int index)
        {
            return DataSource.baseStations[index].Id;
        }

        public int GetSerialNumber()
        {
            return ++DataSource.Config.SerialNumber;
        }

        #endregion
    }
}