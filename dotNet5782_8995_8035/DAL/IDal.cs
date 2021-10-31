using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    interface IDal
    {
        /// <summary>
        /// the function creates new base station acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="longtude"></param>
        /// <param name="lattitude"></param>
        /// <param name="chargeslots"></param>
        public void AddBaseStation(int idNumber, string name, double longtude, double lattitude, int chargeslots);

        /// <summary>
        /// the function creates new drone acording to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <param name="battery"></param>
        public void AddDrone(int idNumber, string model, IDAL.DO.WeightCategories weight, IDAL.DO.DroneStatuses status, double battery);

        /// <summary>
        /// the function creates new customer acording to given specs, and adding it to the array.
        /// while updating the config class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="longtitude"></param>
        /// <param name="lattitude"></param>
        public void AddCustumer(int idNumber, string name, string phone, double longtitude, double lattitude);

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
        public void AddParcel(int idNumber, int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime reqested, DateTime scheduled);



        ////***update options***/////


        /// <summary>
        ///the function is givig the parcel the number of the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void UpdateDroneForAParcel(int parcelId, int droneId);

        /// <summary>
        ///updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickingUpParcel(int parcelId);

        /// <summary>
        ///updating the time of delivering in the parcel, and changing the status of the drone to free.
        /// </summary>
        /// <param name="parcelId"></param>
        public void DeliveringParcel(int parcelId);

        /// <summary>
        /// creating a droneCharge and chrging the drone.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="droneId"></param>
        public void ChargeDrone(int baseStationId, int droneId);

        /// <summary>
        ///deleting the droneCharge from the list. 
        /// </summary>
        /// <param name="droneId"></param>
        public void UnChargeDrone(int droneId);

        ////***show options***/////

        /// <summary>
        /// the function recives an ID number of  a base station and returns the relevent station.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public IDAL.DO.BaseStation GetBaseStation(int baseStationId);

        /// <summary>
        /// the function recives an ID number of a drone and returns the relevent drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public IDAL.DO.Drone GetDrone(int droneId);

        /// <summary>
        /// the function recives an ID number of a customer and returns the relevent customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IDAL.DO.Customer GetCustomer(int customerId);

        /// <summary>
        /// the function recives an ID number of a parcel and returns the relevent parcel.
        /// </summary>
        /// <param name="parchesId"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel GetParcel(int parcelId);



        ////***get Lists***////

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        public IEnumerable<IDAL.DO.BaseStation> GetBaseStations();

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Drone> GetDrones();

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        public IEnumerable<IDAL.DO.Customer> GetCustomers();

        /// <summary>
        /// returns the array of the parcheses
        /// </summary>
        public IEnumerable<IDAL.DO.Parcel> GetParcheses();

        /// <summary>
        /// returns all the parcels that dont have a drone assigned to them.
        /// </summary>
        public IEnumerable<IDAL.DO.Parcel> GetParcelToDrone();

        /// <summary>
        /// returns all the base stations that have free charging slots
        /// </summary>
        public IEnumerable<IDAL.DO.BaseStation> GetFreeStations();

        /// <summary>
        /// the function gets a  weight category of a parcel.
        /// the function returns a list with all the drones that are capable of taking it.
        /// </summary>
        /// <param name="weight"></param>
        public IEnumerable<IDAL.DO.Drone> GetDroneForParcel(IDAL.DO.WeightCategories weight);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double[] ElectricityUse();

    }
}
