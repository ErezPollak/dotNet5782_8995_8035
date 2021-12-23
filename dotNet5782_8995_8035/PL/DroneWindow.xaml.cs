﻿using System;
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
        BlApi.IBL bl;
        BO.Drone drone;
        ListsViewWindow listOfDronesViewWindow;

        PARCEL_STATE parcelState;

        /// <summary>
        /// ctor for adding a drone.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listOfDronesViewWindow"></param>
        public DroneWindow(BlApi.IBL bl, ListsViewWindow listOfDronesViewWindow)
        {
            drone = new();
            r = new Random();
            InitializeComponent();
            AddingStack.Visibility = Visibility.Visible;

            this.bl = bl;
            this.listOfDronesViewWindow = listOfDronesViewWindow;

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
        public DroneWindow(BlApi.IBL bl, ListsViewWindow listOfDronesViewWindow, BO.Drone drone)
        {
            r = new Random();

            InitializeComponent();
            OptionStack.Visibility = Visibility.Visible;
            this.bl = bl;
            this.listOfDronesViewWindow = listOfDronesViewWindow;
            this.drone = drone;

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

            }

            if (drone.ParcelInDelivery != null)
            {

                if (this.bl.GetParcel(bl.GetDrone(drone.Id).ParcelInDelivery.Id).PickupTime != null)
                {
                    parcelState = PARCEL_STATE.DELIVER;

                }
                else
                {
                    //pickup needs to be turned on
                    parcelState = PARCEL_STATE.PICKUP;
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

                if (bl.AddDrone(drone))
                {
                    MessageBox.Show("drone added seccussfully");

                    listOfDronesViewWindow.UpdateDroneList();

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
                bl.UpdateNameForADrone(drone.Id, Model.Text);
                listOfDronesViewWindow.UpdateDroneList();
                UpdateModel.Visibility = Visibility.Collapsed;
                OptionStack.DataContext = this.drone;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex));
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

                    listOfDronesViewWindow.UpdateDroneList();
                    //after updating was seccussful we can update the drone we got from the user to be the new drone.
                    drone = bl.GetDrone(drone.Id);

                    RecommandingCharge((int)drone.Battery);

                    OptionStack.DataContext = this.drone;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ShowException(ex));
                }
            }
            else
            {

                try
                {
                    bl.ChargeDrone(drone.Id);

                    listOfDronesViewWindow.UpdateDroneList();

                    drone = bl.GetDrone(drone.Id);

                    OptionStack.DataContext = this.drone;

                    RecommandingCharge(100);

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ShowException(ex));
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

                            listOfDronesViewWindow.UpdateDroneList();

                            this.drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = this.drone;

                            ProgresDelivery.Value = 66;

                            parcelState = PARCEL_STATE.PICKUP;
                            DeliveringOption.DataContext = parcelState;

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
                            bl.PickingUpParcelToDrone(drone.Id);

                            listOfDronesViewWindow.UpdateDroneList();

                            this.drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = this.drone;

                            parcelState = PARCEL_STATE.DELIVER;
                            DeliveringOption.DataContext = parcelState;

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
                            bl.DeliveringParcelFromADrone(drone.Id);

                            listOfDronesViewWindow.UpdateDroneList();

                            this.drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = this.drone;

                            //DeliveringOption.Content = "Assign Parcel To Drone";
                            parcelState = PARCEL_STATE.ASSIGN;
                            DeliveringOption.DataContext = parcelState;

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

        private void Open_Prcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Parcel parcel = bl.GetParcel(drone.ParcelInDelivery.Id);
                new ParcelWindow(bl, parcel).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
