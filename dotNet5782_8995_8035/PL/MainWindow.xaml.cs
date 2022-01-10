
using System;
using System.Windows;

namespace PL
{

    public enum AccssesAtholerazetion {GUEST, CUSTOMER, EMPLOYEE, MANAGER}

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AccssesAtholerazetion accssesAtholerazetion = AccssesAtholerazetion.GUEST;

        public MainWindow()
        {
            InitializeComponent();

            LoginStack.DataContext = accssesAtholerazetion;

        }


        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.MANAGER;
            LoginStack.DataContext = accssesAtholerazetion;
            //LoginStack.Visibility = Visibility.Visible;
        }

        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.EMPLOYEE;
            LoginStack.DataContext = accssesAtholerazetion;
            //LoginStack.Visibility = Visibility.Visible;

        }

        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.CUSTOMER;
            LoginStack.DataContext = accssesAtholerazetion;
            //LoginStack.Visibility = Visibility.Visible;

        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = AccssesAtholerazetion.GUEST;
            LoginStack.DataContext = accssesAtholerazetion;
            new ListsViewWindow(accssesAtholerazetion).ShowDialog();

        }

        private void SignIn_CLick(object sender, RoutedEventArgs e)
        {
            new SignUpWindow().ShowDialog();
        }

        private void ContactUs_Click(object sender, RoutedEventArgs e)
        {
            ContactUsWindow contactUsWindow = new();
            contactUsWindow.ShowDialog();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (accssesAtholerazetion == AccssesAtholerazetion.CUSTOMER)
            {
                try
                {
                    new ListsViewWindow(accssesAtholerazetion, int.Parse(UserName.Text)).ShowDialog();
                }
                catch(Exception ex)
                {
                    UserName.Clear();
                    Password.Clear();
                }
            }
            else
                new ListsViewWindow(accssesAtholerazetion).ShowDialog();
            accssesAtholerazetion = AccssesAtholerazetion.GUEST;
            LoginStack.DataContext = accssesAtholerazetion;
            MistakenPasswordOrName.Visibility = Visibility.Collapsed;


            //if ((UserName.Text == "Mordechay" && Password.Password == "8035") || (UserName.Text == "Erez" && Password.Password == "8995"))
            //{
            //    new ListsViewWindow(bl, accssesAtholerazetion).ShowDialog();
            //    accssesAtholerazetion = AccssesAtholerazetion.GUEST;
            //    LoginStack.DataContext = accssesAtholerazetion;
            //    MistakenPasswordOrName.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    MistakenPasswordOrName.Visibility = Visibility.Visible;
            //}

            //UserName.Clear();
            //Password.Clear();
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
    }
}
