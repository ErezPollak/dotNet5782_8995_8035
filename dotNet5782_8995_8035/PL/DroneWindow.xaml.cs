using BO;
using BlApi;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace PL
{

    public enum PARCEL_STATE { ASSIGN, PICKUP, DELIVER }

    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {

        public static bool idAutomatic = false;

        private bool isAutomatic;

        private Random r;
        private IBL bl = BlFactory.GetBl();
        private Drone drone;
        private ListsViewWindow listsViewWindow;

        private PARCEL_STATE parcelState = PARCEL_STATE.ASSIGN;


        private bool IsAuto = false;
        /// <summary>
        /// ctor for adding a drone.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listsViewWindow"></param>
        public DroneWindow(ListsViewWindow listsViewWindow)
        {
            drone = new();
            r = new Random();
            InitializeComponent();
            AddingStack.Visibility = Visibility.Visible;

            this.listsViewWindow = listsViewWindow;

            //Weight.ItemsSource = Enum.GetValues<BO.Enums.WeightCategories>();

            AddingStack.DataContext = drone;

            WeightInput.DataContext = Enum.GetValues<BO.Enums.WeightCategories>();

        }

        /// <summary>
        /// ctor for showing and updating a drone. 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listOfDronesViewWindow"></param>
        /// <param name="drone"></param>
        public DroneWindow(ListsViewWindow listOfDronesViewWindow, BO.Drone drone)
        {
            r = new Random();

            InitializeComponent();

            OptionStack.Visibility = Visibility.Visible;
            this.listsViewWindow = listOfDronesViewWindow;
            this.drone = drone;
            DeliveryPanel.DataContext = parcelState;
            OptionStack.DataContext = this.drone;

            DroneID.Text = drone.Id + "";
            DroneID.IsEnabled = false;

            


            if (drone.Status != BO.Enums.DroneStatuses.MAINTENANCE)
            {
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
                parcelState = PARCEL_STATE.ASSIGN;
                //ProgresDelivery.DataContext = parcelState;
            }

            if (drone.ParcelInDelivery != null)
            {

                if (this.bl.GetParcel(bl.GetDrone(drone.Id).ParcelInDelivery.Id).PickupTime != null)
                {
                    parcelState = PARCEL_STATE.DELIVER;
                    //ProgresDelivery.DataContext = parcelState;
                }
                else
                {
                    //pickup needs to be turned on
                    parcelState = PARCEL_STATE.PICKUP;
                    //ProgresDelivery.DataContext = parcelState;
                }
            }

            
            DeliveryPanel.DataContext = parcelState;
            
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

                if (bl.AddDrone(drone))
                {
                    MessageBox.Show("drone added seccussfully");

                    //listsViewWindow.UpdateDroneList();

                    listsViewWindow.AddDrone(drone);

                    Close();
                }

            }
            catch (Exception ex)
            {
                if (ex is BO.IdAlreadyExistsException || ex is FormatException)
                {
                    DroneID.Foreground = Brushes.Red;
                }

                MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ModelUpdatedChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (UpdateModel != null && drone != null)
            {
                UpdateModel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateNameForADrone(drone.Id, Model.Text);
                listsViewWindow.UpdateDroneList();
                UpdateModel.Visibility = Visibility.Collapsed;
                OptionStack.DataContext = this.drone;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Model.Text = bl.GetDrone(drone.Id).Model;
            }

        }

        private void ChargeAndUnchargeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.drone.Status == BO.Enums.DroneStatuses.MAINTENANCE)
            {
                try
                {
                    bl.UnChargeDrone(drone.Id);

                    listsViewWindow.UpdateLists();
                    //after updating was seccussful we can update the drone we got from the user to be the new drone.
                    drone = bl.GetDrone(drone.Id);

                    RecommandingCharge((int)drone.Battery);

                    OptionStack.DataContext = this.drone;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {

                try
                {
                    bl.ChargeDrone(drone.Id);

                    listsViewWindow.UpdateLists();

                    drone = bl.GetDrone(drone.Id);

                    OptionStack.DataContext = this.drone;

                    RecommandingCharge(100);

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        private void DroneIdTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DroneID.Foreground = Brushes.Black;
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
                BatteryLabel.Foreground = Brushes.Orange;
            }
            else
            {
                BatteryLabel.Foreground = Brushes.Black;
            }
        }

        private void DeliveringOption_Click(object sender, RoutedEventArgs e)
        {
            switch (parcelState)
            {
                case PARCEL_STATE.ASSIGN:
                    {
                        try
                        {
                            bl.AssignParcelToADrone(drone.Id);

                            this.drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = this.drone;

                            parcelState = PARCEL_STATE.PICKUP;
                            DeliveryPanel.DataContext = parcelState;

                            listsViewWindow.UpdateLists();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    break;
                case PARCEL_STATE.PICKUP:
                    {
                        try
                        {
                            bl.PickingUpParcelToDrone(drone.Id);

                            this.drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = this.drone;

                            parcelState = PARCEL_STATE.DELIVER;
                            DeliveryPanel.DataContext = parcelState;

                            listsViewWindow.UpdateLists();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    break;
                case PARCEL_STATE.DELIVER:
                    {
                        try
                        {
                            bl.DeliveringParcelFromADrone(drone.Id);

                            listsViewWindow.UpdateLists();

                            this.drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = this.drone;

                            parcelState = PARCEL_STATE.ASSIGN;
                            DeliveryPanel.DataContext = parcelState;
                            listsViewWindow.UpdateParcelList();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void Open_Prcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Parcel parcel = bl.GetParcel(drone.ParcelInDelivery.Id);
                new ParcelWindow(listsViewWindow, parcel).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private BackgroundWorker worker;

        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            if (worker == null)
            {

                OptionStack.Visibility = Visibility.Hidden;
                AutomaticStack.Visibility = Visibility.Visible;


                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;

                worker.RunWorkerAsync(100);
            }
            else
            {
                if (worker.WorkerSupportsCancellation == true)
                {
                    OptionStack.Visibility = Visibility.Visible;
                    AutomaticStack.Visibility = Visibility.Hidden;

                    // Cancel the asynchronous operation.
                    worker.CancelAsync();
                    worker = null;
                }
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int length = (int)e.Argument;

            bl.AutomaticOperation(worker, drone.Id , length);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            listsViewWindow.UpdateLists();
            AutomaticStack.DataContext = this.drone = bl.GetDrone(drone.Id);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OptionStack.DataContext = this.drone = bl.GetDrone(drone.Id);
        }
    }
}
