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

        public void AddBaseStation(int idNumber, string name, double longtude, double lattitude, int chargeslots);
        public void AddDrone(int idNumber, string model, IDAL.DO.WeightCategories weight);
        public void AddCustumer(int idNumber, string name, string phone, double longtitude, double lattitude);
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

        public IEnumerable<IDAL.DO.BaseStation> GetBaseStations();
        public IEnumerable<IDAL.DO.Drone> GetDrones();
        public IEnumerable<IDAL.DO.Customer> GetCustomers();
        public IEnumerable<IDAL.DO.Parcel> GetParcels();
        public IEnumerable<IDAL.DO.Parcel> GetParcelToDrone();
        public IEnumerable<IDAL.DO.BaseStation> GetFreeStations();

        
        //public Dictionary<int , int> GetDronesToUpdate();
        public double[] ElectricityUse();
        public int GetClothestStation(int id);

    }
}
