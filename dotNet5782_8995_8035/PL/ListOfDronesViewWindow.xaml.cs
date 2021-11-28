using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            
            ListOfDronesView.ItemsSource = bl.GetDrones(t => true);

            List<string> statusesSelector = Enum.GetNames(typeof(IBAL.BO.Enums.DroneStatuses)).Cast<string>().ToList();

            statusesSelector.Add("Show all");

            this.StatusSelector.ItemsSource = statusesSelector;//.GetValues(typeof(IBAL.BO.Enums.DroneStatuses));

            this.WeightSelecter.ItemsSource = Enum.GetValues(typeof(IBAL.BO.Enums.WeightCategories));

        }

        private void StatusChoose(object sender, SelectionChangedEventArgs e)
        {
            if (this.StatusSelector.SelectedItem.ToString() == "Show all")
            {
                //ListOfDronesView.ItemsSource = bl.GetDrones(t => true);

                if (this.WeightSelecter.SelectedItem != null)
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Weight.ToString() == this.WeightSelecter.SelectedItem.ToString());
                }
                else
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(t => true);
                }
            }
            else
            {

                // && t.Weight.ToString() == this.WeightSelecter.SelectedItem.ToString()

                if (this.WeightSelecter.SelectedItem != null)
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Status.ToString() == this.StatusSelector.SelectedItem.ToString() && t.Weight.ToString() == this.WeightSelecter.SelectedItem.ToString());
                }
                else
                {
                    ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Status.ToString() == this.StatusSelector.SelectedItem.ToString());
                }
            }
        }

        private void WeightChoose(object sender, SelectionChangedEventArgs e)
        {
            
            if (this.StatusSelector.SelectedItem != null)
            {
                ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Weight.ToString() == this.WeightSelecter.SelectedItem.ToString() && (t.Status.ToString() == this.StatusSelector.SelectedItem.ToString() || this.StatusSelector.SelectedItem.ToString() == "Show all"));
            }
            else
            {
                ListOfDronesView.ItemsSource = bl.GetDrones(t => t.Weight.ToString() == this.WeightSelecter.SelectedItem.ToString());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddDroneWindow addDroneWindow = new AddDroneWindow(bl);
            addDroneWindow.Show();
        }
    }
}
