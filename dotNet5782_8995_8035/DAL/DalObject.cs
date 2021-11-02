
//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program is a public class for the namespace "DalObjects", that contains all the basic functions that can be done with the data structures.


using DalObject;
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
        public void AddBaseStation(int idNumber, string name, double longtude, double lattitude, int chargeslots)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            foreach(IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            {
                if (idNumber == baseStation.id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.baseStations.Add(new IDAL.DO.BaseStation() { id = idNumber ,name = name , longitude = longtude , lattitude = lattitude , chargeSlots = chargeslots });

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
                if (idNumber == drone.id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.drones.Add(new IDAL.DO.Drone() { id = idNumber, model = model, MaxWeight = weight});

        }

        /// <summary>
        /// the function creates new customer acording to given specs, and adding it to the array.
        /// while updating the config class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="longtitude"></param>
        /// <param name="lattitude"></param>
        public void AddCustumer(int idNumber ,string name, string phone, double longtitude, double lattitude)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            foreach (IDAL.DO.Customer customer in DataSource.customers)
            {
                if (idNumber == customer.id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.customers.Add(new IDAL.DO.Customer() { id = idNumber, name = name, phone = phone, lattitude = lattitude, longitude = longtitude });
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
        public void AddParcel(int idNumber , int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime reqested, DateTime scheduled)
        {
            //checking that the number is not already in the list, in witch case exeption will be thrown.
            foreach (IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if (idNumber == parcel.id) throw new IDAL.DO.SerialNumberExistsExceptions(idNumber);
            }

            //adding the base station to the list after no matching serial numbers was fuond.
            DataSource.parcels.Add(new IDAL.DO.Parcel() { id = idNumber, senderId = senderId, targetId = targetId, Weight = weight, priority = praiority, droneId = droneId, requested = reqested, scheduled = scheduled });
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
                if (parcel.id == parcelId)
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
                if (drone.id == droneId) isNameExists = true;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "drone");


            //updating the number of the drone in the DroneId field of the parcel to have the updated droneId number.
            //according to the number that was found while looking for an exception.  
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.droneId = droneId;
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
                if (parcel.id == parcelId) { 
                    isNameExists = true;
                    break;
                }
                ++parcelIndex;

            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(parcelId, "parcel");

            //updating the time of pickedUp field to be now.
            //according to the number that was found while looking for an exception.  
            IDAL.DO.Parcel newParcel = DataSource.parcels[parcelIndex];
            newParcel.pickedUp = DateTime.Now;
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
                if (parcel.id == parcelId)
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
            newParcel.delivered = DateTime.Now;
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
                if (drone.id == droneId)
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
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge() { droneId = droneId, stationId = baseStationId };
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
                if (drone.id == droneId)
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
                if(droneCharge.droneId == droneId)
                {
                    isNameExists = true;
                    break;
                }
                ++chargeIndex; 
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "chargeDrone");

            //not relevent
            ////update the status of the drone to be FIXING.    
            //IDAL.DO.Drone newDrone = DataSource.drones[droneIndex];
            ////newDrone.Status = IDAL.DO.DroneStatuses.FREE;
            ////newDrone.battery = 100;
            //DataSource.drones[droneIndex] = newDrone;

            //updates the number of free charging slots int he base station.
            //finds the index of the station and update when finds, no need for excption search, because the station exists for sure.
            for (int i = 0; i < DataSource.baseStations.Count; i++)
            {
                if(DataSource.baseStations[i].id == DataSource.charges[chargeIndex].stationId)
                {
                    IDAL.DO.BaseStation baseStation = DataSource.baseStations[DataSource.charges[chargeIndex].stationId];
                    ++baseStation.chargeSlots;
                    DataSource.baseStations[DataSource.charges[chargeIndex].stationId] = baseStation;
                    break;
                }
            }

            //removing the cahrge from the list.
            DataSource.charges.Remove(DataSource.charges[chargeIndex]);

        }

        ////***show options***/////
       
        /// <summary>
        /// the function recives an ID number of  a base station and returns the relevent station.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public IDAL.DO.BaseStation GetBaseStation(int baseStationId)
        {
            int baseStationIndex = 0;

            //checking if the numbers of base station that was provided exist in the database or not. if not an excption will be thrown.
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

            return DataSource.baseStations[baseStationIndex];
        }

        /// <summary>
        /// the function recives an ID number of a drone and returns the relevent drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public IDAL.DO.Drone GetDrone(int droneId)
        {
            int droneIndex = 0;

            //checking if the numbers of drone that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach (IDAL.DO.Drone drone in DataSource.drones)
            {
                if (drone.id == droneId)
                {
                    isNameExists = true;
                    break;
                }
                ++droneIndex;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(droneId, "drone");

            return DataSource.drones[droneIndex];
        }

        /// <summary>
        /// the function recives an ID number of a customer and returns the relevent customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IDAL.DO.Customer GetCustomer(int customerId)
        {
            int customerIndex = 0;

            //checking if the numbers of drone that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach (IDAL.DO.Customer customer in DataSource.customers)
            {
                if (customer.id == customerId)
                {
                    isNameExists = true;
                    break;
                }
                ++customerIndex;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(customerId, "customer");

            return DataSource.customers[customerIndex];
        }


        /// <summary>
        /// the function recives an ID number of a parcel and returns the relevent parcel.
        /// </summary>
        /// <param name="parchesId"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel GetParcel(int parcelId)
        {
            int parcelIndex = 0;

            //checking if the numbers of drone that was provided exist in the database or not. if not an excption will be thrown.
            bool isNameExists = false;
            foreach (IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if (parcel.id == parcelId)
                {
                    isNameExists = true;
                    break;
                }
                ++parcelIndex;
            }
            if (!isNameExists) throw new IDAL.DO.SerialNumberWasNotFoundExceptions(parcelId, "parcel");

            return DataSource.parcels[parcelId];
        }

        ////***get Lists***////

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        public IEnumerable<IDAL.DO.BaseStation> GetBaseStations()
        {
            return DataSource.baseStations;
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Drone> GetDrones()
        {
            return DataSource.drones;
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        public IEnumerable<IDAL.DO.Customer> GetCustomers()
        {
            return DataSource.customers;
        }

        /// <summary>
        /// returns the array of the parcheses
        /// </summary>
        public IEnumerable<IDAL.DO.Parcel> GetParcheses()
        {
            return DataSource.parcels;
        }

        /// <summary>
        /// returns all the parcels that dont have a drone assigned to them.
        /// </summary>
        public IEnumerable<IDAL.DO.Parcel> GetParcelToDrone()
        {
            List<IDAL.DO.Parcel> parcelsToDrones = new List<IDAL.DO.Parcel>();

            foreach(IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if (parcel.droneId == -1)
                {
                    parcelsToDrones.Add(parcel);
                }
            }

            return parcelsToDrones;
        }

        /// <summary>
        /// returns all the base stations that have free charging slots
        /// </summary>
        public IEnumerable<IDAL.DO.BaseStation> GetFreeStations()
        {
            List<IDAL.DO.BaseStation> freeBaseStations = new List<IDAL.DO.BaseStation>();
            foreach(IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            {
                if (baseStation.chargeSlots > 0)
                {
                    freeBaseStations.Add(baseStation);
                }
            }
            return freeBaseStations;
        }


        /// <summary>
        /// the function gets a  weight category of a parcel.
        /// the function returns a list with all the drones that are capable of taking it.
        /// </summary>
        /// <param name="weight"></param>
        //public IEnumerable<IDAL.DO.Drone> GetDroneForParcel(IDAL.DO.WeightCategories weight)
        //{
        //    List<IDAL.DO.Drone> capableDrones = new List<IDAL.DO.Drone>();
        //    foreach(IDAL.DO.Drone drone in DataSource.drones)
        //    {
        //        if(drone.MaxWeight >= weight && drone.Status == IDAL.DO.DroneStatuses.FREE)
        //        {
        //            capableDrones.Add(drone);
        //        }
        //    }
        //    return capableDrones;
        //}

        /// <summary>
        /// the class returns a list with all the parcels that was been assined to a drone but wasn't deliverd.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetDronesToUpdate()
        {
            DateTime defaultDate = new DateTime();// needed for comperation
            Dictionary<int, int> parcelsWithoutDrone = new Dictionary<int, int>();
            foreach(IDAL.DO.Parcel parcel in DataSource.parcels)
            {
                if(parcel.droneId == -1 && parcel.delivered != defaultDate)
                {
                    parcelsWithoutDrone.Add(parcel.droneId , parcel.id);
                }
            }

            return parcelsWithoutDrone;
        }


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
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetClothestStation(int id)
        {
            int clothestStation = 0;
            double distanseSqered = 0 , minDistanceSqered = 1000000;

            IDAL.DO.Customer customer = GetCustomer(id);
            foreach(IDAL.DO.BaseStation baseStation in DataSource.baseStations)
            {
                distanseSqered = Math.Pow(baseStation.lattitude - customer.lattitude, 2) + Math.Pow(baseStation.longitude - customer.longitude, 2);
                if(distanseSqered < minDistanceSqered)
                {
                    minDistanceSqered = distanseSqered;
                    clothestStation = baseStation.id;
                }
            }

            return clothestStation;
            
        }
    }
}
