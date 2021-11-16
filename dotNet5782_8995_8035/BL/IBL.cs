using IBAL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBL
    {

        ////***add options***/////

        public void AddBaseStation(IBAL.BO.BaseStation baesStation);
        public void AddDrone(IBAL.BO.DroneForList newDrone);
        public void AddCustumer(IBAL.BO.Customer customer);
        public void AddParcel(IBAL.BO.Parcel newPparcel);

        ////***update options***/////

        public void SetNameForADrone(int droneId, string model);
        public void UpdateBaseStation(int basStationID, string name, int slots);
        public void UpdateCustomer(int customerID, string name, string phone);
        public void AssignParcelTOADrone(int droneId);
        public void DeliveringParcelFromADrone(int droneId);
        public void ChargeDrone(int droneId);
        public void UnChargeDrone(int droneId, int minutes);

        ////***show options***/////

        public IBAL.BO.BaseStation GetBaseStation(int baseStationId);
        public IBAL.BO.DroneForList GetDrone(int droneId);
        public IBAL.BO.Customer GetCustomer(int customerId);
        public IBAL.BO.Parcel GetParcel(int parcelId);

        ////***get Lists***////

        public IEnumerable<IBAL.BO.BaseStationForList> GetBaseStations(Predicate<IDAL.DO.BaseStation> f);
        public IEnumerable<IBAL.BO.DroneForList> GetDrones(Predicate<IDAL.DO.Drone> f);
        public IEnumerable<IBAL.BO.CustomerForList> GetCustomers(Predicate<IDAL.DO.Customer> f);
        public IEnumerable<IBAL.BO.ParcelForList> GetPacels(Predicate<IDAL.DO.Parcel> f);
        //public IEnumerable<IBAL.BO.Parcel> GetParcelToDrone();


        /////////functions for BL


        public int GetNextSerialNumberForParcel();
        public IBAL.BO.CoustomerForParcel GetCustomerForParcel(int customerId);
        
       
    }
}
