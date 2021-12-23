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
            selectingOptions.Add("Group by numbers");
            NumberOfSlotsSelector.DataContext = selectingOptions;

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
            //string whight = null, status = null;

            //if (WeightSelecter.SelectedItem != null)
            //    whight = WeightSelecter.SelectedItem.ToString();

            //if (StatusSelector.SelectedItem != null)
            //    status = StatusSelector.SelectedItem.ToString();

            //this.droneList = bl.GetDrones(d =>
            //        (d.Weight.ToString() == whight || whight == "Show All") &&
            //        (d.Status.ToString() == status || status == "Show All"));

            //DroneList.DataContext = droneList;

        }

        #endregion


        #region BaseStation

        private void BaseStationGuopingSelected(object sender, SelectionChangedEventArgs e)
        {
            UpdateBaseStationList();
        }

        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClickedBaseStationInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        public void UpdateBaseStationList()
        {
            switch (NumberOfSlotsSelector.SelectedItem.ToString())
            {

                case "Show All":

                    this.baseStatoinList = bl.GetBaseStations(_ => true);

                    break;
                case "Has Open Charging Slots":

                    this.baseStatoinList = bl.GetBaseStations(b => b.FreeChargingSlots > 0);

                    break;
                case "Group by numbers":

                    var group = bl.GetBaseStations(_ => true).GroupBy(b => b.FreeChargingSlots);

                    List<BO.BaseStationForList> newList = new();

                    foreach(var g in group)
                    {
                        foreach(var g1 in g)
                        {
                            newList.Add(g1);
                        }
                    }

                    this.baseStatoinList = newList;

                    

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

        }

        private void ClickedCustomerInList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

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
