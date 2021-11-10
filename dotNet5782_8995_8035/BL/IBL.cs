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
        //void AddDrone(Drone drone);
        public void AddDrone(IBAL.BO.DroneForList newDrone);
        public void AddCustumer(IBAL.BO.Customer customer);
        public void AddParcel(IBAL.BO.Parcel newPparcel);

        ////***update options***/////

        public void UpdateDroneForAParcel(int parcelId, int droneId);
        public void PickingUpParcel(int parcelId);
        public void DeliveringParcel(int parcelId);
        public void ChargeDrone(int baseStationId, int droneId);
        public void UnChargeDrone(int droneId);

        ////***show options***/////

        public IBAL.BO.BaseStation GetBaseStation(int baseStationId);
        public IBAL.BO.DroneForList GetDrone(int droneId);
        public IBAL.BO.Customer GetCustomer(int customerId);
        public IBAL.BO.Parcel GetParcel(int parcelId);

        ////***get Lists***////

        public IEnumerable<IBAL.BO.BaseStation> GetBaseStations();
        public IEnumerable<IBAL.BO.DroneForList> GetDrones();
        public IEnumerable<IBAL.BO.Customer> GetCustomers();
        public IEnumerable<IBAL.BO.Parcel> GetPacelss();
        //public IEnumerable<IBAL.BO.Parcel> GetParcelToDrone();
    }
}
