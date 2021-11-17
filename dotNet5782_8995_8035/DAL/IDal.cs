using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IDal
    {

        ////***adding options***/////

        public bool AddBaseStation(IDAL.DO.BaseStation baseStation);
        public bool AddDrone(IDAL.DO.Drone drone);
        public bool AddCustumer(IDAL.DO.Customer customer);
        public bool AddParcel(IDAL.DO.Parcel parcel);



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


        public IDAL.DO.BaseStation GetBaseStation(int baseStationId);
        public IDAL.DO.Drone GetDrone(int droneId);
        public IDAL.DO.Customer GetCustomer(int customerId);
        public IDAL.DO.Parcel GetParcel(int parcelId);



        ////***get Lists***////

        public IEnumerable<IDAL.DO.BaseStation> GetBaseStations(Predicate<IDAL.DO.BaseStation> f);
        public IEnumerable<IDAL.DO.Drone> GetDrones(Predicate<IDAL.DO.Drone> f);
        public IEnumerable<IDAL.DO.Customer> GetCustomers(Predicate<IDAL.DO.Customer> f);
        public IEnumerable<IDAL.DO.Parcel> GetParcels(Predicate<IDAL.DO.Parcel> f);
        //public IEnumerable<IDAL.DO.Parcel> GetParcelToDrone();
        //public IEnumerable<IDAL.DO.BaseStation> GetFreeStations();
        public IEnumerable<IDAL.DO.DroneCharge> GetChargeDrones(Predicate<IDAL.DO.DroneCharge> f);


        public double[] ElectricityUse();
        public int GetClothestStation(int id);
        public int GetBaseStationsNumber();
        public int GetBaseStationId(int index);
        //public IEnumerable<IDAL.DO.Parcel> GetProvidedParcels();

        //functions for BL


        public int GetSerialNumber();
        //void SetNameForADrone(int droneId, string model);
        //void UpdateBaseStation(int basStationID, string name, int slots);
    }
}
