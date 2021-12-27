using BlApi;
using System.Windows;

namespace PL
{

    public enum AccssesAtholerazetion { MANAGER , EMPLOYEE , CUSTOMER , GUEST }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AccssesAtholerazetion accssesAtholerazetion = AccssesAtholerazetion.GUEST;

        private BlApi.IBL bl;

        public MainWindow()
        {
            InitializeComponent();

            bl = BlApi.BlFactory.GetBl();

            LoginStack.DataContext = accssesAtholerazetion;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ListOfDronesViewWindow listOfDronesViewWindow = new ListOfDronesViewWindow(bl) ;
            //listOfDronesViewWindow.Show();//.Activate();

            new ListsViewWindow(bl , accssesAtholerazetion).ShowDialog();

        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.MANAGER;
            LoginStack.DataContext = accssesAtholerazetion;
        }

        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.EMPLOYEE;
            LoginStack.DataContext = accssesAtholerazetion;

        }

        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.CUSTOMER;
            LoginStack.DataContext = accssesAtholerazetion;

        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.GUEST;
            LoginStack.DataContext = accssesAtholerazetion;

        }

        private void SignIn_CLick(object sender, RoutedEventArgs e)
        {

        }

        private void ContactUs_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
