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
            AddingStack.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// ctor for Update customer
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listsViewWindow"></param>
        /// <param name="customer"></param>
        public CustomerWindow(BlApi.IBL bl, ListsViewWindow listsViewWindow, Customer customer)
        {
            InitializeComponent();

            this.bl = bl;
            this.listsViewWindow = listsViewWindow;
            this.customer = customer;
            OptionStack.DataContext = customer;
            OptionStack.Visibility = Visibility.Visible;
            listViewOfBaseStatin_FromCustomer.DataContext = customer.FromCustomer;
            listViewOfBaseStatin_ToCustomer.DataContext = customer.ToCustomer;

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


        private void OnClickAddCustomerButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (bl.AddCustumer(customer))
                {

                    listsViewWindow.AddCustomer(customer);

                    MessageBox.Show("customer added seccussfully");
                    // listsViewWindow.UpdateBaseStationList(); TODO: fix nullptr exception
                    Close();
                }

            }
            catch (Exception ex) when (ex is BO.IdAlreadyExistsException or FormatException)
            {
                customerID.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickUpdateCustomerButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (bl.UpdateCustomer(customer.Id, customer.Name, customer.Phone))
                {
                    MessageBox.Show("customer updated seccussfully");
                    // listsViewWindow.UpdateBaseStationList(); TODO: fix nullptr exception
                    Close();
                }

            }
            catch (Exception ex) when (ex is BO.IdAlreadyExistsException or FormatException)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void FloatNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text);
        }

    }
}
