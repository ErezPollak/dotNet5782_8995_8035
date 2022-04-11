using System;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using BL;
using BL.Abstracts;
using BL.Exceptions;
using BL.Models;

namespace PL
{

    public enum PARCEL_STATE { ASSIGN, PICKUP, DELIVER }

    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow
    {
        private readonly Random r;
        private readonly IBl bl = BlFactory.GetBl();
        private Drone drone;
        private readonly ListsViewWindow listsViewWindow;
        private PARCEL_STATE parcelState = PARCEL_STATE.ASSIGN;

        /// <summary>
        /// ctor for adding a drone.
        /// </summary>
        /// <param name="listsViewWindow"></param>
        public DroneWindow(ListsViewWindow listsViewWindow)
        {
            drone = new Drone();
            r = new Random();
            InitializeComponent();
            
            AddingStack.Visibility = Visibility.Visible;

            this.listsViewWindow = listsViewWindow;
            
            AddingStack.DataContext = drone;

            WeightInput.DataContext = Enum.GetValues<Enums.WeightCategories>();

        }

        /// <summary>
        /// ctor for showing and updating a drone. 
        /// </summary>
        /// <param name="listOfDronesViewWindow"></param>
        /// <param name="drone"></param>
        public DroneWindow(ListsViewWindow listOfDronesViewWindow, Drone drone)
        {
            r = new Random();

            InitializeComponent();

            OptionStack.Visibility = Visibility.Visible;
            listsViewWindow = listOfDronesViewWindow;
            this.drone = drone;
            DeliveryPanel.DataContext = parcelState;
            OptionStack.DataContext = this.drone;

            DroneID.Text = drone.Id + "";
            DroneID.IsEnabled = false;

            if (drone.Status != Enums.DroneStatuses.DELIVERY)
            {
                //assign need to be turns on
                parcelState = PARCEL_STATE.ASSIGN;
            }

            if (drone.ParcelInDelivery != null)
            {
                parcelState = bl.GetParcel(bl.GetDrone(drone.Id).ParcelInDelivery.Id).PickupTime != null ? PARCEL_STATE.DELIVER : PARCEL_STATE.PICKUP;
            }

            DeliveryPanel.DataContext = parcelState;
            
        }

        #region Operational functions
       
        /// <summary>
        /// closes the window, so we wont have to use the regular close button..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// dragging the window by holding it.
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
        /// button that adds the drone to the dataBase aster inserting the required properties to the textBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDroneButton(object sender, RoutedEventArgs e)
        {
            try
            {
                drone.Status = Enums.DroneStatuses.MAINTENANCE;
                drone.Battery = r.Next() % 20;
                drone.ParcelInDelivery = null;
                drone.Location = null;

                if (!bl.AddDrone(drone)) return;
                
                MessageBox.Show("drone added successfully");

                listsViewWindow.UpdateDroneList();

                Close();

            }
            catch (Exception ex)
            {
                if (ex is IdAlreadyExistsException or FormatException)
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
                OptionStack.DataContext = drone;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Model.Text = bl.GetDrone(drone.Id).Model;
            }

        }

        /// <summary>
        /// the button the sends the drone to charge if it is not already in charge.
        /// and release it from charge if it is in charge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeAndUnchargedButton_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Status == Enums.DroneStatuses.MAINTENANCE)
            {
                try
                {
                    bl.UnChargeDrone(drone.Id);

                    listsViewWindow.UpdateLists();
                    //after updating was successful we can update the drone we got from the user to be the new drone.
                    drone = bl.GetDrone(drone.Id);

                    OptionStack.DataContext = drone;

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

                    OptionStack.DataContext = drone;
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
        /// getting the parcel to next step of its delivery. 
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

                            drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = drone;

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

                            drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = drone;

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

                            drone = bl.GetDrone(drone.Id);

                            OptionStack.DataContext = drone;

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
                
            }
        }

        /// <summary>
        /// show the parcel window of the drone. the button appears only when the drone is carrying a parcel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Parcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var parcel = bl.GetParcel(drone.ParcelInDelivery.Id);
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
                //changing the visibility of the panel stack.
                OptionStack.Visibility = Visibility.Hidden;
                AutomaticStack.Visibility = Visibility.Visible;

                worker = new BackgroundWorker();

                //adding the functions to the events of the worker.
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                //initializing teh flags of the worker
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;

                //starting the worker, with the parameter that represents the speeds of charging the battery..
                worker.RunWorkerAsync(100);
            }
            else
            {
                if (worker.WorkerSupportsCancellation != true) return;
                
                OptionStack.Visibility = Visibility.Visible;
                AutomaticStack.Visibility = Visibility.Hidden;

                // Cancel the asynchronous operation.
                worker.CancelAsync();
                worker = null;
            }
        }

        /// <summary>
        /// calls the function in the bl that do the automatic work.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //the argument that is sent by the async.
            var length = (int)e.Argument;

            bl.AutomaticOperation(worker, drone.Id , length);
        }

        /// <summary>
        /// do the update of the presentation level every time that it required by the bl function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            //update the list
            listsViewWindow.UpdateLists();
            //update the data context of the stack
            AutomaticStack.DataContext = drone = bl.GetDrone(drone.Id);

        }

        /// <summary>
        /// once the manual button was presided the panel going back to be the regular updating panel with the same drone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OptionStack.DataContext = drone = bl.GetDrone(drone.Id);
        }

        #endregion

        #region Exceptions

        /// <summary>
        /// the actual method to invoke.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private string ShowException(Exception e, string s = "")
        {
            return e == null ? s : ShowException(e.InnerException, s + e.Message + "\n");
        }

        #endregion
    }
}
