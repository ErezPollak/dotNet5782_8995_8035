using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BL;
using BL.Abstracts;
using BL.Exceptions;
using BL.Models;

namespace PL
{
    /// <summary>
    /// Interaction logic for ListOfDronesViewWindow.xaml
    /// </summary>
    public partial class ListsViewWindow
    {
        /// <summary>
        /// bl for the bl operations.
        /// </summary>
        private readonly IBl _bl = BlFactory.GetBl();

        /// <summary>
        /// lists like requested for the bonus.
        /// </summary>
        private ObservableCollection<DroneForList> _droneList;
        private ObservableCollection<ParcelForList> _parcelList;
        private ObservableCollection<BaseStationForList> _baseStationList;
        private ObservableCollection<CustomerForList> _costumerList;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="accessesAuthorizer"></param>
        /// <param name="customerId"></param>
        internal ListsViewWindow(ACCESS_AUTHORIZATION accessesAuthorizer, int customerId = 0)
        {
            InitializeComponent();

            MainTabsStack.DataContext = accessesAuthorizer;

            if (customerId == 0)
            {
                //initializing the lists.
                _droneList = _bl.GetDrones();
                _parcelList = _bl.GetParcels();
                _baseStationList = _bl.GetBaseStations();
                _costumerList = _bl.GetCustomers();

                //initializing the data context
                ListOfDronesView.DataContext = _droneList;
                ListOfParcelsView.DataContext = _parcelList;
                ListOfBaseStationsView.DataContext = _baseStationList;
                ListOfCustomersView.DataContext = _costumerList;

                //making list of values for the status selector.
                var statusesSelector = Enum.GetNames(typeof(Enums.DroneStatuses)).ToList();
                statusesSelector.Add("Show All");
                DroneStatusSelector.DataContext = statusesSelector;

                //making the list for the weight selector.
                var weightSelectors = Enum.GetNames(typeof(Enums.WeightCategories)).ToList();
                weightSelectors.Add("Show All");
                DroneWeightSelecter.DataContext = weightSelectors;

                //making list for the number of slots selector
                List<string> selectingOptions = new()
                {
                    "Show All",
                    "Has Open Charging Slots"
                };
                NumberOfSlotsSelector.DataContext = selectingOptions;

                //make lst for the parcel status selector
                var parcelStatusSelector = Enum.GetNames(typeof(Enums.ParcelStatus)).ToList();
                parcelStatusSelector.Add("Show All");
                ParcelStatusSelector.DataContext = parcelStatusSelector;
            }
            else
            {
                //for customer mode, initializing the data that is required for the specific customer.
                if (_bl.GetCustomer(customerId) == null)
                    throw new IdDontExistsException(customerId, "customer");

                //the list of parcels that the customer is a receiver or a sender.
                _parcelList = _bl.GetParcelsThatIncludeTheCustomer(customerId);
                //all the other customers from the parcels that the customer is a pat of.
                _costumerList = _bl.GetCustomersThatIncludeTheCustomer(_parcelList);

                ListOfParcelsView.DataContext = _parcelList;
                ListOfCustomersView.DataContext = _costumerList;
            }
        }

        #region DroneList

        /// <summary>
        /// updating the drone list if the selector chose a status. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateDroneList();
        }

