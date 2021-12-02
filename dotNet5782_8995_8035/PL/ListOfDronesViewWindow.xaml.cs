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

            List<string> statusesSelector = Enum.GetNames(typeof(IBAL.BO.Enums.DroneStatuses)).Cast<string>().ToList();

            statusesSelector.Add("Show all");

            StatusSelector.ItemsSource = statusesSelector;//.GetValues(typeof(IBAL.BO.Enums.DroneStatuses));

            WeightSelecter.ItemsSource = Enum.GetValues(typeof(IBAL.BO.Enums.WeightCategories));

        }

        private void StatusChoose(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem.ToString() == "Show all")
            {
                //ListOfDronesView.ItemsSource = bl.GetDrones(t => true);

                if (WeightSelecter.SelectedItem != null)
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Weight.ToString() == WeightSelecter.SelectedItem.ToString());
                }
                else
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(_ => true);
                }
            }
            else
            {

                // && t.Weight.ToString() == this.WeightSelecter.SelectedItem.ToString()

                if (WeightSelecter.SelectedItem != null)
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Status.ToString() == StatusSelector.SelectedItem.ToString() && t.Weight.ToString() == WeightSelecter.SelectedItem.ToString());
                }
                else
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Status.ToString() == StatusSelector.SelectedItem.ToString());
                }
            }
        }

        private void WeightChoose(object sender, SelectionChangedEventArgs e)
        {
            
            if (StatusSelector.SelectedItem != null)
            {
                ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Weight.ToString() == WeightSelecter.SelectedItem.ToString() && (t.Status.ToString() == StatusSelector.SelectedItem.ToString() || StatusSelector.SelectedItem.ToString() == "Show all"));
            }
            else
            {
                ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Weight.ToString() == WeightSelecter.SelectedItem.ToString());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddDroneWindow addDroneWindow = new AddDroneWindow(bl);
            addDroneWindow.Show();
        }
    }
}
