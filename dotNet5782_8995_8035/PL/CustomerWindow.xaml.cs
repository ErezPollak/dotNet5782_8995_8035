using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private static readonly Random Random = new();

        private readonly IBL bl =  BlFactory.GetBl();
        private readonly ListsViewWindow listsViewWindow;
        private Customer customer;

        /// <summary>
        /// ctor for add customer
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listsViewWindow"></param>
        public CustomerWindow(ListsViewWindow listsViewWindow)
        {
            InitializeComponent();

            this.listsViewWindow = listsViewWindow;
            customer = new(Location: new Location(), FromCustomer: new List<BO.ParcelByCustomer>(),ToCustomer: new List<BO.ParcelByCustomer>());
            AddingStack.DataContext = customer;

            //shows the pannel that controls the adding of an new customer.
            AddingStack.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// ctor for Update customer
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listsViewWindow"></param>
        /// <param name="customer"></param>
        public CustomerWindow(ListsViewWindow listsViewWindow, Customer customer)
        {
            InitializeComponent();

            this.listsViewWindow = listsViewWindow;
            this.customer = customer;
            OptionStack.DataContext = customer;

            //shows the pannel that controls the updateing of an existing customer.
            OptionStack.Visibility = Visibility.Visible;

            //takes from the customer the lists of the parcels that are being sent from or to him.
            listViewOfBaseStatin_FromCustomer.DataContext = customer.FromCustomer;
            listViewOfBaseStatin_ToCustomer.DataContext = customer.ToCustomer;

        }

        #region operative functions

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

        #endregion

        #region Adding functions
        /// <summary>
        /// add base station buton click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickAddCustomerButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (bl.AddCustumer(customer))
                {

                    listsViewWindow.AddCustomer(customer);

                    MessageBox.Show("customer added seccussfully");
                    listsViewWindow.UpdateBaseStationList();
                    Close();
                }

            }
            catch (Exception ex) when (ex is BO.IdAlreadyExistsException or FormatException)
            {
                customerID.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// allows only number to go into the id textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        /// <summary>
        /// allows to type only float number into the lication.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloatNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text);
        }


        #endregion

        #region updating functions
        /// <summary>
        /// updates the name of teh base station.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickUpdateCustomerButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bl.UpdateCustomer(customer.Id, customer.Name, customer.Phone))
                {
                    MessageBox.Show("customer updated seccussfully");
                    listsViewWindow.UpdateBaseStationList(); 
                    Close();
                }

            }
            catch (Exception ex) when (ex is BO.IdAlreadyExistsException or FormatException)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        #endregion

    }
}
