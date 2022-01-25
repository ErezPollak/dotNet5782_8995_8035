using BlApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for ListOfDronesViewWindow.xaml
    /// </summary>
    public partial class ListsViewWindow : Window
    {
        /// <summary>
        /// bl for the bl operations.
        /// </summary>
        private BlApi.IBL bl = BlFactory.GetBl();

        /// <summary>
        /// lists like reqested for teh bous.
        /// </summary>
        private ObservableCollection<BO.DroneForList> droneList;
        private ObservableCollection<BO.ParcelForList> parcelList;
        private ObservableCollection<BO.BaseStationForList> baseStatoinList;
        private ObservableCollection<BO.CustomerForList> costumerList;

        /// <summary>
        /// atholerazetion for teh binding of the window.
        /// </summary>
        private ACCESS_ATHOLERAZATION accssesAtholerazetion;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="accssesAtholerazetion"></param>
        /// <param name="customerId"></param>
        internal ListsViewWindow(ACCESS_ATHOLERAZATION accssesAtholerazetion, int customerId = 0)
        {

            this.accssesAtholerazetion = accssesAtholerazetion;

            InitializeComponent();

            MainTabsStack.DataContext = accssesAtholerazetion;

            if (customerId == 0)
            {
                //initilizing the lists.
                this.droneList = bl.GetDrones();
                this.parcelList = bl.GetPacels();
                this.baseStatoinList = bl.GetBaseStations();
                this.costumerList = bl.GetCustomers();

                //initilizing the data context
                ListOfDronesView.DataContext = droneList;
                ListOfParcelsView.DataContext = parcelList;
                ListOfBaseStationsView.DataContext = baseStatoinList;
                ListOfCustomersView.DataContext = costumerList;

                //making list of values for the status selector.
                List<string> statusesSelector = Enum.GetNames(typeof(BO.Enums.DroneStatuses)).Cast<string>().ToList();
                statusesSelector.Add("Show All");
                DroneStatusSelector.DataContext = statusesSelector;

                //making the list for the whight selector.
                List<string> whightSelectorlist = Enum.GetNames(typeof(BO.Enums.WeightCategories)).Cast<string>().ToList();
                whightSelectorlist.Add("Show All");
                DroneWeightSelecter.DataContext = whightSelectorlist;

                //making list for the number of slots selector
                List<string> selectingOptions = new();
                selectingOptions.Add("Show All");
                selectingOptions.Add("Has Open Charging Slots");
                NumberOfSlotsSelector.DataContext = selectingOptions;

                //make lst for the parcel status selector
                List<string> parcelStatusSelector = Enum.GetNames(typeof(BO.Enums.ParcelStatus)).Cast<string>().ToList();
                parcelStatusSelector.Add("Show All");
                ParcelStatusSelector.DataContext = parcelStatusSelector;
            }
            else
            {
                //for costomer mode, initilizing the data that is reqiered for the sepecific customer.
                if (bl.GetCustomer(customerId) == null)
                    throw new BO.IdDontExistsException(customerId, "customer");

                //the list of parcels that the customer is a reciver or a sender.
                this.parcelList = bl.GetPacelsThatIncludeTheCustomer(customerId);
                //all the other customers from the parcels that the customer is a pat of.
                this.costumerList = bl.GetCustomersThatIncludeTheCustomer(parcelList);

                ListOfParcelsView.DataContext = parcelList;
                ListOfCustomersView.DataContext = costumerList;
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
        /// opening the window of adding a drone with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow addDroneWindow = new DroneWindow(this);
            addDroneWindow.ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a drone with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedDroneInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DroneWindow droneWindow = new DroneWindow(this, bl.GetDrone(((BO.DroneForList)ListOfDronesView.SelectedItem).Id));
            droneWindow.ShowDialog();
        }

        /// <summary>
        /// afunction that updating the list with the add function of observable collection.
        /// </summary>
        /// <param name="drone"></param>
        internal void AddDrone(BO.Drone drone)
        {
            BO.DroneForList listDrone = new BO.DroneForList()
            {
                Id = drone.Id,
                Battery = drone.Battery,
                Location = drone.Location,
                Model = drone.Model,
                ParcelId = 0,
                Status = drone.Status,
                Weight = drone.MaxWeight
            };

            this.droneList.Add(listDrone);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        internal void UpdateDroneList() {

            string whight = null;
            string status = null;

            if (DroneWeightSelecter.SelectedItem != null)
                whight = DroneWeightSelecter.SelectedItem.ToString();

            if (DroneStatusSelector.SelectedItem != null)
                status = DroneStatusSelector.SelectedItem.ToString();

            this.droneList = bl.GetDronesForSelectors(whight, status);

            ListOfDronesView.DataContext = this.droneList;

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
        /// opening the window of adding a parcel with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            //Open Parcel for add

            new ParcelWindow(this).ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a parcel with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedParcelInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //open parcel for operations
            try
            {
                BO.Parcel parcel = bl.
                    GetParcel(((BO.
                    ParcelForList)ListOfParcelsView.
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
        /// grouping the sendetr name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBySender_Click(object sender, RoutedEventArgs e)
        {
            var group = from parcel in this.parcelList as IEnumerable<BO.ParcelForList>
                        group parcel by parcel.SenderName;

            this.parcelList = GroupToObservable(group);
            ListOfParcelsView.DataContext = this.parcelList;
        }

        /// <summary>
        /// grouping the reciver name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupByReciver_Click(object sender, RoutedEventArgs e)
        {
            var group = from parcel in this.parcelList as IEnumerable<BO.ParcelForList>
                        group parcel by parcel.ReceiverName;

            this.parcelList = GroupToObservable(group);
            ListOfParcelsView.DataContext = this.parcelList;
        }

        /// <summary>
        /// afunction that updating the list with the add function of observable collection.
        /// </summary>
        /// <param name="drone"></param>
        internal void AddParcel(BO.Parcel parcel)
        {

            BO.ParcelForList listParcel = new()
            {
                Id = parcel.Id,
                Priority = parcel.Priority,

                Weight = parcel.Weight,
                SenderName = parcel.Sender.CustomerName,
                ReceiverName = parcel.Reciver.CustomerName,
                Status = BO.Enums.ParcelStatus.DEFINED
            };

            this.parcelList.Add(listParcel);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        internal void UpdateParcelList()
        {
            string parcelStaus = null;

            if (ParcelStatusSelector.SelectedItem != null)
                parcelStaus = ParcelStatusSelector.SelectedItem.ToString();

            this.parcelList = bl.GetPacelsForSalector(parcelStaus);

            ListOfParcelsView.DataContext = this.parcelList;
        }

        #endregion


        #region BaseStation
        /// <summary>
        /// grouping the basestations according to the selected grouping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseStationSlotsNumberSelected(object sender, SelectionChangedEventArgs e)
        {
            UpdateBaseStationList();
        }

        /// <summary>
        /// opening the window of adding a basestation with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(this).ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a basestation with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedBaseStationInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(ListOfBaseStationsView.SelectedItem != null)
                new BaseStationWindow(this, bl.GetBaseStation(((BO.BaseStationForList)ListOfBaseStationsView.SelectedItem).Id)).ShowDialog();
        }

        /// <summary>
        /// a function that updating the list with the add function of observable collection.
        /// </summary>
        /// <param name="drone"></param>
        internal void AddBaseStation(BO.BaseStation baseStation)
        {
            BO.BaseStationForList listBaseStation = new()
            {
                Id = baseStation.Id,
                Name = baseStation.Name,
                FreeChargingSlots = baseStation.ChargeSlots,
                TakenCharingSlots = 0
            };

            this.baseStatoinList.Add(listBaseStation);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        internal void UpdateBaseStationList()
        {
            string openSlots = null;

            if (NumberOfSlotsSelector.SelectedItem != null)
                openSlots = NumberOfSlotsSelector.SelectedItem.ToString();

            this.baseStatoinList = bl.GetBaseStationsForSelector(openSlots);
            ListOfBaseStationsView.DataContext = this.baseStatoinList;

        }

        /// <summary>
        /// grouping the base stations by the selected grouping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void GroupBaseStationByFreeSlots_Click(object sender, RoutedEventArgs e)
        {
            var group = from baseStation in this.baseStatoinList as IEnumerable<BO.BaseStationForList>
                        group baseStation by baseStation.FreeChargingSlots;

            this.baseStatoinList = GroupToObservable(group);
            ListOfBaseStationsView.DataContext = this.baseStatoinList;
        }

        #endregion


        #region CustomerList

        /// <summary>
        /// opening the window of adding a customer with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(this).ShowDialog();
        }

        /// <summary>
        /// opening the window of updating a customer with the reqierd parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedCustomerInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new CustomerWindow(this, bl.GetCustomer(((BO.CustomerForList)ListOfCustomersView.SelectedItem).Id)).ShowDialog();
        }

        /// <summary>
        /// a function that updating the list with the add function of observable collection.
        /// </summary>
        /// <param name="drone"></param>
        internal void AddCustomer(BO.Customer customer)
        {
            BO.CustomerForList listCustomer = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                SentFromAndDeliverd = 0,
                SentFromAndNotDeliverd = 0,
                SentToAndDeliverd = 0,
                SentToAnDNotDelivered = 0
            };
            this.costumerList.Add(listCustomer);
        }

        /// <summary>
        /// updating the list according to the selectors.
        /// </summary>
        internal void UpdateCustomerList()
        {
            this.costumerList = bl.GetCustomers();

            ListOfCustomersView.DataContext = this.costumerList;
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
        /// draging the window.
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
        /// creats an obsevable collaction and puts in it all the grouped values from the grouped
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        private ObservableCollection<T> GroupToObservable<T, K>(IEnumerable<IGrouping<K, T>> group)
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
