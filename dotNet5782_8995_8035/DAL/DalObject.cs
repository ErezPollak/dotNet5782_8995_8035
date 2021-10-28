
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
    public class DalObject
    {

        //an data structure to contain all the charging of the drones.
        public List<IDAL.DO.DroneCharge> charges;

        //ctor
        public DalObject()
        {
            DataSource.Initialize();

            charges = new List<IDAL.DO.DroneCharge>();

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
        public void AddBaseStation(string name, double longtude, double lattitude, int chargeslots)
        {
            //the code for arrays:
            //DataSource.baseStations[DataSource.Config.freeBaseStation] = new IDAL.DO.BaseStation() { id = DataSource.Config.freeBaseStation, name = name, longitude = longtude, lattitude = lattitude, chargeSlots = chargeslots };
            //++DataSource.Config.freeBaseStation;

            //the relevent code:
            DataSource.baseStations.Add(new IDAL.DO.BaseStation() { id = DataSource.baseStations.Count ,name = name , longitude = longtude , lattitude = lattitude , chargeSlots = chargeslots });

        }

        /// <summary>
        /// the function creates new drone acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <param name="battery"></param>
        public void AddDrone(string model, IDAL.DO.WeightCategories weight, IDAL.DO.DroneStatuses status, double battery)
        {
            //the code for arrays:
            //DataSource.drones[DataSource.Config.freeDrone] = new IDAL.DO.Drone() { id = DataSource.Config.freeDrone, model = model, MaxWeight = weight, Status = status, battery = battery };
            //++DataSource.Config.freeDrone;

            //the relevent code:
            DataSource.drones.Add(new IDAL.DO.Drone() { id = DataSource.drones.Count, model = model, MaxWeight = weight, battery = battery, Status = status });

        }

        /// <summary>
        /// the function creates new customer acording to given specs, and adding it to the array.
        /// while updating the config class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="longtitude"></param>
        /// <param name="lattitude"></param>
        public void AddCustumer(string name, string phone, double longtitude, double lattitude)
        {
            //the code for arrays:
            //DataSource.customers[DataSource.Config.freeCustumer] = new IDAL.DO.Customer() { id = DataSource.Config.freeCustumer, name = name, phone = phone, longitude = longtitude, lattitude = lattitude };
            //++DataSource.Config.freeCustumer;

            //the relevent code:
            DataSource.customers.Add(new IDAL.DO.Customer() { id = DataSource.customers.Count, name = name, phone = phone, lattitude = lattitude, longitude = longtitude });
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
        public void AddParcel(int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime reqested, DateTime scheduled)
        {
            //the code for arrays:
            //DataSource.parcheses[DataSource.Config.freePerches] = new IDAL.DO.Parcel() { id = DataSource.Config.freePerches, senderId = senderId, targetId = targetId, Weight = weight, priority = praiority, droneId = droneId, requested = reqested, scheduled = scheduled, delivered = new DateTime(), pickedUp = new DateTime() };
            //++DataSource.Config.freePerches;

            //the relevent code
            DataSource.parcels.Add(new IDAL.DO.Parcel() { id = DataSource.parcels.Count, senderId = senderId, targetId = targetId, Weight = weight, priority = praiority, droneId = droneId, requested = reqested, scheduled = scheduled });
        }

        ////***update options***/////


        /// <summary>
        ///the function is givig the parcel the number of the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void UpdateDroneForAParcel(int parcelId, int droneId)
        {
            //the irrelevent codr for arrays
            //DataSource.parcels[parcelId].droneId = droneId;
            
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if(DataSource.parcels[i].id == parcelId)
                {
                    IDAL.DO.Parcel parcel = DataSource.parcels[i];
                    parcel.droneId = droneId;
                    DataSource.parcels[i] = parcel;
                    break;
                }
            }
            
        }

        /// <summary>
        ///updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickingUpParcel(int parcelId)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].id == parcelId)
                {
                    IDAL.DO.Parcel parcel = DataSource.parcels[i];
                    parcel.pickedUp = DateTime.Now;
                    DataSource.parcels[i] = parcel;
                    break;
                }
            }
        }

       /// <summary>
       ///updating the time of delivering in the parcel, and changing the status of the drone to free.
       /// </summary>
       /// <param name="parcelId"></param>
        public void DeliveringParcel(int parcelId)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].id == parcelId)
                {
                    IDAL.DO.Parcel parcel = DataSource.parcels[i];
                    parcel.delivered = DateTime.Now;
                    DataSource.parcels[i] = parcel;
                    break;
                }
            }
        }



        /// <summary>
        /// creating a droneCharge and chrging the drone.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="droneId"></param>
        public void ChargeDrone(int baseStationId, int droneId)
        {
            //update the status of the drone to be FIXING.
            //DataSource.drones[droneId].Status = IDAL.DO.DroneStatuses.FIXING;
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].id == droneId)
                {
                    IDAL.DO.Drone drone = DataSource.drones[i];
                    drone.Status = IDAL.DO.DroneStatuses.FIXING;
                    DataSource.drones[i] = drone;
                    break;
                }
            }

            //update the number of the free charge slots at the base station to be one less.
            //--DataSource.baseStations[baseStationId].chargeSlots;
            for (int i = 0; i < DataSource.baseStations.Count; i++)
            {
                if (DataSource.baseStations[i].id == baseStationId)
                {
                    IDAL.DO.BaseStation baseStation = DataSource.baseStations[i];
                    --baseStation.chargeSlots;
                    DataSource.baseStations[i] = baseStation;
                    break;
                }
            }

            //creating the charge drone ans adding it to the list of charges.
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge() { droneId = droneId, stationId = baseStationId };
            charges.Add(droneCharge);
        }

        /// <summary>
        ///deleting the droneCharge from the array. 
        /// </summary>
        /// <param name="droneId"></param>
        public void UnChargeDrone(int droneId)
        {
            //to save the number of the base station to update the number of the free charging slots.
            int baseStationId = -1;

            //DataSource.drones[droneId].Status = IDAL.DO.DroneStatuses.FREE;
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].id == droneId)
                {
                    IDAL.DO.Drone drone = DataSource.drones[i];
                    drone.Status = IDAL.DO.DroneStatuses.FREE;
                    drone.battery = 100;
                    DataSource.drones[i] = drone;
                    break;
                }
            }

            //removing the relevent charging from the list.
            for (int i = 0; i < charges.Count; i++)
            {
                if(charges[i].droneId == droneId)
                {
                    if(baseStationId != -1)baseStationId = charges[i].stationId;
                    charges.Remove(charges[i]);
                }
            }


            //updates the number of free charging slots int he base station.
            //--DataSource.baseStations[baseStationId].chargeSlots;
            for (int i = 0; i < DataSource.baseStations.Count; i++)
            {
                if (DataSource.baseStations[i].id == baseStationId)
                {
                    IDAL.DO.BaseStation baseStation = DataSource.baseStations[i];
                    ++baseStation.chargeSlots;
                    DataSource.baseStations[i] = baseStation;
                    break;
                }
            }

        }

        ////***show options***/////
       
        /// <summary>
        /// the function recives an ID number of  a base station and returns the relevent station.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public IDAL.DO.BaseStation GetBaseStation(int baseStationId)
        {
            return DataSource.baseStations[baseStationId];
        }

        /// <summary>
        /// the function recives an ID number of a drone and returns the relevent drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public IDAL.DO.Drone GetDrone(int droneId)
        {
            return DataSource.drones[droneId];
        }

        /// <summary>
        /// the function recives an ID number of a customer and returns the relevent customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IDAL.DO.Customer GetCustomer(int customerId)
        {
            return DataSource.customers[customerId];
        }


        /// <summary>
        /// the function recives an ID number of a parcel and returns the relevent parcel.
        /// </summary>
        /// <param name="parchesId"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel GetParcel(int parchesId)
        {
            return DataSource.parcels[parchesId];
        }

        ////***get Lists***////

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        public List<IDAL.DO.BaseStation> GetBaseStations()
        {
            return DataSource.baseStations;
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <returns></returns>
        public List<IDAL.DO.Drone> GetDrones()
        {
            return DataSource.drones;
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        public List<IDAL.DO.Customer> GetCustomers()
        {
            return DataSource.customers;
        }

        /// <summary>
        /// returns the array of the parcheses
        /// </summary>
        public List<IDAL.DO.Parcel> GetParcheses()
        {
            return DataSource.parcels;
        }

        /// <summary>
        /// returns all the parcels that dont have a drone assigned to them.
        /// </summary>
        public List<IDAL.DO.Parcel> GetParcelToDrone()
        {
            List<IDAL.DO.Parcel> parcelsToDrones = new List<IDAL.DO.Parcel>();

            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].droneId == -1)
                {
                    parcelsToDrones.Add(DataSource.parcels[i]);
                }
            }

            return parcelsToDrones;
        }

        /// <summary>
        /// returns all the base stations that have free charging slots
        /// </summary>
        public List<IDAL.DO.BaseStation> GetFreeStations()
        {
            List<IDAL.DO.BaseStation> freeBaseStations = new List<IDAL.DO.BaseStation>();
            for (int i = 0; i < DataSource.baseStations.Count; i++)
            {
                if (DataSource.baseStations[i].chargeSlots > 0)
                {
                    freeBaseStations.Add(DataSource.baseStations[i]);
                }
            }
            return freeBaseStations;
        }


        /// <summary>
        /// the function gets a  weight category of a parcel.
        /// the function returns a list with all the drones that are capable of taking it.
        /// </summary>
        /// <param name="weight"></param>
        public List<IDAL.DO.Drone> GetDroneForParcel(IDAL.DO.WeightCategories weight)
        {
            List<IDAL.DO.Drone> capableDrones = new List<IDAL.DO.Drone>();
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if(DataSource.drones[i].MaxWeight >= weight && DataSource.drones[i].Status == IDAL.DO.DroneStatuses.FREE)
                {
                    capableDrones.Add(DataSource.drones[i]);
                }
            }
            return capableDrones;
        }

    }
}