        /// <summary>
        /// updating the drone list if the selector chose a weight. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateDroneList();
        }

        /// <summary>
        /// opening the window of adding a drone with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            var addDroneWindow = new DroneWindow(this);
            addDroneWindow.ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a drone with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedDroneInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var droneWindow = new DroneWindow(this, _bl.GetDrone(((DroneForList)ListOfDronesView.SelectedItem).Id));
            droneWindow.ShowDialog();
        }

        /// <summary>
        /// a function that updating the list with the add function of observable collection.
        /// </summary>
        /// <param name="drone"></param>
        internal void AddDrone(Drone drone)
        {
            var listDrone = new DroneForList()
            {
                Id = drone.Id,
                Battery = drone.Battery,
                Location = drone.Location,
                Model = drone.Model,
                ParcelId = 0,
                Status = drone.Status,
                Weight = drone.MaxWeight
            };

            _droneList.Add(listDrone);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        internal void UpdateDroneList() {

            string weight = null;
            string status = null;

            if (DroneWeightSelecter.SelectedItem != null)
                weight = DroneWeightSelecter.SelectedItem.ToString();

            if (DroneStatusSelector.SelectedItem != null)
                status = DroneStatusSelector.SelectedItem.ToString();

            _droneList = _bl.GetDronesForSelectors(weight, status);

            ListOfDronesView.DataContext = _droneList;

        }

        #endregion


        #region ParcelList
        /// <summary>
        /// updating the parcel list if the selector chose a status. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelStatusChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateParcelList();
        }

        /// <summary>
        /// opening the window of adding a parcel with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            //Open Parcel for add

            new ParcelWindow(this).ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a parcel with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedParcelInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //open parcel for operations
            try
            {
                var parcel = _bl.
                    GetParcel(((ParcelForList)ListOfParcelsView.
                    SelectedItem).
                    Id);
                new ParcelWindow(this, parcel).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        /// <summary>
        /// grouping the sender name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBySender_Click(object sender, RoutedEventArgs e)
        {
            var group = from parcel in _parcelList
                        group parcel by parcel.SenderName;

            _parcelList = GroupToObservable(group);
            ListOfParcelsView.DataContext = _parcelList;
        }

        /// <summary>
        /// grouping the receiver name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupByReceiver_Click(object sender, RoutedEventArgs e)
        {
            var group = from parcel in _parcelList 
                        group parcel by parcel.ReceiverName;

            _parcelList = GroupToObservable(group);
            ListOfParcelsView.DataContext = _parcelList;
        }

        /// <summary>
        /// a function that updating the list with the add function of observable collection.
        /// </summary>
        internal void AddParcel(Parcel parcel)
        {
            if (parcel == null) throw new ArgumentNullException(nameof(parcel));

            ParcelForList listParcel = new()
            {
                Id = parcel.Id,
                Priority = parcel.Priority,

                Weight = parcel.Weight,
                SenderName = parcel.Sender.CustomerName,
                ReceiverName = parcel.Receiver.CustomerName,
                Status = Enums.ParcelStatus.DEFINED
            };

            _parcelList.Add(listParcel);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        internal void UpdateParcelList()
        {
            string parcelStatus = null;

            if (ParcelStatusSelector.SelectedItem != null)
                parcelStatus = ParcelStatusSelector.SelectedItem.ToString();

            _parcelList = _bl.GetParcelsForSelector(parcelStatus);

            ListOfParcelsView.DataContext = _parcelList;
        }

        #endregion


        #region BaseStation
        /// <summary>
        /// grouping the baseStations according to the selected grouping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseStationSlotsNumberSelected(object sender, SelectionChangedEventArgs e)
        {
            UpdateBaseStationList();
        }

        /// <summary>
        /// opening the window of adding a baseStation with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(this).ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a baseStation with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedBaseStationInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(ListOfBaseStationsView.SelectedItem != null)
                new BaseStationWindow(this, _bl.GetBaseStation(((BaseStationForList)ListOfBaseStationsView.SelectedItem).Id)).ShowDialog();
        }

        /// <summary>
        /// a function that updating the list with the add function of observable collection.
        /// </summary>
        internal void AddBaseStation(BaseStation baseStation)
        {
            if (baseStation == null) throw new ArgumentNullException(nameof(baseStation));
            BaseStationForList listBaseStation = new()
            {
                Id = baseStation.Id,
                Name = baseStation.Name,
                FreeChargingSlots = baseStation.ChargeSlots,
                TakenCharingSlots = 0
            };

            _baseStationList.Add(listBaseStation);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        internal void UpdateBaseStationList()
        {
            string openSlots = null;

            if (NumberOfSlotsSelector.SelectedItem != null)
                openSlots = NumberOfSlotsSelector.SelectedItem.ToString();

            _baseStationList = _bl.GetBaseStationsForSelector(openSlots);
            ListOfBaseStationsView.DataContext = _baseStationList;

        }

        /// <summary>
        /// grouping the base stations by the selected grouping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBaseStationByFreeSlots_Click(object sender, RoutedEventArgs e)
        {
            var group = from baseStation in _baseStationList 
                        group baseStation by baseStation.FreeChargingSlots;

            _baseStationList = GroupToObservable(group);
            ListOfBaseStationsView.DataContext = _baseStationList;
        }

        #endregion


        #region CustomerList

        /// <summary>
        /// opening the window of adding a customer with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(this).ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a customer with the required parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedCustomerInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new CustomerWindow(this, _bl.GetCustomer(((CustomerForList)ListOfCustomersView.SelectedItem).Id)).ShowDialog();
        }

        /// <summary>
        /// a function that updating the list with the add function of observable collection.
        /// </summary>
        /// <param name="customer"></param>
        internal void AddCustomer(Customer customer)
        {
            CustomerForList listCustomer = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                SentFromAndDeliverd = 0,
                SentFromAndNotDeliverd = 0,
                SentToAndDeliverd = 0,
                SentToAnDNotDelivered = 0
            };
            _costumerList.Add(listCustomer);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        private void UpdateCustomerList()
        {
            _costumerList = _bl.GetCustomers();

            ListOfCustomersView.DataContext = _costumerList;
        }

        #endregion

        /// <summary>
        /// updates all the lists
        /// </summary>
        internal void UpdateLists()
        {
            //update drone list
            UpdateDroneList();

            //update the base station list.
            UpdateBaseStationList();

            //update parcel list
            UpdateParcelList();

            //update the customer list
            UpdateCustomerList();
        }

        /// <summary>
        /// close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// dragging the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        /// <summary>
        /// creates an observable collection and puts in it all the grouped values from the grouped
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        private ObservableCollection<T> GroupToObservable<T, TK>(IEnumerable<IGrouping<TK, T>> group)
        {
            ObservableCollection<T> collection = new();
            foreach (var numberGroup in group)
            {
                foreach (var element in numberGroup)
                {
                    collection.Add(element);
                }
            }

            return collection;
        }




    }
}
