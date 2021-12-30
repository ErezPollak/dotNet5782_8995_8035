using System;
using System.Collections.Generic;

namespace DalApi
{
    public interface IDal
    {

        # region add options

        public bool AddBaseStation(DO.BaseStation baseStation);
        public bool AddDrone(DO.Drone drone);
        public bool AddCustumer(DO.Customer customer);
        public bool AddParcel(DO.Parcel parcel);

        #endregion

        #region update options

        public bool UpdateNameForADrone(int droneId, string model);
        public bool UpdateBaseStation(int basStationID, string name, int slots);
        public bool UpdateCustomer(int customerID, string name, string phone);
        public bool AssignDroneToParcel(int parcelId, int droneId);
        public bool PickingUpParcel(int parcelId, int droneId);
        public bool DeliveringParcel(int parcelId);
        public bool ChargeDrone(int baseStationId, int droneId);
        public double UnChargeDrone(int droneId);

        #endregion

        #region show oobject options

        public DO.BaseStation GetBaseStation(int baseStationId);
        public DO.Drone GetDrone(int droneId);
        public DO.Customer GetCustomer(int customerId);
        public DO.Parcel GetParcel(int parcelId);

        #endregion

        # region get lists option

        public IEnumerable<DO.BaseStation> GetBaseStations(Predicate<DO.BaseStation> f);
        public IEnumerable<DO.Drone> GetDrones(Predicate<DO.Drone> f);
        public IEnumerable<DO.Customer> GetCustomers(Predicate<DO.Customer> f);
        public IEnumerable<DO.Parcel> GetParcels(Predicate<DO.Parcel> f);
        public IEnumerable<DO.DroneCharge> GetChargeDrones(Predicate<DO.DroneCharge> f);

        #endregion

        #region operational functions
        public double[] ElectricityUse();
        public int GetClosestStation(int id);
        public int GetBaseStationId(int index);
        public int GetSerialNumber();

        #endregion

    }
}
