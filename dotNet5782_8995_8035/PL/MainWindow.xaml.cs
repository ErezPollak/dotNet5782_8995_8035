using BlApi;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BlApi.IBL bl;

        public MainWindow()
        {
            InitializeComponent();

            //bl = BlApi.BL.GetInstance();
            bl = BlApi.BlFactory.GetBl();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ListOfDronesViewWindow listOfDronesViewWindow = new ListOfDronesViewWindow(bl) ;
            //listOfDronesViewWindow.Show();//.Activate();

            new ListsViewWindow(bl).ShowDialog();

        }
    }
}
