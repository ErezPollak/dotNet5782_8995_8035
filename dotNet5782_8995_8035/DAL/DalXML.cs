using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal sealed class DalXML : DalApi.IDal
    {
        public bool AddBaseStation(BaseStation baseStation)
        {
            throw new NotImplementedException();
        }

        public bool AddCustumer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public bool AddDrone(Drone drone)
        {
            throw new NotImplementedException();
        }

        public bool AddParcel(Parcel parcel)
        {
            throw new NotImplementedException();
        }

        public bool ChargeDrone(int baseStationId, int droneId)
        {
            throw new NotImplementedException();
        }

        public bool DeliveringParcel(int parcelId)
        {
            throw new NotImplementedException();
        }

        public double[] ElectricityUse()
        {
            throw new NotImplementedException();
        }

        public BaseStation GetBaseStation(int baseStationId)
        {
            throw new NotImplementedException();
        }

        public int GetBaseStationId(int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> f)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetChargeDrones(Predicate<DroneCharge> f)
        {
            throw new NotImplementedException();
        }

        public int GetClosestStation(int id)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> f)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int droneId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> f)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int parcelId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> f)
        {
            throw new NotImplementedException();
        }

        public int GetSerialNumber()
        {
            throw new NotImplementedException();
        }

        public bool PickingUpParcel(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public double UnChargeDrone(int droneId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBaseStation(int basStationID, string name, int slots)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCustomer(int customerID, string name, string phone)
        {
            throw new NotImplementedException();
        }

        public bool AssignDroneToParcel(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateNameForADrone(int droneId, string model)
        {
            throw new NotImplementedException();
        }

    }
}
