
using System;
using System.Windows;

namespace PL
{

    internal enum ACCESS_AUTHORIZATION {GUEST, CUSTOMER, EMPLOYEE, MANAGER}

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// the authorize that goes to the lists window. the default is guest.
        /// </summary>
        private ACCESS_AUTHORIZATION accessesAuthorizer = ACCESS_AUTHORIZATION.GUEST;

        /// <summary>
        /// ctor for the window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            LoginStack.DataContext = accessesAuthorizer;
        }

        /// <summary>
        /// open the login stack with manager authorizer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            accessesAuthorizer = ACCESS_AUTHORIZATION.MANAGER;
            LoginStack.DataContext = accessesAuthorizer;
        }

        /// <summary>
        /// open the login stack with Employee authorize.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            accessesAuthorizer = ACCESS_AUTHORIZATION.EMPLOYEE;
            LoginStack.DataContext = accessesAuthorizer;

        }

        /// <summary>
        /// open the login stack with Customer authorizer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            accessesAuthorizer = ACCESS_AUTHORIZATION.CUSTOMER;
            LoginStack.DataContext = accessesAuthorizer;

        }

        /// <summary>
        /// open the login stack with Guest authorizer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            accessesAuthorizer = ACCESS_AUTHORIZATION.GUEST;
            LoginStack.DataContext = accessesAuthorizer;
            new ListsViewWindow(accessesAuthorizer).ShowDialog();

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
        /// opening the lists window with the correct authorizer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (accessesAuthorizer == ACCESS_AUTHORIZATION.CUSTOMER)
            {
                try
                {
                    new ListsViewWindow(accessesAuthorizer, int.Parse(UserName.Text)).ShowDialog();
                }
                catch(Exception)
                {

                    MessageBox.Show("no customer as " + UserName.Text + " in the database");
                    UserName.Clear();
                    Password.Clear();
                }
            }
            else
                new ListsViewWindow(accessesAuthorizer).ShowDialog();
            accessesAuthorizer = ACCESS_AUTHORIZATION.GUEST;
            LoginStack.DataContext = accessesAuthorizer;
            MistakenPasswordOrName.Visibility = Visibility.Collapsed;


            //if (UserName.Text == "Erez" && Password.Password == "8995")
            //{
            //    new ListsViewWindow(accessesAuthorizer).ShowDialog();
            //    accessesAuthorizer = ACCESS_AUTHORIZATION.GUEST;
            //    LoginStack.DataContext = accessesAuthorizer;
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
        /// dragging the window.
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
