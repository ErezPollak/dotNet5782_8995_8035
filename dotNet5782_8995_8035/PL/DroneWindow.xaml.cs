using System;
using System.Windows;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        Random r;
        IBL.IBL droneBL;

        ListOfDronesViewWindow listOfDronesViewWindow;

        public DroneWindow(IBL.IBL bl , ListOfDronesViewWindow listOfDronesViewWindow)
        {
            r = new Random();

            InitializeComponent();
            droneBL = bl;
            this.listOfDronesViewWindow = listOfDronesViewWindow;

            WindowTitle.Content = "Adding A Drone";

            Weight.ItemsSource = Enum.GetValues(typeof(IBAL.BO.Enums.WeightCategories));

            BatteryText.Visibility = Visibility.Hidden;
            BatteryLabel.Visibility = Visibility.Hidden;
            StatusText.Visibility = Visibility.Hidden;
            StatusLabel.Visibility = Visibility.Hidden;
            ParcelText.Visibility = Visibility.Hidden;
            ParcelLabel.Visibility = Visibility.Hidden;
            LocationText.Visibility = Visibility.Hidden;
            LongtudeText.Visibility = Visibility.Hidden;
            LattitudeText.Visibility = Visibility.Hidden;
        }

        public DroneWindow(IBL.IBL bl, ListOfDronesViewWindow listOfDronesViewWindow , IBAL.BO.Drone drone)
        {
            r = new Random();

            InitializeComponent();
            droneBL = bl;
            this.listOfDronesViewWindow = listOfDronesViewWindow;

            WindowTitle.Content = "Operations On Drone";

            DroneID.Text = drone.Id + "";
            DroneID.IsEnabled = false;

            Weight.ItemsSource = Enum.GetValues(typeof(IBAL.BO.Enums.WeightCategories));
            Weight.SelectedIndex = (int)drone.MaxWeight;
            Weight.IsEnabled = false;

            AddingButton.Visibility = Visibility.Hidden;

            BatteryLabel.Content = drone.Battery;
            StatusLabel.Content = drone.Status.ToString();
            LongtudeText.Content = drone.Location.Longitude;
            LattitudeText.Content = drone.Location.Latitude;

            if(drone.ParcelInDelivery != null)
            {
                ParcelLabel.Content = drone.ParcelInDelivery.Id;
            }
            else
            {
                ParcelLabel.Content = "no parcel";
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(DroneID.Text);

                int weight = Weight.SelectedIndex;

                IBAL.BO.Drone newDrone = new IBAL.BO.Drone()
                {
                    Id = id,
                    Model = Model.Text,
                    MaxWeight = (IBAL.BO.Enums.WeightCategories)weight,
                    Status = IBAL.BO.Enums.DroneStatuses.FREE,
                    ParcelInDelivery = null,
                    Battery = r.Next() % 20,
                    Location = null
                };

           
                if (droneBL.AddDrone(newDrone))
                {
                    MessageBox.Show("drone added seccussfully");

                    listOfDronesViewWindow.UpdateList();

                    Close();
                }

            }catch(Exception ex)
            {
                if (ex is IBAL.BO.IdAlreadyExistsException || ex is FormatException)
                {
                    DroneID.Foreground = Brushes.Red;
                }

                MessageBox.Show(showException(ex , ""));

                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
            MessageBox.Show("operation caceled");
        }

        private string showException(Exception e , string s)
        {
            if (e == null) return s;

            return showException(e.InnerException, s + e.Message + "\n");
        }

        private void TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DroneID.Foreground = Brushes.Black;
        }
    }
}
