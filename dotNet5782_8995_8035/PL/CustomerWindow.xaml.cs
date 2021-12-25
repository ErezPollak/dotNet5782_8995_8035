using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private static readonly Random Random = new();


        private readonly IBL bl;
        private readonly ListsViewWindow listsViewWindow;
        private Customer customer;


        public CustomerWindow(BlApi.IBL bl, ListsViewWindow listsViewWindow)
        {
            InitializeComponent();

            this.bl = bl;
            this.listsViewWindow = listsViewWindow;
            customer = new(
                Location: new Location(Random.NextDouble(31, 35), Random.NextDouble(31, 35)),
                FromCustomer: new(),
                ToCustomer: new()
                );
            AddingStack.DataContext = customer;
            AddingStack.Visibility = Visibility.Visible;
        }

        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void OnClickAddCustomerButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (bl.AddCustumer(customer))
                {
                    MessageBox.Show("baseStation added seccussfully");
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        /// <summary>
        /// hiding the x button of the window
        /// </summary>
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Loded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

    }
}
