using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BlApi
{
    public interface IBL
    {
        #region adding option 

        public bool AddBaseStation(BO.BaseStation baesStation);
        public bool AddDrone(BO.Drone newDrone);
        public bool AddCustumer(BO.Customer customer);
        public bool AddParcel(BO.Parcel newPparcel);

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

        public BO.BaseStation GetBaseStation(int baseStationId);
        public BO.Drone GetDrone(int droneId);
        public BO.Customer GetCustomer(int customerId);
        public BO.Parcel GetParcel(int parcelId);


        #region Public Calls for Lists Functions

        public ObservableCollection<BaseStationForList> GetBaseStations();
        public ObservableCollection<DroneForList> GetDrones();
        public ObservableCollection<CustomerForList> GetCustomers();
        public ObservableCollection<ParcelForList> GetPacels();
        public ObservableCollection<ParcelForList> GetPacelsThatIncludeTheCustomer(int customerId);
        public ObservableCollection<CustomerForList> GetCustomersThatIncludeTheCustomer(ObservableCollection<BO.ParcelForList> parcelList);
        public ObservableCollection<DroneForList> GetDronesForSelectors(string whight, string status);
        public ObservableCollection<ParcelForList> GetPacelsForSalector(string parcelStaus);
        public ObservableCollection<BaseStationForList> GetBaseStationsForSelector(string openSlots);

        #endregion

        #endregion

        #region operation functions

        public int GetNextSerialNumberForParcel();

        #endregion

        #region Aoutomatic
        public void AutomaticOperation(BackgroundWorker worker, int DroneId, int length);
        #endregion
    }
}
