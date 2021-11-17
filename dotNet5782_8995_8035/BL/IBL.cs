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

        public bool AddBaseStation(IBAL.BO.BaseStation baesStation);
        public bool AddDrone(IBAL.BO.DroneForList newDrone);
        public bool AddCustumer(IBAL.BO.Customer customer);
        public bool AddParcel(IBAL.BO.Parcel newPparcel);

        ////***update options***/////

        public bool UpdateNameForADrone(int droneId, string model);
        public bool UpdateBaseStation(int basStationID, string name, int slots);
        public bool UpdateCustomer(int customerID, string name, string phone);
        public bool AssignParcelToADrone(int droneId);
        public bool DeliveringParcelFromADrone(int droneId);
        public bool ChargeDrone(int droneId);
        public bool UnChargeDrone(int droneId, int minutes);

        ////***show options***/////

        public IBAL.BO.BaseStation GetBaseStation(int baseStationId);
        public IBAL.BO.DroneForList GetDrone(int droneId);
        public IBAL.BO.Customer GetCustomer(int customerId);
        public IBAL.BO.Parcel GetParcel(int parcelId);

        ////***get Lists***////

        public IEnumerable<IBAL.BO.BaseStationForList> GetBaseStations(Predicate<IDAL.DO.BaseStation> f);
        public IEnumerable<IBAL.BO.DroneForList> GetDrones(Predicate<IBAL.BO.DroneForList> f);
        public IEnumerable<IBAL.BO.CustomerForList> GetCustomers(Predicate<IDAL.DO.Customer> f);
        public IEnumerable<IBAL.BO.ParcelForList> GetPacels(Predicate<IDAL.DO.Parcel> f);
        //public IEnumerable<IBAL.BO.Parcel> GetParcelToDrone();


        /////////functions for BL


        public int GetNextSerialNumberForParcel();
        public IBAL.BO.CoustomerForParcel GetCustomerForParcel(int customerId);
        
       
    }
}
