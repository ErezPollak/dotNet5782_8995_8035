using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BL.Abstracts;
using BL.Exceptions;
using BL.Models;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow
    {
        private readonly IBl bl =  BlFactory.GetBl();
        private readonly ListsViewWindow listsViewWindow;
        private readonly Customer customer;

        /// <summary>
        /// ctor for add customer
        /// </summary>
        /// <param name="listsViewWindow"></param>
        public CustomerWindow(ListsViewWindow listsViewWindow)
        {
            InitializeComponent();

            this.listsViewWindow = listsViewWindow;
            customer = new Customer(Location: new Location(), FromCustomer: new List<ParcelByCustomer>(),ToCustomer: new List<ParcelByCustomer>());
            AddingStack.DataContext = customer;

            //shows the panel that controls the adding of an new customer.
            AddingStack.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// ctor for Update customer
        /// </summary>
        /// <param name="listsViewWindow"></param>
        /// <param name="customer"></param>
        public CustomerWindow(ListsViewWindow listsViewWindow, Customer customer)
        {
            InitializeComponent();

            this.listsViewWindow = listsViewWindow;
            this.customer = customer;
            OptionStack.DataContext = customer;

            //shows the panel that controls the updating of an existing customer.
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
        /// dragging the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion

        #region Adding functions
        /// <summary>
        /// add base station button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickAddCustomerButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (bl.AddCustomer(customer))
                {

                    listsViewWindow.AddCustomer(customer);

                    MessageBox.Show("customer added successfully");
                    listsViewWindow.UpdateBaseStationList();
                    Close();
                }

            }
            catch (Exception ex) when (ex is IdAlreadyExistsException or FormatException)
            {
                customerID.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// allows only number to go into the id textBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        /// <summary>
        /// allows to type only float number into the location.
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
                    MessageBox.Show("customer updated successfully");
                    listsViewWindow.UpdateBaseStationList(); 
                    Close();
                }

            }
            catch (Exception ex) when (ex is IdAlreadyExistsException or FormatException)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        #endregion

    }
}
