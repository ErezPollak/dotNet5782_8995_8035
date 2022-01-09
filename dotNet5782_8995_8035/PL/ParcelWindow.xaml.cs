
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

        public ParcelWindow(ListsViewWindow listsViewWindow)
        {
            this.parcel = new();
            this.listsViewWindow = listsViewWindow;

            InitializeComponent();

            AddingStack.Visibility = Visibility.Visible;
            SenderOrReciver.DataContext = bl.GetCustomers(_ => true);
            WeightChoose.DataContext = Enum.GetValues<BO.Enums.WeightCategories>();
            PriorityChoose.DataContext = Enum.GetValues<BO.Enums.Priorities>();
        }

        public ParcelWindow(ListsViewWindow listsViewWindow, BO.Parcel parcel)
        {
            this.parcel = parcel;
            this.listsViewWindow = listsViewWindow;

            InitializeComponent();
            ParcelProgressBar.DataContext = parcel;
            OptionStack.Visibility = Visibility.Visible;
            OptionStack.DataContext = parcel;
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

        private void AddingParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                this.parcel = new BO.Parcel()
                {
                    Id = int.Parse(IdInput.Text),
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
                if (ex is BO.IdAlreadyExistsException || ex is FormatException)
                {
                    IdInput.Foreground = Brushes.Red;
                }

                MessageBox.Show(ShowException(ex), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }

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

        private void OpenDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow droneWindow = new DroneWindow(listsViewWindow, bl.GetDrone(this.parcel.Drone.Id));
            droneWindow.ShowDialog();
        }
    }
}
