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

        public void AddBaseStation(int idNumber, string name, IDAL.DO.Location location, int chargeslots);
        public void AddDrone(int idNumber, string model, IDAL.DO.WeightCategories weight);
        public void AddCustumer(int idNumber, string name, string phone, IDAL.DO.Location location);
        public void AddParcel(int idNumber, int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime? reqested, DateTime? scheduled);



        ////***update options***/////

        public void UpdateDroneForAParcel(int parcelId, int droneId);
        public void PickingUpParcel(int parcelId);
        public void DeliveringParcel(int parcelId);
        public void ChargeDrone(int baseStationId, int droneId);
        public void UnChargeDrone(int droneId);

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
