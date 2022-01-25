
using BlApi;
using System;
using System.Windows;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        
        private IBL bl = BlFactory.GetBl();
        private BO.Parcel parcel;
        private ListsViewWindow listsViewWindow;
        private int ID;

        /// <summary>
        /// ctor for adding new Parcel
        /// </summary>
        /// <param name="listsViewWindow"></param>
        public ParcelWindow(ListsViewWindow listsViewWindow)
        {
            ID = bl.GetNextSerialNumberForParcel();
            this.parcel = new();
            this.listsViewWindow = listsViewWindow;

            InitializeComponent();

            AddingStack.Visibility = Visibility.Visible;
            SenderOrReciver.DataContext = bl.GetCustomers();
            WeightChoose.DataContext = Enum.GetValues<BO.Enums.WeightCategories>();
            PriorityChoose.DataContext = Enum.GetValues<BO.Enums.Priorities>();
            IdInput.DataContext = ID;
        }

        /// <summary>
        /// ctor for updating an existing parcel.
        /// </summary>
        /// <param name="listsViewWindow"></param>
        /// <param name="parcel"></param>
        public ParcelWindow(ListsViewWindow listsViewWindow, BO.Parcel parcel)
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
        /// adding botton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddingParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                this.parcel = new BO.Parcel()
                {
                    Id = ID,
                    Sender = new BO.CoustomerForParcel()
                    {
                        Id = ((BO.CustomerForList)SenderSlector.SelectedItem).Id,
                        CustomerName = ((BO.CustomerForList)SenderSlector.SelectedItem).Name,
                    },
                    Reciver = new BO.CoustomerForParcel()
                    {
                        Id = ((BO.CustomerForList)ReciverSelector.SelectedItem).Id,
                        CustomerName = ((BO.CustomerForList)ReciverSelector.SelectedItem).Name,
                    },
                    Weight = (BO.Enums.WeightCategories)WeightChoose.SelectedItem,
                    Priority = (BO.Enums.Priorities)PriorityChoose.SelectedItem,
                    Drone = null,
                    DefinedTime = DateTime.Now,
                    AssigedTime = null,
                    PickupTime = null,
                    DeliveringTime = null
                };

               if (bl.AddParcel(parcel))
                {
                    MessageBox.Show("parcel added seccussfully");

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
        /// opens the drone that is carring the parcel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow droneWindow = new DroneWindow(listsViewWindow, bl.GetDrone(this.parcel.Drone.Id));
            droneWindow.ShowDialog();
        }

        #region Exceptions
        /// <summary>
        /// function that show the messege of the exception.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private string ShowException(Exception e)
        {
            return ShowException(e, "");
        }
        
        private string ShowException(Exception e, string s)
        {
            if (e == null) return s;

            return ShowException(e.InnerException, s + e.Message + "\n");
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
        /// draging the window
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
