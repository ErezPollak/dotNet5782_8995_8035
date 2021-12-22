using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace PL
{

    public enum PARCEL_STATE { ASSIGN, PICKUP, DELIVER }

    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        Random r;
        BlApi.IBL droneBL;
        BO.Drone drone;
        ListOfDronesViewWindow listOfDronesViewWindow;

        PARCEL_STATE parcelState;

        /// <summary>
        /// ctor for adding a drone.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listOfDronesViewWindow"></param>
        public DroneWindow(BlApi.IBL bl, ListOfDronesViewWindow listOfDronesViewWindow)
        {
            drone = new();
            r = new Random();
            InitializeComponent();
            AddingStack.Visibility = Visibility.Visible;

            droneBL = bl;
            this.listOfDronesViewWindow = listOfDronesViewWindow;

            //Weight.ItemsSource = Enum.GetValues<BO.Enums.WeightCategories>();

            AddingStack.DataContext = drone;
        }

        /// <summary>
        /// ctor for showing and updating a drone. 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listOfDronesViewWindow"></param>
        /// <param name="drone"></param>
        public DroneWindow(BlApi.IBL bl, ListOfDronesViewWindow listOfDronesViewWindow, BO.Drone drone)
        {
            r = new Random();

            InitializeComponent();
            OptionStack.Visibility = Visibility.Visible;
            droneBL = bl;
            this.listOfDronesViewWindow = listOfDronesViewWindow;
            this.drone = drone;

            OptionStack.DataContext = this.drone;

            DroneID.Text = drone.Id + "";
            DroneID.IsEnabled = false;

            //Weight.ItemsSource = Enum.GetValues<BO.Enums.WeightCategories>();
            //Weight.SelectedIndex = (int)drone.MaxWeight;
            //Weight.IsEnabled = false;
           
            //Model.Text = drone.Model;
            //BatteryLabel.Content = drone.Battery;
            //StatusLabel.Content = drone.Status.ToString();
            //LongtudeText.Content = drone.Location.Longitude;
            //LatitudeText.Content = drone.Location.Latitude;

            if (drone.Status != BO.Enums.DroneStatuses.FREE)
            {
                GoToCharge.IsEnabled = false;
            }

            ReliceDroneFromCharge.IsEnabled = false;
            if (drone.Status != BO.Enums.DroneStatuses.MAINTENANCE)
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

            if (drone.Status != BO.Enums.DroneStatuses.DELIVERY)
            {
                //assign need to be turns on
                //ParcelLabel.Content = "no parcel";
                parcelState = PARCEL_STATE.ASSIGN;
                DeliveringOption.Content = "Assining Parcel To Drone";

            }

            if (drone.ParcelInDelivery != null)
            {
                //ParcelLabel.Content = drone.ParcelInDelivery.Id;

                if (droneBL.GetParcel(bl.GetDrone(drone.Id).ParcelInDelivery.Id).PickupTime != null)
                {
                    parcelState = PARCEL_STATE.DELIVER;
                    DeliveringOption.Content = "Dlivering Parcel";

                }
                else
                {
                    //pickup needs to be turned on

                    parcelState = PARCEL_STATE.PICKUP;
                    DeliveringOption.Content = "Pick Up A Parcel";
                }
            }
            DeliveringOption.DataContext = parcelState;
            RecommandingCharge((int)drone.Battery);

        }

        private void AddDroneButton(object sender, RoutedEventArgs e)
        {
            try
            {
               
                drone.Status = BO.Enums.DroneStatuses.FREE;
                drone.ParcelInDelivery = null;
                drone.Battery = r.Next() % 20;
                drone.Location = null;

                if (droneBL.AddDrone(drone))
                {
                    MessageBox.Show("drone added seccussfully");

                    listOfDronesViewWindow.UpdateList();

                    Close();
                }

            }
            catch (Exception ex)
            {
                if (ex is BO.IdAlreadyExistsException || ex is FormatException)
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

        private void ModelUpdatedChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (UpdateModel != null && drone != null)
            {
                UpdateModel.IsEnabled = true;
                UpdateModel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneBL.UpdateNameForADrone(drone.Id, Model.Text);
                listOfDronesViewWindow.UpdateList();
                UpdateModel.Visibility = Visibility.Collapsed;
                OptionStack.DataContext = this.drone;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex));
                Model.Text = droneBL.GetDrone(drone.Id).Model;
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

                drone = droneBL.GetDrone(drone.Id);

                //StatusLabel.Content = drone.Status.ToString();
                //LongtudeText.Content = drone.Location.Longitude;
                //LatitudeText.Content = drone.Location.Latitude;

                OptionStack.DataContext = this.drone;

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
                ReliceDroneFromCharge.IsEnabled = false;
                MinutesInput.IsEnabled = false;

                //after updating was seccussful we can update the drone we got from the user to be the new drone.
                drone = droneBL.GetDrone(drone.Id);

                //BatteryLabel.Content = drone.Battery;
                if (drone.Battery != 100)
                {
                    GoToCharge.IsEnabled = true;
                }
                //StatusLabel.Content = drone.Status;

                RecommandingCharge((int)drone.Battery);

                OptionStack.DataContext = this.drone;

            }
            catch (Exception ex)
            {
                MinutesInput.Foreground = Brushes.Red;
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
                GoToCharge.Content = "Rcomanding Charge";
                BatteryLabel.Foreground = Brushes.Orange;
            }
            else
            {
                GoToCharge.Content = "Go To Charge";
                BatteryLabel.Foreground = Brushes.Black;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (parcelState)
            {
                case PARCEL_STATE.ASSIGN:
                    {
                        try
                        {
                            droneBL.AssignParcelToADrone(drone.Id);

                            listOfDronesViewWindow.UpdateList();

                            parcelState = PARCEL_STATE.PICKUP;
                            DeliveringOption.DataContext = parcelState;

                            this.drone = droneBL.GetDrone(drone.Id);

                            //StatusLabel.Content = drone.Status;
                            //ParcelLabel.Content = drone.ParcelInDelivery.Id;

                            //AssiningParcelToDrone.IsEnabled = false;
                            GoToCharge.IsEnabled = false;
                            //PickingUpAParcel.IsEnabled = true;

                            ProgresDelivery.Value = 66;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ShowException(ex));
                        }
                    }
                    break;
                case PARCEL_STATE.PICKUP:
                    {
                        try
                        {
                            droneBL.PickingUpParcelToDrone(drone.Id);

                            listOfDronesViewWindow.UpdateList();

                            parcelState = PARCEL_STATE.DELIVER;
                            DeliveringOption.DataContext = parcelState;

                            this.drone = droneBL.GetDrone(drone.Id);

                            //BatteryLabel.Content = drone.Battery;
                            //LongtudeText.Content = drone.Location.Longitude;
                            //LatitudeText.Content = drone.Location.Latitude;

                            //PickingUpAParcel.IsEnabled = false;
                            //DliveringParcel.IsEnabled = true;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ShowException(ex));
                        }
                    }
                    break;
                case PARCEL_STATE.DELIVER:
                    {
                        try
                        {
                            droneBL.DeliveringParcelFromADrone(drone.Id);

                            listOfDronesViewWindow.UpdateList();

                            DeliveringOption.Content = "Assign Parcel To Drone";
                            parcelState = PARCEL_STATE.ASSIGN;
                            DeliveringOption.DataContext = parcelState;

                            this.drone = droneBL.GetDrone(drone.Id);

                            //StatusLabel.Content = drone.Status;
                           // ParcelLabel.Content = "no parcel";
                            //BatteryLabel.Content = drone.Battery;
                            //LongtudeText.Content = drone.Location.Longitude;
                            //LatitudeText.Content = drone.Location.Latitude;

                            //AssiningParcelToDrone.IsEnabled = true;
                            //DliveringParcel.IsEnabled = false;
                            GoToCharge.IsEnabled = true;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ShowException(ex));
                        }
                    }
                    break;
                default:
                    break;
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

        private void Open_Prcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Parcel parcel = droneBL.GetParcel(drone.ParcelInDelivery.Id);

            }
            catch(Exception ex)
            {

            }
        }
    }
}
