
//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program is a public class for the namespace "DalObjects", that contains all the basic functions that can be done with the data structures.


using DalObject;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject : IDAL.IDal
    {
        //ctor

        public DalObject()
        {
            DataSource.Initialize();
        }

        //////***add options***/////

        /// <summary>
        /// the function creates new base station acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="baseStation">to add baseStation</param>
        public bool AddBaseStation(IDAL.DO.BaseStation baseStation)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            if (DataSource.baseStations.Any(b => b.Id == baseStation.Id)) throw new IDAL.DO.IdAlreadyExistsException(baseStation.Id, "baseSattion");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.baseStations.Add(baseStation);

            return true;

        }

        /// <summary>
        /// the function creates new drone acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="drone"></param>
        public bool AddDrone(IDAL.DO.Drone drone)
        {

            //checking that the number is not already in the list, in witch case exeption will be thrown.
            if (DataSource.drones.Any(d => d.Id == drone.Id)) throw new IDAL.DO.IdAlreadyExistsException(drone.Id, "drone");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.drones.Add(drone);

            return true;

        }

        /// <summary>
        /// the function gets a new customer and adding it to the list, if the id alraedy exists an excption will be thrown.
        /// </summary>
        /// <param name="customer"></param>
        public bool AddCustumer(IDAL.DO.Customer customer)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.

            if (DataSource.customers.Any(c => c.Id == customer.Id)) throw new IDAL.DO.IdAlreadyExistsException(customer.Id, "customer");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.customers.Add(customer);

            return true;
        }

        /// <summary>
        ///  /// the function gets a new patcel and adding it to the list, if the id alraedy exists an excption will be thrown.
        /// </summary>
        /// <param name="parcel"></param>
        public bool AddParcel(IDAL.DO.Parcel parcel)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            if (DataSource.parcels.Any(p => p.Id == parcel.Id)) throw new IDAL.DO.IdAlreadyExistsException(parcel.Id, "parcel");

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.parcels.Add(parcel);

            return true;
        }

        ////***update options***/////


        public bool UpdateNameForADrone(int droneId, string model)
        {
            int index = DataSource.drones.FindIndex(d => (d.Id == droneId));

            if (index == -1) throw new IdDontExistsException(droneId, "drone");

            IDAL.DO.Drone drone = DataSource.drones[index];

            drone.Model = model;

            DataSource.drones[index] = drone;

            return true;
        }

        public bool UpdateBaseStation(int baseStationId, string name, int slots)

        {
            int index = DataSource.baseStations.FindIndex(b => (b.Id == baseStationId));

            if (index == -1) throw new IDAL.DO.IdDontExistsException(baseStationId, "baseStation");

            IDAL.DO.BaseStation baseStation = DataSource.baseStations[index];

            baseStation.Name = name;

            baseStation.ChargeSlots = slots;

            DataSource.baseStations[index] = baseStation;

            return true;
        }

        public bool UpdateCustomer(int customerID, string name, string phone)
        {
            int index = DataSource.customers.FindIndex(d => d.Id == customerID);

            if (index == -1) throw new IDAL.DO.IdDontExistsException();

            IDAL.DO.Customer customer = DataSource.customers[index];

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
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.DroneId = droneId;
            DataSource.parcels[parcelIndex] = newParcel;

            return true;

        }

        /// <summary>
        ///updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        /// </summary>
        /// <param name="parcelId"></param>
        public bool PickingUpParcel(int parcelId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.
            int parcelIndex = DataSource.parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcle");
            }

            if (!DataSource.drones.Exists(d => d.Id == droneId)) //check if the dron in the list
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the time of pickedUp field to be now.
            //according to the number that was found while looking for an exception.  
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
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
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
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
            if (baseStationIndex == -1) throw new IDAL.DO.IdDontExistsException(baseStationId, "base station");


            if (!DataSource.drones.Any(d => d.Id == droneId)) throw new IDAL.DO.IdDontExistsException(droneId, "drone");


            //update the number of the free charge slots at the base station to be one less.
            IDAL.DO.BaseStation newBaseStation = DataSource.baseStations[baseStationIndex];
            --newBaseStation.ChargeSlots;
            DataSource.baseStations[baseStationIndex] = newBaseStation;

            //creating the charge drone ans adding it to the list of charges.
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge() { DroneId = droneId, StationId = baseStationId };
            DataSource.droneCharges.Add(droneCharge);

            return true;

        }

        /// <summary>
        ///deleting the droneCharge from the list. 
        /// </summary>
        /// <param name="droneId"></param>
        public bool UnChargeDrone(int droneId)
        {
            if (!DataSource.drones.Exists(d => d.Id == droneId))
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //check if the name of the drone exist in the charges list. of not an excption will be thrown.
            int chargeIndex = DataSource.droneCharges.FindIndex(cd => cd.DroneId == droneId);
            if (chargeIndex == -1)
            {
                throw new IDAL.DO.IdDontExistsException(droneId, "chargeDrone");
            }

            //updates the number of free charging slots int he base station.
            //finds the index of the station and update when finds, no need for excption search, because the station exists for sure.
            int baseStationsIndex = DataSource.baseStations.FindIndex(b => b.Id == DataSource.droneCharges[chargeIndex].StationId);
            if (baseStationsIndex == -1)
            {
                throw new IdDontExistsException(DataSource.droneCharges[chargeIndex].StationId, "baseStations");
            }
            BaseStation newBaseStation = DataSource.baseStations[baseStationsIndex];
            newBaseStation.ChargeSlots++;
            DataSource.baseStations[baseStationsIndex] = newBaseStation;

            //removing the cahrge from the list.
            DataSource.droneCharges.Remove(DataSource.droneCharges[chargeIndex]);

            return true;

        }

        ////***show options***/////

        /// <summary>
        /// the function recives an ID number of  a base station and returns the relevent station.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public IDAL.DO.BaseStation GetBaseStation(int baseStationId)
        {
            int baseStationIndex = DataSource.baseStations.FindIndex(b => b.Id == baseStationId);

            if (baseStationIndex == -1) throw new IDAL.DO.IdDontExistsException(baseStationId, "base station");

            return DataSource.baseStations[baseStationIndex];
        }

        /// <summary>
        /// the function recives an ID number of a drone and returns the relevent drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public IDAL.DO.Drone GetDrone(int droneId)
        {
            int droneIndex = DataSource.drones.FindIndex(d => d.Id == droneId);

            if (droneIndex == -1) throw new IDAL.DO.IdDontExistsException(droneId, "drone");

            return DataSource.drones[droneIndex];
        }

        /// <summary>
        /// the function recives an ID number of a customer and returns the relevent customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IDAL.DO.Customer GetCustomer(int customerId)
        {
            int customerIndex = DataSource.customers.FindIndex(c => c.Id == customerId);

            if (customerIndex == -1) throw new IDAL.DO.IdDontExistsException(customerId, "customer");

            return DataSource.customers[customerIndex];
        }

        /// <summary>
        /// the function recives an ID number of a parcel and returns the relevent parcel.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel GetParcel(int parcelId)
        {
            int parcelIndex = DataSource.parcels.FindIndex(p => p.Id == parcelId);

            if (parcelIndex == -1) throw new IDAL.DO.IdDontExistsException(parcelId, "parcel");

            return DataSource.parcels[parcelId];
        }

        ////***get Lists***////

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> f)
        {
            // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            // return DataSource.baseStations.Where(b => f(b));    ///its like what you done jast shorter.
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            //List<BaseStation> list = new List<BaseStation>();
            //DataSource.baseStations.ForEach(b => { if (f(b)) list.Add(b); });
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

            List<BaseStation> list = new List<BaseStation>();
            DataSource.baseStations.ForEach(delegate (BaseStation b) { if (f(b)) { list.Add(b); } });

            return list;
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<IDAL.DO.Drone> GetDrones(Predicate<IDAL.DO.Drone> f)
        {
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            //return DataSource.drones.Where(d => f(d));
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

            List<IDAL.DO.Drone> drones = new List<IDAL.DO.Drone>();

            DataSource.drones.ForEach(delegate (IDAL.DO.Drone d) { if (f(d)) { drones.Add(d); } });

            return drones;
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<IDAL.DO.Customer> GetCustomers(Predicate<IDAL.DO.Customer> f)
        {
            List<IDAL.DO.Customer> customers = new List<IDAL.DO.Customer>();

            DataSource.customers.ForEach(delegate (IDAL.DO.Customer c) { if (f(c)) { customers.Add(c); } });

            return customers;
        }

        /// <summary>
        /// returns the array of the parcheses
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<IDAL.DO.Parcel> GetParcels(Predicate<IDAL.DO.Parcel> f)
        {
            List<IDAL.DO.Parcel> parcels = new List<Parcel>();

            DataSource.parcels.ForEach(delegate (IDAL.DO.Parcel p) { if (f(p)) { parcels.Add(p); } });

            return parcels;
        }

        public IEnumerable<DroneCharge> GetChargeDrones(Predicate<IDAL.DO.DroneCharge> f)
        {
            List<IDAL.DO.DroneCharge> droneCarges = new List<IDAL.DO.DroneCharge>();

            DataSource.droneCharges.ForEach(delegate (IDAL.DO.DroneCharge dc) { if (f(dc)) { droneCarges.Add(dc); } });

            return droneCarges;
        }


        ///// <summary>
        ///// returns all the parcels that dont have a drone assigned to them.
        ///// </summary>
        //public IEnumerable<IDAL.DO.Parcel> GetParcelToDrone()
        //{
        //    List<IDAL.DO.Parcel> parcelsToDrones = new List<IDAL.DO.Parcel>();

        //    foreach(IDAL.DO.Parcel parcel in DataSource.parcels)
        //    {
        //        if (parcel.DroneId == -1)
        //        {
        //            parcelsToDrones.Add(parcel);
        //        }
        //    }

        //    return parcelsToDrones;
        //}

        ///// <summary>
        ///// returns all the base stations that have free charging slots
        ///// </summary>
        //public IEnumerable<IDAL.DO.BaseStation> GetFreeStations()
        //{
        //    List<IDAL.DO.BaseStation> freeBaseStations = new List<IDAL.DO.BaseStation>();
        //    foreach(IDAL.DO.BaseStation baseStation in DataSource.baseStations)
        //    {
        //        if (baseStation.chargeSlots > 0)
        //        {
        //            freeBaseStations.Add(baseStation);
        //        }
        //    }
        //    return freeBaseStations;
        //}

        ///// <summary>
        ///// the function gets a  weight category of a parcel.
        ///// the function returns a list with all the drones that are capable of taking it.
        ///// </summary>
        ///// <param name="weight"></param>
        ////public IEnumerable<IDAL.DO.Drone> GetDroneForParcel(IDAL.DO.WeightCategories weight)
        ////{
        ////    List<IDAL.DO.Drone> capableDrones = new List<IDAL.DO.Drone>();
        ////    foreach(IDAL.DO.Drone drone in DataSource.drones)
        ////    {
        ////        if(drone.MaxWeight >= weight && drone.Status == IDAL.DO.DroneStatuses.FREE)
        ////        {
        ////            capableDrones.Add(drone);
        ////        }
        ////    }
        ////    return capableDrones;
        ////}

        ///// <summary>
        ///// the class returns a list with all the parcels that was been assined to a drone but wasn't deliverd.
        ///// </summary>
        ///// <returns></returns>
        //public List<IDAL.DO.Parcel> GetDronesToUpdate()
        //{

        //    List<IDAL.DO.Parcel> parcelsWithoutDrone = new List<IDAL.DO.Parcel>();
        //    foreach (IDAL.DO.Parcel parcel in DataSource.parcels)
        //    {
        //        if (parcel.DroneId != -1 && parcel.AcceptedTime == null)
        //        {
        //            parcelsWithoutDrone.Add(parcel);
        //        }
        //    }

        //    return parcelsWithoutDrone;
        //}


        //////////functions for BL.


        /// <summary>
        /// returns an array with the information of charging drones.
        /// </summary>
        /// <returns></returns>
        public double[] ElectricityUse()
        {
            double[] chargingInformation = { DataSource.Config.Free, DataSource.Config.Light, DataSource.Config.Middel, DataSource.Config.Heavy, DataSource.Config.ChargingSpeed };
            return chargingInformation;
        }

        /// <summary>
        ///  the function gets an id of drone or a customer and returns the station that is clothst to this drone or customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int GetClothestStation(int customerId)
        {
            int clothestStation = 0;
            double distanseSqered = 0, minDistanceSqered = 1000000;

            IDAL.DO.Customer customer = GetCustomers(c => c.Id == customerId).First();
            foreach (IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            {
                distanseSqered = Math.Pow(baseStation.Location.Lattitude - customer.Location.Lattitude, 2) + Math.Pow(baseStation.Location.Longitude - customer.Location.Longitude, 2);
                if (distanseSqered < minDistanceSqered)
                {
                    minDistanceSqered = distanseSqered;
                    clothestStation = baseStation.Id;
                }
            }

            return clothestStation;

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
            return ++DataSource.Config.serialNumber;
        }




        //public void SetNameForADrone(int droneId, string model)
        //{
        //    int index = DataSource.drones.FindIndex(d => (d.Id == droneId));

        //    IDAL.DO.Drone drone = DataSource.drones[index];

        //    drone.Model = model;

        //    DataSource.drones[index] = drone;
        //}

        //public int clothestStation(int customerId)
        //{
        //    double minDistance = distance(GetCustomer(customerId).Longitude , GetCustomer(customerId).Llattitude , this.GetBaseStations().ToList()[0].longitude , this.GetBaseStations().ToList()[0].lattitude);
        //    foreach(IDAL.DO.BaseStation baseStation in get)
        //}



        //private double distance(double x1, double y1, double x2, double y2)
        //{
        //    return Math.Sqrt(Math.Pow(x1 - x2, 2) + (Math.Pow(y1 - y2, 2)));
        //}

    }
}
