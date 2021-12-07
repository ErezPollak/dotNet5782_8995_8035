using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
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
        IBAL.BO.Drone drone;
        ListOfDronesViewWindow listOfDronesViewWindow;

        /// <summary>
        /// ctor for adding a drone.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listOfDronesViewWindow"></param>
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
            LatitudeText.Visibility = Visibility.Hidden;
            UpdateModel.Visibility = Visibility.Hidden;
            GoToCharge.Visibility = Visibility.Hidden;
            ReliceDroneFromCharge.Visibility = Visibility.Hidden;
            
            MinutesInput.Visibility = Visibility.Hidden;
            LabelMinutsInCharge.Visibility = Visibility.Hidden;

            AssiningParcelToDrone.Visibility = Visibility.Hidden;
            PickingUpAParcel.Visibility = Visibility.Hidden;
            DliveringParcel.Visibility = Visibility.Hidden;


        }

        /// <summary>
        /// ctor for showing and updating a drone. 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listOfDronesViewWindow"></param>
        /// <param name="drone"></param>
        public DroneWindow(IBL.IBL bl, ListOfDronesViewWindow listOfDronesViewWindow , IBAL.BO.Drone drone)
        {
            r = new Random();

            InitializeComponent();
            droneBL = bl;
            this.listOfDronesViewWindow = listOfDronesViewWindow;
            this.drone = drone;

            WindowTitle.Content = "Operations On Drone";

            DroneID.Text = drone.Id + "";
            DroneID.IsEnabled = false;

            Weight.ItemsSource = Enum.GetValues(typeof(IBAL.BO.Enums.WeightCategories));
            Weight.SelectedIndex = (int)drone.MaxWeight;
            Weight.IsEnabled = false;

            AddingButton.Visibility = Visibility.Hidden;


            Model.Text = drone.Model;
            BatteryLabel.Content = drone.Battery;
            StatusLabel.Content = drone.Status.ToString();
            LongtudeText.Content = drone.Location.Longitude;
            LatitudeText.Content = drone.Location.Latitude;

            UpdateModel.IsEnabled = false;


            if (drone.Status != IBAL.BO.Enums.DroneStatuses.FREE)
            {
                GoToCharge.IsEnabled = false;
                AssiningParcelToDrone.IsEnabled = false;
            }

            ReliceDroneFromCharge.IsEnabled = false;
            if (drone.Status != IBAL.BO.Enums.DroneStatuses.MAINTENANCE)
            {
                MinutesInput.IsEnabled = false;
            }
            else
            {
                if (drone.Battery < 20)
                {
                    BatteryLabel.Foreground = Brushes.Orange;
                }
            }

            if (drone.Status != IBAL.BO.Enums.DroneStatuses.DELIVERY)
            {
                PickingUpAParcel.IsEnabled = false;
                DliveringParcel.IsEnabled = false;
            }

            if(drone.ParcelInDelivery != null)
            {

                ParcelLabel.Content = drone.ParcelInDelivery.Id;

                if(droneBL.GetParcel(bl.GetDrone(drone.Id).ParcelInDelivery.Id).PickupTime != null)
                {
                    PickingUpAParcel.IsEnabled = false;
                }
                else
                {
                    DliveringParcel.IsEnabled = false;
                }
            }
            else
            {
                ParcelLabel.Content = "no parcel";
                PickingUpAParcel.IsEnabled = false;
                DliveringParcel.IsEnabled = false;
            }

            RecommandingCharge((int)drone.Battery);
        }

        private void AddDroneButton(object sender, RoutedEventArgs e)
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

                MessageBox.Show(ShowException(ex));

                
            }
        }

        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ModelUpdatedButtonClick(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (UpdateModel != null)
            {
                UpdateModel.IsEnabled = true;
                UpdateModel.Background = Brushes.Orange;
            }
        }

        private void UpdateModel_Click(object sender, RoutedEventArgs e)
        {
            UpdateModel.Background = Brushes.LightGray;

            try
            {
                droneBL.UpdateNameForADrone(drone.Id, Model.Text);
                listOfDronesViewWindow.UpdateList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ShowException(ex));
            }
            
        }

        private void GoToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneBL.ChargeDrone(drone.Id);
                listOfDronesViewWindow.UpdateList();
                //if the drone is in charge we can open the option to un charge it.
                MinutesInput.IsEnabled = true;
                GoToCharge.IsEnabled = false;
                AssiningParcelToDrone.IsEnabled = false;

                drone = droneBL.GetDrone(drone.Id);

                StatusLabel.Content = drone.Status.ToString();
                LongtudeText.Content = drone.Location.Longitude;
                LatitudeText.Content = drone.Location.Latitude;
                
                RecommandingCharge(100);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ShowException(ex));
            }

        }

        private void ReliceDroneFromCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int minutes = int.Parse(MinutesInput.Text);
                
                droneBL.UnChargeDrone(drone.Id, minutes);

                listOfDronesViewWindow.UpdateList();
                ReliceDroneFromCharge.Background = Brushes.LightGray;
                ReliceDroneFromCharge.IsEnabled = false;
                MinutesInput.IsEnabled = false;
                AssiningParcelToDrone.IsEnabled = true;

                //after updating was seccussful we can update the drone we got from the user to be the new drone.
                drone = droneBL.GetDrone(drone.Id);

                BatteryLabel.Content = drone.Battery;
                if(drone.Battery != 100)
                {
                    GoToCharge.IsEnabled = true;
                }
                StatusLabel.Content = drone.Status;
                
                RecommandingCharge((int)drone.Battery);
                

            }
            catch (Exception ex)
            {
                MinutesInput.Foreground = Brushes.Red;
                ReliceDroneFromCharge.Background = Brushes.Orange;
                MessageBox.Show(ShowException(ex));
            }
        }
       
        private void AssiningParcelToDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneBL.AssignParcelToADrone(drone.Id);

                this.drone = droneBL.GetDrone(drone.Id);

                StatusLabel.Content = drone.Status;
                ParcelLabel.Content = drone.ParcelInDelivery.Id;

                AssiningParcelToDrone.IsEnabled = false;
                GoToCharge.IsEnabled = false;
                PickingUpAParcel.IsEnabled = true;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex));
            }
        }

        private void PickingUpAParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneBL.PickingUpParcelToDrone(drone.Id);

                this.drone = droneBL.GetDrone(drone.Id);

                BatteryLabel.Content = drone.Battery;
                LongtudeText.Content = drone.Location.Longitude;
                LatitudeText.Content = drone.Location.Latitude;

                PickingUpAParcel.IsEnabled = false;
                DliveringParcel.IsEnabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex));
            }
        }

        private void DliveringParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneBL.DeliveringParcelFromADrone(drone.Id);

                this.drone = droneBL.GetDrone(drone.Id);

                StatusLabel.Content = drone.Status;
                ParcelLabel.Content = "no parcel";
                BatteryLabel.Content = drone.Battery;
                LongtudeText.Content = drone.Location.Longitude;
                LatitudeText.Content = drone.Location.Latitude;

                AssiningParcelToDrone.IsEnabled = true;
                DliveringParcel.IsEnabled = false;
                GoToCharge.IsEnabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex));
            }
        }



        private void DroneIdTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DroneID.Foreground = Brushes.Black;
        }

        private void MinutesInputChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            MinutesInput.Foreground = Brushes.Black;
            ReliceDroneFromCharge.IsEnabled = true;
            ReliceDroneFromCharge.Content = "Update Drone Battary";
            ReliceDroneFromCharge.Background = Brushes.Orange;
        }



        private string ShowException(Exception e) 
        {
            return ShowException(e, ""); 
        }

        private string ShowException(Exception e, string s)
        {
            if (e == null) return s;

            return ShowException(e.InnerException, s + e.Message + "\n");
        }

        private void RecommandingCharge(int battryLevel)
        {
            if (battryLevel < 20)
            {
                GoToCharge.Background = Brushes.Orange;
                GoToCharge.Content = "Rcomanding Charge";
                BatteryLabel.Foreground = Brushes.Orange;
            }
            else{
                GoToCharge.Background = Brushes.LightGray;
                GoToCharge.Content = "Go To Charge";
                BatteryLabel.Foreground = Brushes.Black;
            }
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
