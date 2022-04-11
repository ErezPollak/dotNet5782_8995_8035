using System;
using System.Collections.Generic;
using DalFacade.Models;

namespace DalApi
{
    public interface IDal
    {

        # region add options

        public bool AddBaseStation(BaseStation baseStation);
        public bool AddDrone(DO.Drone drone);
        public bool AddCustomer(DO.Customer customer);
        public bool AddParcel(DO.Parcel parcel);

        #endregion

        #region update options

        public bool UpdateModelForADrone(int droneId, string model);
        public bool UpdateBaseStation(int basStationId, string name, int slots);
        public bool UpdateCustomer(int customerId, string name, string phone);
        public bool AssignDroneToParcel(int parcelId, int droneId);
        public bool PickingUpParcel(int parcelId, int droneId);
        public bool DeliveringParcel(int parcelId);
        public bool ChargeDrone(int baseStationId, int droneId);
        public double UnChargeDrone(int droneId);

        #endregion

        #region show oobject options

        public BaseStation GetBaseStation(int baseStationId);
        public DO.Drone GetDrone(int droneId);
        public DO.Customer GetCustomer(int customerId);
        public DO.Parcel GetParcel(int parcelId);

        #endregion

        # region get lists option

        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> f);
        public IEnumerable<DO.Drone> GetDrones(Predicate<DO.Drone> f);
        public IEnumerable<DO.Customer> GetCustomers(Predicate<DO.Customer> f);
        public IEnumerable<DO.Parcel> GetParcels(Predicate<DO.Parcel> f);
        public IEnumerable<DO.DroneCharge> GetChargeDrones(Predicate<DO.DroneCharge> f);

        #endregion

        #region operational functions
        public double[] ElectricityUse();
        public int GetClosestStation(int id);
        public int GetSerialNumber();

        #endregion

    }
}
