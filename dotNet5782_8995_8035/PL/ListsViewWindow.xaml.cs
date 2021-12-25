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
        private IEnumerable<BO.DroneForList> droneList;
        private IEnumerable<BO.ParcelForList> parcelList;
        private IEnumerable<BO.BaseStationForList> baseStatoinList;
        private IEnumerable<BO.CustomerForList> costumerList;



        public ListsViewWindow(BlApi.IBL bl)
        {

            InitializeComponent();

            this.bl = bl;

            this.droneList = bl.GetDrones(_ => true);
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
            StatusSelector.DataContext = statusesSelector;

            //making the list for the whight selector.
            List<string> whightSelectorlist = Enum.GetNames(typeof(BO.Enums.WeightCategories)).Cast<string>().ToList();
            whightSelectorlist.Add("Show All");
            WeightSelecter.DataContext = whightSelectorlist;

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


        public void UpdateDroneList()
        {
            string whight = null, status = null;

            if (WeightSelecter.SelectedItem != null)
                whight = WeightSelecter.SelectedItem.ToString();

            if (StatusSelector.SelectedItem != null)
                status = StatusSelector.SelectedItem.ToString();

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


        public void UpdateParcelList()
        {
            string status = null;

            if (ParcelStatusSelector.SelectedItem != null)
                status = ParcelStatusSelector.SelectedItem.ToString();

            this.parcelList = bl.GetPacels(p =>
                    //(p.Weight.ToString() == whight || whight == "Show All") &&
                    (p.Status.ToString() == status || status == "Show All"));

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

        public void UpdateBaseStationList()
        {
            switch (NumberOfSlotsSelector?.SelectedItem?.ToString())
            {

                case "Show All":

                    this.baseStatoinList = bl.GetBaseStations(_ => true);

                    break;
                case "Has Open Charging Slots":

                    this.baseStatoinList = bl.GetBaseStations(b => b.FreeChargingSlots > 0);

                    break;

                default:
                    break;
            }

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

        public void UpdateCustomerList()
        {
            //string whight = null, status = null;

            //if (WeightSelecter.SelectedItem != null)
            //    whight = WeightSelecter.SelectedItem.ToString();

            //if (StatusSelector.SelectedItem != null)
            //    status = StatusSelector.SelectedItem.ToString();

            //this.droneList = bl.GetDrones(d =>
            //        (d.Weight.ToString() == whight || whight == "Show All") &&
            //        (d.Status.ToString() == status || status == "Show All"));

            CustomerList.DataContext = costumerList;

        }

        #endregion


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
