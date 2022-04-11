using System;
using System.Windows;
using BL;
using BL.Abstracts;
using BL.Models;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow
    {
        
        private readonly IBl bl = BlFactory.GetBl();
        private Parcel parcel;
        private readonly ListsViewWindow listsViewWindow;
        private readonly int ID;

        /// <summary>
        /// ctor for adding new Parcel
        /// </summary>
        /// <param name="listsViewWindow"></param>
        public ParcelWindow(ListsViewWindow listsViewWindow)
        {
            ID = bl.GetNextSerialNumberForParcel();
            parcel = new Parcel();
            this.listsViewWindow = listsViewWindow;

            InitializeComponent();

            AddingStack.Visibility = Visibility.Visible;
            SenderOrReciver.DataContext = bl.GetCustomers();
            WeightChoose.DataContext = Enum.GetValues<Enums.WeightCategories>();
            PriorityChoose.DataContext = Enum.GetValues<Enums.Priorities>();
            IdInput.DataContext = ID;
        }

        /// <summary>
        /// ctor for updating an existing parcel.
        /// </summary>
        /// <param name="listsViewWindow"></param>
        /// <param name="parcel"></param>
        public ParcelWindow(ListsViewWindow listsViewWindow, Parcel parcel)
        {
            this.parcel = parcel;
            this.listsViewWindow = listsViewWindow;

            InitializeComponent();
            ParcelProgressBar.DataContext = parcel;
            OptionStack.Visibility = Visibility.Visible;
            OptionStack.DataContext = parcel;
            //SenderStack.DataContext = parcel.Sender;
        }

        #region AddingFunctions
        /// <summary>
        /// adding button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddingParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                parcel = new Parcel()
                {
                    Id = ID,
                    Sender = new CoustomerForParcel()
                    {
                        Id = ((CustomerForList)SenderSlector.SelectedItem).Id,
                        CustomerName = ((CustomerForList)SenderSlector.SelectedItem).Name,
                    },
                    Receiver = new CoustomerForParcel()
                    {
                        Id = ((CustomerForList)ReciverSelector.SelectedItem).Id,
                        CustomerName = ((CustomerForList)ReciverSelector.SelectedItem).Name,
                    },
                    Weight = (Enums.WeightCategories)WeightChoose.SelectedItem,
                    Priority = (Enums.Priorities)PriorityChoose.SelectedItem,
                    Drone = null,
                    DefinedTime = DateTime.Now,
                    AssignedTime = null,
                    PickupTime = null,
                    DeliveringTime = null
                };

               if (bl.AddParcel(parcel))
                {
                    MessageBox.Show("parcel added successfully");

                    listsViewWindow.AddParcel(parcel);

                    Close();
                }


            }
            catch(Exception ex)
            {
                MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        /// <summary>
        /// opens the drone that is carrying the parcel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDrone_Click(object sender, RoutedEventArgs e)
        {
            var droneWindow = new DroneWindow(listsViewWindow, bl.GetDrone(parcel.Drone.Id));
            droneWindow.ShowDialog();
        }

        #region Exceptions

        private string ShowException(Exception e, string s = "")
        {
            return e == null ? s : ShowException(e.InnerException, s + e.Message + "\n");
        }
        #endregion




        #region Oprational fanctions
        /// <summary>
        /// clothing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// dragging the window
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
    }
}
