
using System;
using System.Windows;

namespace PL
{

    internal enum ACCESS_ATHOLERAZATION {GUEST, CUSTOMER, EMPLOYEE, MANAGER}

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// the atholezetion that goes to the lists windiw. the defult is guest.
        /// </summary>
        private ACCESS_ATHOLERAZATION accssesAtholerazetion = ACCESS_ATHOLERAZATION.GUEST;

        /// <summary>
        /// ctor for the window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            LoginStack.DataContext = accssesAtholerazetion;
        }

        /// <summary>
        /// open the login stack with manager atholerazetion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = ACCESS_ATHOLERAZATION.MANAGER;
            LoginStack.DataContext = accssesAtholerazetion;
            //LoginStack.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// open the login stack with Employee atholerazetion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = ACCESS_ATHOLERAZATION.EMPLOYEE;
            LoginStack.DataContext = accssesAtholerazetion;
            //LoginStack.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// open the login stack with Customer atholerazetion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = ACCESS_ATHOLERAZATION.CUSTOMER;
            LoginStack.DataContext = accssesAtholerazetion;
            //LoginStack.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// open the login stack with Guest atholerazetion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            this.accssesAtholerazetion = ACCESS_ATHOLERAZATION.GUEST;
            LoginStack.DataContext = accssesAtholerazetion;
            new ListsViewWindow(accssesAtholerazetion).ShowDialog();

        }

        /// <summary>
        /// open the contact us window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignIn_CLick(object sender, RoutedEventArgs e)
        {
            new SignUpWindow().ShowDialog();
        }

        /// <summary>
        /// open the contact us window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactUs_Click(object sender, RoutedEventArgs e)
        {
            ContactUsWindow contactUsWindow = new();
            contactUsWindow.ShowDialog();
        }

        /// <summary>
        /// opening the lists eindow eith the cirrect atholerazetion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (accssesAtholerazetion == ACCESS_ATHOLERAZATION.CUSTOMER)
            {
                try
                {
                    new ListsViewWindow(accssesAtholerazetion, int.Parse(UserName.Text)).ShowDialog();
                }
                catch(Exception ex)
                {

                    MessageBox.Show("no customer as " + UserName.Text + " in the database");
                    UserName.Clear();
                    Password.Clear();
                }
            }
            else
                new ListsViewWindow(accssesAtholerazetion).ShowDialog();
            accssesAtholerazetion = ACCESS_ATHOLERAZATION.GUEST;
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

        /// <summary>
        /// closing the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// draging the window.
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
    }
}
