using IBL;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IBL.IBL bl;

        public MainWindow()
        {
            InitializeComponent();
            bl = new BL();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ListOfDronesViewWindow listOfDronesViewWindow = new ListOfDronesViewWindow(bl) ;
            //listOfDronesViewWindow.Show();//.Activate();

            new ListOfDronesViewWindow(bl).Show();

        }
    }
}
