using System;
using System.Collections.Generic;
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
    public partial class ListOfDronesViewWindow : Window
    {
        private BlApi.IBL bl;

        public ListOfDronesViewWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            
            this.bl = bl;
            
            ListOfDronesView.ItemsSource = bl.GetDrones(_ => true);

            //making list of values for the status selector.

            List<string> statusesSelector = Enum.GetNames(typeof(BO.Enums.DroneStatuses)).Cast<string>().ToList();

            statusesSelector.Add("Show All");

            StatusSelector.ItemsSource = statusesSelector;

            //making the list for the whight selector.

            List<string> whightSelectorlist = Enum.GetNames(typeof(BO.Enums.WeightCategories)).Cast<string>().ToList();

            whightSelectorlist.Add("Show All");

            WeightSelecter.ItemsSource = whightSelectorlist;

        }

        private void StatusChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void WeightChoose(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow addDroneWindow = new DroneWindow(bl , this);
            addDroneWindow.Show();
        }

        public void UpdateList()
        {
            string whight = null, status = null;

            if (WeightSelecter.SelectedItem != null)
                whight = WeightSelecter.SelectedItem.ToString();

            if (StatusSelector.SelectedItem != null)
                status = StatusSelector.SelectedItem.ToString();


            ListOfDronesView.ItemsSource = bl.GetDrones(d =>
                    (d.Weight.ToString() == whight || whight == "Show All") &&
                    (d.Status.ToString() == status || status == "Show All"));
        }

        private void ClickedItem(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if ((BO.DroneForList)sender != null)
            //{
                DroneWindow droneWindow = new DroneWindow(bl, this, bl.GetDrone(((BO.DroneForList)ListOfDronesView.SelectedItem).Id));
                droneWindow.Show();
            //}
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
