
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
        public IDAL.DO.DroneCharge[] charges;

        //ctor
        public DalObject()
        {
            DataSource.Initialize();

            charges = new IDAL.DO.DroneCharge[10];

            //initilaze the drone chrges to contain non realistic drone ids to represent that they are empty.
            for (int i = 0; i < 10; i++)
            {
                charges[i].droneId = -1;
            }

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
            DataSource.baseStations[DataSource.Config.freeBaseStation] = new IDAL.DO.BaseStation() { id = DataSource.Config.freeBaseStation, name = name, longitude = longtude, lattitude = lattitude, chargeSlots = chargeslots };
            ++DataSource.Config.freeBaseStation;
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
            DataSource.drones[DataSource.Config.freeDrone] = new IDAL.DO.Drone() { id = DataSource.Config.freeDrone, model = model, MaxWeight = weight, Status = status, battery = battery };
            ++DataSource.Config.freeDrone;
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
            DataSource.customers[DataSource.Config.freeCustumer] = new IDAL.DO.Customer() { id = DataSource.Config.freeCustumer, name = name, phone = phone, longitude = longtitude, lattitude = lattitude };
            ++DataSource.Config.freeCustumer;
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
            DataSource.parcheses[DataSource.Config.freePerches] = new IDAL.DO.Parcel() { id = DataSource.Config.freePerches, senderId = senderId, targetId = targetId, Weight = weight, priority = praiority, droneId = droneId, requested = reqested, scheduled = scheduled, delivered = new DateTime(), pickedUp = new DateTime() };
            ++DataSource.Config.freePerches;
        }

        ////***update options***/////


        /// <summary>
        ///the function is givig the parcel the number of the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void UpdateDroneForAParcel(int parcelId, int droneId)
        {
            DataSource.parcheses[parcelId].droneId = droneId;
        }

        /// <summary>
        ///updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickingUpParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].pickedUp = DateTime.Now;
            DataSource.drones[DataSource.parcheses[parcelId].droneId].Status = IDAL.DO.DroneStatuses.DELIVERY;
        }

       /// <summary>
       ///updating the time of delivering in the parcel, and changing the status of the drone to free.
       /// </summary>
       /// <param name="parcelId"></param>
        public void DeliveringParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].delivered = DateTime.Now;
            DataSource.drones[DataSource.parcheses[parcelId].droneId].Status = IDAL.DO.DroneStatuses.FREE;
        }



        /// <summary>
        /// creating a droneCharge and chrging the drone.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="droneId"></param>
        public void ChargeDrone(int baseStationId, int droneId)
        {
            DataSource.drones[droneId].Status = IDAL.DO.DroneStatuses.FIXING;
            --DataSource.baseStations[baseStationId].chargeSlots;
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge() { droneId = droneId, stationId = baseStationId };

            for (int i = 0; i < 10; i++)
            {
                if(charges[i].droneId == -1)
                {
                    charges[i] = droneCharge;
                }
            }
        }

        /// <summary>
        ///deleting the droneCharge from the array. 
        /// </summary>
        /// <param name="droneId"></param>
        public void UnChargeDrone(int droneId)
        {
            DataSource.drones[droneId].battery = 100;
            DataSource.drones[droneId].Status = IDAL.DO.DroneStatuses.FREE;

            for (int i = 0; i < 10; i++)
            {
                if(charges[i].droneId == droneId)
                {
                    charges[i].droneId = -1;
                    ++DataSource.baseStations[charges[i].stationId].chargeSlots;
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
            return DataSource.parcheses[parchesId];
        }

        ////***get Lists***////

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        public IDAL.DO.BaseStation[] GetBaseStations()
        {
            return DataSource.baseStations;
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <returns></returns>
        public IDAL.DO.Drone[] GetDrones()
        {
            return DataSource.drones;
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        public IDAL.DO.Customer[] GetCustomers()
        {
            return DataSource.customers;
        }

        /// <summary>
        /// returns the array of the parcheses
        /// </summary>
        public IDAL.DO.Parcel[] GetParcheses()
        {
            return DataSource.parcheses;
        }

        /// <summary>
        /// returns all the parcels that dont have a drone assigned to them.
        /// </summary>
        public List<IDAL.DO.Parcel> GetParcelToDrone()
        {
            List<IDAL.DO.Parcel> parcelsToDrones = new List<IDAL.DO.Parcel>();

            for (int i = 0; i < DataSource.Config.freePerches; i++)
            {
                if (DataSource.parcheses[i].droneId == -1)
                {
                    parcelsToDrones.Add(DataSource.parcheses[i]);
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
            for (int i = 0; i < DataSource.Config.freeBaseStation; i++)
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
            for (int i = 0; i < DataSource.Config.freeDrone; i++)
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
