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
                }
                else
                {
                    //pickup needs to be turned on
                    parcelState = PARCEL_STATE.PICKUP;
                }
            }

            DeliveryPanel.DataContext = parcelState;
            
        }

        #region Operational functions
       
        /// <summary>
        /// coeses the window, so we wont have to use the regular close button..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// draging the window by holding it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion

        #region Adding functions
        /// <summary>
        /// button that adds the drone to the databese aster inserting the reqired properties to the textboxs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDroneButton(object sender, RoutedEventArgs e)
        {
            try
            {
                drone.Status = BO.Enums.DroneStatuses.MAINTENANCE;
                drone.Battery = r.Next() % 20;
                drone.ParcelInDelivery = null;
                drone.Location = null;

                if (bl.AddDrone(drone))
                {
                    MessageBox.Show("drone added seccussfully");

                    listsViewWindow.UpdateDroneList();

                    //listsViewWindow.AddDrone(drone);

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
        #endregion

        #region update functions

        /// <summary>
        /// showing the button that updates the model of the drone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelUpdatedChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (UpdateModel != null && drone != null)
            {
                UpdateModel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// updates the model of the drone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// the button the sends the drone to charge if it is not already in charge.
        /// and realese it from charge if it is in charge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        /// <summary>
        /// return the color of the text to be black after it was made red after wrong input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneIdTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DroneID.Foreground = Brushes.Black;
        }

        /// <summary>
        /// geting the parcel to next step of its delivery. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// show the parcel window of the drone. the button apears only when the drone is carring a parcel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #endregion

        #region Automatic

        /// <summary>
        /// a property that suppose to invoke the automatic operation of the drone.
        /// </summary>
        private BackgroundWorker worker;

        /// <summary>
        /// the function of the drone, by starting the worker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            if (worker == null)
            {
                //changing the visability of the pannel stack.
                OptionStack.Visibility = Visibility.Hidden;
                AutomaticStack.Visibility = Visibility.Visible;

                worker = new BackgroundWorker();

                //adding the functions to the events of the worker.
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                //initilizing teh flags of the worker
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;

                //starting the worker, with the parameter that represents the speds of charging the battery..
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

        /// <summary>
        /// calls the function in the bl that do the automatic work.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //the argumen that is sent by the async.
            int length = (int)e.Argument;

            bl.AutomaticOperation(worker, drone.Id , length);
        }

        /// <summary>
        /// do the update of the presentation level every time that it requaired by the bl function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            //updat the list
            listsViewWindow.UpdateLists();
            //update the data context of the stack
            AutomaticStack.DataContext = this.drone = bl.GetDrone(drone.Id);

        }

        /// <summary>
        /// once the mnual butoon was pressded the pannel going back to be the regualar updating pannel with the same drone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OptionStack.DataContext = this.drone = bl.GetDrone(drone.Id);
        }

        #endregion

        #region Exceptions
        /// <summary>
        /// show al the inner exceptions.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private string ShowException(Exception e)
        {
            return ShowException(e, "");
        }

        /// <summary>
        /// the actuale mathed to invoke.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private string ShowException(Exception e, string s)
        {
            if (e == null) return s;
            return ShowException(e.InnerException, s + e.Message + "\n");
        }

        #endregion
    }
}
