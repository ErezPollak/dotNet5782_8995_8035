using System;
using System.Windows;

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
            int id;
            int.TryParse(droneId.Text, out id);

            int weight;
            int.TryParse(Weight.SelectedItem.ToString(), out weight);

            IBAL.BO.Drone newDrone = new IBAL.BO.Drone()
            {
                Id = id,
                Model = model.Text,
                MaxWeight = (IBAL.BO.Enums.WeightCategories)weight,
                Status = IBAL.BO.Enums.DroneStatuses.FREE,
                ParcelInDelivery = null,
                Battery = r.Next() % 20
            };

            try
            {
                if (droneBL.AddDrone(newDrone))
                {
                    MessageBox.Show("drone added seccussfully");

                    string whight = null , status = null;

                    if (listOfDronesViewWindow.WeightSelecter.SelectedItem != null) 
                        whight = listOfDronesViewWindow.WeightSelecter.SelectedItem.ToString();

                    if (listOfDronesViewWindow.StatusSelector.SelectedItem != null) 
                        status = listOfDronesViewWindow.StatusSelector.SelectedItem.ToString();
                    
                    listOfDronesViewWindow.ListOfDronesView.ItemsSource = droneBL.GetDrones(d =>
                    (d.Weight.ToString() == whight || whight == "Show all" || whight == null) && 
                    (d.Status.ToString() == status || status == "Show all" || status == null));
                
                    Close();

                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
            MessageBox.Show("operation caceled");
        }
    }
}
