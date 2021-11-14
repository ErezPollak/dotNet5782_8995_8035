﻿
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
        /// <param name="name"></param>
        /// <param name="longtude"></param>
        /// <param name="lattitude"></param>
        /// <param name="chargeslots"></param>
        public void AddBaseStation(int idNumber, string name, IDAL.DO.Location location , int chargeslots)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            foreach(IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            {
                if (idNumber == baseStation.id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.baseStations.Add(new IDAL.DO.BaseStation() { id = idNumber ,name = name , Location = location , chargeSlots = chargeslots });

        }

        /// <summary>
        /// the function creates new drone acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <param name="battery"></param>
        public void AddDrone(int idNumber , string model, IDAL.DO.WeightCategories weight)
        {
          
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            foreach (IDAL.DO.Drone drone in DataSource.drones)
            {
                if (idNumber == drone.Id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.drones.Add(new IDAL.DO.Drone() { Id = idNumber, Model = model, MaxWeight = weight});

        }

        /// <summary>
        /// the function creates new customer acording to given specs, and adding it to the array.
        /// while updating the config class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="longtitude"></param>
        /// <param name="lattitude"></param>
        public void AddCustumer(int idNumber ,string name, string phone, IDAL.DO.Location location)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            foreach (IDAL.DO.Customer customer in DataSource.customers)
            {
                if (idNumber == customer.Id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.customers.Add(new IDAL.DO.Customer() { Id = idNumber, Name = name, Phone = phone, Location = location });
        }

        /// <summary>
        /// the function creates new parcel acording to given specs, and adding it to the array.
        /// while updating the config class.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="targetId"></param>
        /// <param name="weight"></param>
        /// <param name="praiority"></param>
        /// <param name="droneId"></param>
        /// <param name="reqested"></param>
        /// <param name="scheduled"></param>
        public void AddParcel(int idNumber , int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime? reqested, DateTime? scheduled)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            foreach (IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if (idNumber == parcel.Id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.parcels.Add(new IDAL.DO.Parcel() { Id = idNumber, SenderId = senderId, TargetId = targetId, Weight = weight, Priority = praiority, DroneId = droneId, Requested = reqested, DeliveryTime = scheduled });
        }

        ////***update options***/////


        /// <summary>
        ///the function is givig the parcel the number of the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void UpdateDroneForAParcel(int parcelId, int droneId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            int parcelIndex = 0;

            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach(IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if (parcel.Id == parcelId)
                {
                    isNameExists = true;
                    break;
                }
                ++parcelIndex;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(parcelId, "parcel");

            isNameExists = false;
            foreach (IDAL.DO.Drone drone in DataSource.drones)
            {
                if (drone.Id == droneId)
                {
                    isNameExists = true;
                    break;
                }
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "drone");


            //updating the number of the drone in the DroneId field of the parcel to have the updated droneId number.
            //according to the number that was found while looking for an exception.  
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.DroneId = droneId;
            DataSource.parcels[parcelIndex] = newParcel;
                
        }

        /// <summary>
        ///updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickingUpParcel(int parcelId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            int parcelIndex = 0;

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach (IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if (parcel.Id == parcelId) { 
                    isNameExists = true;
                    break;
                }
                ++parcelIndex;

            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(parcelId, "parcel");

            //updating the time of pickedUp field to be now.
            //according to the number that was found while looking for an exception.  
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.PickedUp = DateTime.Now;
            DataSource.parcels[parcelIndex] = newParcel;
        }

       /// <summary>
       ///updating the time of delivering in the parcel, and changing the status of the drone to free.
       /// </summary>
       /// <param name="parcelId"></param>
        public void DeliveringParcel(int parcelId)
        {
            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            int parcelIndex = 0;

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach (IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if (parcel.Id == parcelId)
                {
                    isNameExists = true;
                    break;
                }
                ++parcelIndex;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(parcelId, "parcel");

            //updating the time of delivered field to be now.
            //according to the number that was found while looking for an exception.  
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.AcceptedTime = DateTime.Now;
            DataSource.parcels[parcelIndex] = newParcel;
        }

        /// <summary>
        /// creating a droneCharge and chrging the drone.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="droneId"></param>
        public void ChargeDrone(int baseStationId, int droneId)
        {

            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            int baseStationIndex = 0; //droneIndex = 0;

            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach (IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            {
                if (baseStation.id == baseStationId)
                {
                    isNameExists = true;
                    break;
                }
                ++baseStationIndex;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(baseStationId, "base station");

            isNameExists = false;
            foreach (IDAL.DO.Drone drone in DataSource.drones)
            {
                if (drone.Id == droneId)
                {
                    isNameExists = true;
                    break;
                }
                //++droneIndex;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "drone");

            //update the status of the drone to be FIXING.
            //IDAL.DO.Drone newDrone = DataSource.drones[droneIndex];
            //newDrone.Status = IDAL.DO.DroneStatuses.FIXING;
            //DataSource.drones[droneIndex] = newDrone;
            
            //update the number of the free charge slots at the base station to be one less.
            IDAL.DO.BaseStation newBaseStation = DataSource.baseStations[baseStationIndex];
             --newBaseStation.chargeSlots;
             DataSource.baseStations[baseStationIndex] = newBaseStation;
           
            //creating the charge drone ans adding it to the list of charges.
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge() { DroneId = droneId, StationId = baseStationId };
            DataSource.charges.Add(droneCharge);
        }

        /// <summary>
        ///deleting the droneCharge from the list. 
        /// </summary>
        /// <param name="droneId"></param>
        public void UnChargeDrone(int droneId)
        {
            bool isNameExists = false;
            //int droneIndex = 0; not relevent
            foreach (IDAL.DO.Drone drone in DataSource.drones)
            {
                if (drone.Id == droneId)
                {
                    isNameExists = true;
                    break;
                }
                //++droneIndex; not relevent
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "drone");

            //check if the name of the drone exist in the charges list. of not an excption will be thrown.
            int chargeIndex = 0;
            isNameExists = false;
            foreach(IDAL.DO.DroneCharge droneCharge in DataSource.charges)
            {
                if(droneCharge.DroneId == droneId)
                {
                    isNameExists = true;
                    break;
                }
                ++chargeIndex; 
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "chargeDrone");

            //updates the number of free charging slots int he base station.
            //finds the index of the station and update when finds, no need for excption search, because the station exists for sure.
            for (int i = 0; i < DataSource.baseStations.Count; i++)
            {
                if(DataSource.baseStations[i].id == DataSource.charges[chargeIndex].StationId)
                {
                    IDAL.DO.BaseStation baseStation = DataSource.baseStations[DataSource.charges[chargeIndex].StationId];
                    ++baseStation.chargeSlots;
                    DataSource.baseStations[DataSource.charges[chargeIndex].StationId] = baseStation;
                    break;
                }
            }

            //removing the cahrge from the list.
            DataSource.charges.Remove(DataSource.charges[chargeIndex]);

        }

        ////***show options***/////
       
        ///// <summary>
        ///// the function recives an ID number of  a base station and returns the relevent station.
        ///// </summary>
        ///// <param name="baseStationId"></param>
        ///// <returns></returns>
        //public IDAL.DO.BaseStation GetBaseStation(int baseStationId)
        //{
        //    int baseStationIndex = 0;

        //    //checking if the numbers of base station that was provided exist in the database or not. if not an excption will be thrown.
        //    bool isNameExists = false;
        //    foreach (IDAL.DO.BaseStation baseStation in DataSource.baseStations)
        //    {
        //        if (baseStation.id == baseStationId)
        //        {
        //            isNameExists = true;
        //            break;
        //        }
        //        ++baseStationIndex;
        //    }
        //    if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(baseStationId, "base station");

        //    return DataSource.baseStations[baseStationIndex];
        //}

        ///// <summary>
        ///// the function recives an ID number of a drone and returns the relevent drone.
        ///// </summary>
        ///// <param name="droneId"></param>
        ///// <returns></returns>
        //public IDAL.DO.Drone GetDrone(int droneId)
        //{
        //    int droneIndex = 0;

        //    //checking if the numbers of drone that was provided exist in the database or not. if not an excption will be thrown.
        //    bool isNameExists = false;
        //    foreach (IDAL.DO.Drone drone in DataSource.drones)
        //    {
        //        if (drone.Id == droneId)
        //        {
        //            isNameExists = true;
        //            break;
        //        }
        //        ++droneIndex;
        //    }
        //    if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "drone");

        //    return DataSource.drones[droneIndex];
        //}

        ///// <summary>
        ///// the function recives an ID number of a customer and returns the relevent customer.
        ///// </summary>
        ///// <param name="customerId"></param>
        ///// <returns></returns>
        //public IDAL.DO.Customer GetCustomer(int customerId)
        //{
        //    int customerIndex = 0;

        //    //checking if the numbers of drone that was provided exist in the database or not. if not an excption will be thrown.
        //    bool isNameExists = false;
        //    foreach (IDAL.DO.Customer customer in DataSource.customers)
        //    {
        //        if (customer.Id == customerId)
        //        {
        //            isNameExists = true;
        //            break;
        //        }
        //        ++customerIndex;
        //    }
        //    if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(customerId, "customer");

        //    return DataSource.customers[customerIndex];
        //}


        ///// <summary>
        ///// the function recives an ID number of a parcel and returns the relevent parcel.
        ///// </summary>
        ///// <param name="parchesId"></param>
        ///// <returns></returns>
        //public IDAL.DO.Parcel GetParcel(int parcelId)
        //{
        //    int parcelIndex = 0;

        //    //checking if the numbers of drone that was provided exist in the database or not. if not an excption will be thrown.
        //    bool isNameExists = false;
        //    foreach (IDAL.DO.Parcel parcel in DataSource.parcels)
        //    {
        //        if (parcel.Id == parcelId)
        //        {
        //            isNameExists = true;
        //            break;
        //        }
        //        ++parcelIndex;
        //    }
        //    if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(parcelId, "parcel");

        //    return DataSource.parcels[parcelId];
        //}

        ////***get Lists***////

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        public IEnumerable<IDAL.DO.BaseStation> GetBaseStations(Predicate<IDAL.DO.BaseStation> f)
        {
            List<IDAL.DO.BaseStation> list = new List<BaseStation>();

            DataSource.baseStations.ForEach(delegate (IDAL.DO.BaseStation b) { if (f(b)) { list.Add(b); } });

            //foreach (IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            //{
            //    if (f(baseStation))
            //    {
            //        list.Add(baseStation);
            //    }
            //}

            return list;
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Drone> GetDrones(Predicate<IDAL.DO.Drone> f)
        {
            return DataSource.drones.ToList();
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        public IEnumerable<IDAL.DO.Customer> GetCustomers(Predicate<IDAL.DO.Customer> f)
        {
            return DataSource.customers.ToList();
        }

        /// <summary>
        /// returns the array of the parcheses
        /// </summary>
        public IEnumerable<IDAL.DO.Parcel> GetParcels(Predicate<IDAL.DO.Parcel> f)
        {
            return DataSource.parcels.ToList();
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
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetClothestStation(int customerId)
        {
            int clothestStation = 0;
            double distanseSqered = 0 , minDistanceSqered = 1000000;

            IDAL.DO.Customer customer = GetCustomers(c =>  c.Id == customerId).First();
            foreach(IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            {
                distanseSqered = Math.Pow(baseStation.Location.Lattitude - customer.Location.Lattitude, 2) + Math.Pow(baseStation.Location.Longitude - customer.Location.Longitude, 2);
                if(distanseSqered < minDistanceSqered)
                {
                    minDistanceSqered = distanseSqered;
                    clothestStation = baseStation.id;
                }
            }

            return clothestStation;
            
        }

        public int GetBaseStationsNumber()
        {
            return DataSource.baseStations.Count;
        }

        public int GetBaseStationId(int index)
        {
            return DataSource.baseStations[index].id;
        }

        //public IEnumerable<IDAL.DO.Parcel> GetProvidedParcels()
        //{
        //    List<IDAL.DO.Parcel> providedParcels = new List<IDAL.DO.Parcel>();
        //    foreach(IDAL.DO.Parcel parcel in DataSource.parcels)
        //    {
        //        if(parcel.AcceptedTime != null)
        //        {
        //            providedParcels.Add(parcel);
        //        }
        //    }
        //    return providedParcels;

        //}

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
