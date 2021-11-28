using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        Random r;
        IBL.IBL droneBL;

        public AddDroneWindow(IBL.IBL bl)
        {
            r = new Random();

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
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
