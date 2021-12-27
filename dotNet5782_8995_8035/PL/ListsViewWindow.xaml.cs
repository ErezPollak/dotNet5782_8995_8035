using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace PL
{
    /// <summary>
    /// Interaction logic for ListOfDronesViewWindow.xaml
    /// </summary>
    public partial class ListsViewWindow : Window
    {
        
        private BlApi.IBL bl;
        private ObservableCollection<BO.DroneForList> droneList;
        private ObservableCollection<BO.ParcelForList> parcelList;
        private ObservableCollection<BO.BaseStationForList> baseStatoinList;
        private ObservableCollection<BO.CustomerForList> costumerList;

        public ListsViewWindow(BlApi.IBL bl)
        { 

            InitializeComponent();

            this.bl = bl;

            this.droneList =  bl.GetDrones(_ => true);
            this.parcelList = bl.GetPacels(_ => true);
            this.baseStatoinList = bl.GetBaseStations(_ => true);
            this.costumerList = bl.GetCustomers(_ => true);

            DroneList.DataContext = droneList;
            ParcelList.DataContext = parcelList;
            BaseStationList.DataContext = baseStatoinList;
            CustomerList.DataContext = costumerList;

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
            //this.DataContext = this;



        }

        #region DroneList

        private void StatusChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateDroneList();
        }

        private void WeightChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateDroneList();
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow addDroneWindow = new DroneWindow(bl, this);
            addDroneWindow.ShowDialog();
        }

        private void ClickedDroneInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if ((BO.DroneForList)sender != null)
            //{
            DroneWindow droneWindow = new DroneWindow(bl, this, bl.GetDrone(((BO.DroneForList)DroneList.SelectedItem).Id));
            droneWindow.ShowDialog();
            //}
        }

        public void AddDrone(BO.Drone drone)
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

        public void UpdateDroneList() {

            string whight = null;
            string status = null;

            if (DroneWeightSelecter.SelectedItem != null)
                whight = DroneWeightSelecter.SelectedItem.ToString();

            if (DroneStatusSelector.SelectedItem != null)
                status = DroneStatusSelector.SelectedItem.ToString();

            this.droneList = bl.GetDrones(d =>
                    (d.Weight.ToString() == whight || whight == "Show All") &&
                    (d.Status.ToString() == status || status == "Show All"));

            DroneList.DataContext = droneList;

        }

        #endregion


        #region ParcelList

        private void ParcelStatusChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateParcelList();
        }

        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            //Open Parcel for add
        }

        private void ClickedParcelInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //open parcel for operations
        }

        public void AddParcel(BO.Parcel parcel)
        {
            //BO.Enums.ParcelStatus status = BO.Enums.ParcelStatus.DEFINED;

            //if (parcel.Drone != null) status = BO.Enums.ParcelStatus.ASSIGNED;
            //
           // if (parcel.PickupTime != null) status = BO.Enums.ParcelStatus.PICKEDUP;

            //if (parcel.DeliveringTime != null) status = BO.Enums.ParcelStatus.DELIVERED;

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

        public void UpdateParcelList()
        {
            string parcelStaus = null;

            if (ParcelStatusSelector.SelectedItem != null)
                parcelStaus = ParcelStatusSelector.SelectedItem.ToString();

            this.parcelList = bl.GetPacels(b =>
                (b.Status.ToString() == parcelStaus || parcelStaus == "Show All")
            );

            ParcelList.DataContext = this.parcelList;

        }

        #endregion


        #region BaseStation

        private void BaseStationGuopingSelected(object sender, SelectionChangedEventArgs e)
        {
            UpdateBaseStationList();
        }

        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(bl, this).ShowDialog();
        }

        private void ClickedBaseStationInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new BaseStationWindow(bl, this, bl.GetBaseStation(((BO.BaseStationForList)ListOfBaseStationsView.SelectedItem).Id)).ShowDialog();
        }

        public void AddBaseStation(BO.BaseStation baseStation)
        {
            //int numberOfDrones = baseStation.ChargingDrones.Count();

            BO.BaseStationForList listBaseStation = new()
            {
                Id = baseStation.Id,
                Name = baseStation.Name,
                FreeChargingSlots = baseStation.ChargeSlots,
                TakenCharingSlots = 0

                //FreeChargingSlots = baseStation.ChargeSlots - numberOfDrones,
                //TakenCharingSlots = numberOfDrones
            };

            this.baseStatoinList.Add(listBaseStation);
        }

        public void UpdateBaseStationList()
        {

            string openSlots = null;

            if (NumberOfSlotsSelector.SelectedItem != null)
                openSlots = NumberOfSlotsSelector.SelectedItem.ToString();

            this.baseStatoinList = bl.GetBaseStations(b =>
                ((b.FreeChargingSlots > 0) && (openSlots == "Has Open Charging Slots")) || (openSlots == "Show All")
            );

            BaseStationList.DataContext = this.baseStatoinList;

        }

        #endregion


        #region CustomerList

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, this).ShowDialog();
        }

        private void ClickedCustomerInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new CustomerWindow(bl, this, bl.GetCustomer(((BO.CustomerForList)ListOfCustomersView.SelectedItem).Id)).ShowDialog();
        }

        public void AddCustomer(BO.Customer customer)
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
                //SentFromAndDeliverd = customer.FromCustomer.Count(p => bl.GetParcel(p.Id).DeliveringTime != null),
                //SentFromAndNotDeliverd = customer.FromCustomer.Count(p => bl.GetParcel(p.Id).DeliveringTime == null),
                //SentToAndDeliverd = customer.ToCustomer.Count(p => bl.GetParcel(p.Id).DeliveringTime != null),
                //SentToAnDNotDelivered = customer.ToCustomer.Count(p => bl.GetParcel(p.Id).DeliveringTime == null),
            };
            this.costumerList.Add(listCustomer);
        }

        public void UpdateCustomerList()
        {
            this.costumerList = bl.GetCustomers(_ => true);

            CustomerList.DataContext = this.CustomerList;
        }

        #endregion


        public void UpdateLists()
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



        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// hiding the x button of the window
        /// </summary>
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Loded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }


    }
}
