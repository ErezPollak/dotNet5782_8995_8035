
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal sealed class DalXML : DalApi.IDal
    {

        #region Singalton

        /// <summary>
        /// private constructor for the dal class, for the singalton.
        /// </summary>
        private DalXML()
        {
            //DataSourceXML.Initialize();
        }

        /// <summary>
        /// dal field intended to keep the insstance of the bl that was created.
        /// </summary>
        private static readonly Lazy<DalApi.IDal> instance = new Lazy<DalApi.IDal>(() => new DalXML());

        // <summary>
        /// the function the creates new instance of DAL only if it doesn't exists already.
        /// </summary>
        /// <returns></returns>
        public static DalApi.IDal GetInstance()
        {
            return instance.Value;
        }

        #endregion



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
