using System;
using System.Collections.Generic;

namespace BlApi
{
    public interface IBL
    {

        #region adding option 

        public bool AddBaseStation(BO.BaseStation baesStation);
        public bool AddDrone(BO.Drone newDrone);
        public bool AddCustumer(BO.Customer customer);
        public bool AddParcel(BO.Parcel newPparcel);


        #endregion


        #region update options 

        public bool UpdateNameForADrone(int droneId, string model);
        public bool UpdateBaseStation(int basStationId, string name, int slots);
        public bool UpdateCustomer(int customerId, string name, string phone);
        public bool AssignParcelToADrone(int droneId);
        public bool DeliveringParcelFromADrone(int droneId);
        public bool PickingUpParcelToDrone(int droneId);
        public bool ChargeDrone(int droneId);
        public bool UnChargeDrone(int droneId, int minutes);

        #endregion


        #region show options 

        public BO.BaseStation GetBaseStation(int baseStationId);
        public BO.Drone GetDrone(int droneId);
        public BO.Customer GetCustomer(int customerId);
        public BO.Parcel GetParcel(int parcelId);

        public IEnumerable<BO.BaseStationForList> GetBaseStations(Predicate<BO.BaseStationForList> f);
        public IEnumerable<BO.DroneForList> GetDrones(Predicate<BO.DroneForList> f);
        public IEnumerable<BO.CustomerForList> GetCustomers(Predicate<BO.CustomerForList> f);
        public IEnumerable<BO.ParcelForList> GetPacels(Predicate<BO.ParcelForList> f);

        #endregion



        #region operation functions

        public int GetNextSerialNumberForParcel();

        #endregion
    }
}
