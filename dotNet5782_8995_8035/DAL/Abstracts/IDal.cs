using System;
using System.Collections.Generic;

namespace DalApi
{
    public interface IDal
    {

        ////***adding options***/////

        public bool AddBaseStation(DO.BaseStation baseStation);
        public bool AddDrone(DO.Drone drone);
        public bool AddCustumer(DO.Customer customer);
        public bool AddParcel(DO.Parcel parcel);



        ////***update options***/////

        public bool UpdateNameForADrone(int droneId, string model);
        public bool UpdateBaseStation(int basStationID, string name, int slots);
        public bool UpdateCustomer(int customerID, string name, string phone);
        public bool UpdateDroneForAParcel(int parcelId, int droneId);
        public bool PickingUpParcel(int parcelId, int droneId);
        public bool DeliveringParcel(int parcelId);
        public bool ChargeDrone(int baseStationId, int droneId);
        public bool UnChargeDrone(int droneId);

        ////***show options***/////


        public DO.BaseStation GetBaseStation(int baseStationId);
        public DO.Drone GetDrone(int droneId);
        public DO.Customer GetCustomer(int customerId);
        public DO.Parcel GetParcel(int parcelId);



        ////***get Lists***////

        public IEnumerable<DO.BaseStation> GetBaseStations(Predicate<DO.BaseStation> f);
        public IEnumerable<DO.Drone> GetDrones(Predicate<DO.Drone> f);
        public IEnumerable<DO.Customer> GetCustomers(Predicate<DO.Customer> f);
        public IEnumerable<DO.Parcel> GetParcels(Predicate<DO.Parcel> f);
        //public IEnumerable<DalApi.DO.Parcel> GetParcelToDrone();
        //public IEnumerable<DalApi.DO.BaseStation> GetFreeStations();
        public IEnumerable<DO.DroneCharge> GetChargeDrones(Predicate<DO.DroneCharge> f);


        public double[] ElectricityUse();
        public int GetClosestStation(int id);
        public int GetBaseStationId(int index);
        //public IEnumerable<DalApi.DO.Parcel> GetProvidedParcels();

        //functions for BL


        public int GetSerialNumber();
        //void SetNameForADrone(int droneId, string model);
        //void UpdateBaseStation(int basStationID, string name, int slots);
    }
}
