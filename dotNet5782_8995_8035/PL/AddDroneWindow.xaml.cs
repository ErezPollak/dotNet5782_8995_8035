using System;
using System.Windows;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        Random r;
        IBL.IBL droneBL;

        ListOfDronesViewWindow listOfDronesViewWindow;

        public AddDroneWindow(IBL.IBL bl , ListOfDronesViewWindow listOfDronesViewWindow)
        {
            r = new Random();

            this.listOfDronesViewWindow = listOfDronesViewWindow;

            InitializeComponent();

            droneBL = bl;

            Weight.ItemsSource = Enum.GetValues(typeof(IBAL.BO.Enums.WeightCategories));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(droneId.Text);

                int weight = Weight.SelectedIndex;

                IBAL.BO.Drone newDrone = new IBAL.BO.Drone()
                {
                    Id = id,
                    Model = model.Text,
                    MaxWeight = (IBAL.BO.Enums.WeightCategories)weight,
                    Status = IBAL.BO.Enums.DroneStatuses.FREE,
                    ParcelInDelivery = null,
                    Battery = r.Next() % 20
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
                    droneId.Foreground = Brushes.Red;
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
            
            s += e.Message + "\n";

            return showException(e.InnerException, s);
        }

        private void TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            droneId.Foreground = Brushes.Black;
        }
    }
}
