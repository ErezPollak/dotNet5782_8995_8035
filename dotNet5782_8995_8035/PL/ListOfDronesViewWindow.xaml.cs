using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for ListOfDronesViewWindow.xaml
    /// </summary>
    public partial class ListOfDronesViewWindow : Window
    {
        private IBL.IBL bl;

        public ListOfDronesViewWindow(IBL.IBL bl)
        {
            InitializeComponent();
            
            this.bl = bl;
            
            ListOfDronesView.ItemsSource = bl.GetDrones(_ => true);

            //making list of values for the status selector.

            List<string> statusesSelector = Enum.GetNames(typeof(IBAL.BO.Enums.DroneStatuses)).Cast<string>().ToList();

            statusesSelector.Add("Show All");

            StatusSelector.ItemsSource = statusesSelector;

            //making the list for the whight selector.

            List<string> whightSelectorlist = Enum.GetNames(typeof(IBAL.BO.Enums.WeightCategories)).Cast<string>().ToList();

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
                    (d.Weight.ToString() == whight || whight == "Show All" || whight == null) &&
                    (d.Status.ToString() == status || status == "Show All" || status == null));
        }

        private void ClickedItem(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ///MessageBox.Show(ListOfDronesView.SelectedItem.ToString());

            DroneWindow droneWindow = new DroneWindow(bl, this, bl.GetDrone(((IBAL.BO.DroneForList)ListOfDronesView.SelectedItem).Id));
            droneWindow.Show();

        }
    }
}
