using System.Collections.ObjectModel;
using System.ComponentModel;
using BL.Models;

namespace BL.Abstracts
{
    public interface IBl
    {
        #region adding option 

        public bool AddBaseStation(BaseStation newBaseStation);
        public bool AddDrone(Drone newDrone);
        public bool AddCustomer(Customer customer);
        public bool AddParcel(Parcel newParcel);

        #endregion

        #region update options 

        public bool UpdateNameForADrone(int droneId, string model);
        public bool UpdateBaseStation(int basStationId, string name, int slots);
        public bool UpdateCustomer(int customerId, string name, string phone);
        public bool AssignParcelToADrone(int droneId);
        public bool DeliveringParcelFromADrone(int droneId);
        public bool PickingUpParcelToDrone(int droneId);
        public bool ChargeDrone(int droneId);
        public bool UnChargeDrone(int droneId);

        #endregion

        #region show options 

        public BaseStation GetBaseStation(int baseStationId);
        public Drone GetDrone(int droneId);
        public Customer GetCustomer(int customerId);
        public Parcel GetParcel(int parcelId);


        #region Public Calls for Lists Functions

        public ObservableCollection<BaseStationForList> GetBaseStations();
        public ObservableCollection<DroneForList> GetDrones();
        public ObservableCollection<CustomerForList> GetCustomers();
        public ObservableCollection<ParcelForList> GetParcels();
        public ObservableCollection<ParcelForList> GetParcelsThatIncludeTheCustomer(int customerId);
        public ObservableCollection<CustomerForList> GetCustomersThatIncludeTheCustomer(ObservableCollection<ParcelForList> parcelList);
        public ObservableCollection<DroneForList> GetDronesForSelectors(string weight, string status);
        public ObservableCollection<ParcelForList> GetParcelsForSelector(string parcelStatus = "DEFINED");
        public ObservableCollection<BaseStationForList> GetBaseStationsForSelector(string openSlots = "Has Open Charging Slots");

        #endregion

        #endregion

        #region operation functions

        public int GetNextSerialNumberForParcel();

        #endregion

        #region Aoutomatic
        public void AutomaticOperation(BackgroundWorker worker, int droneId, int length);
        
        #endregion
    }
}
