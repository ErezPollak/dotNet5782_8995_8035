using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        private static readonly Random Random = new();


        private readonly IBL bl;
        private readonly ListsViewWindow listsViewWindow;
        private BaseStation baseStation;

        /// <summary>
        /// ctor for adding
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listsViewWindow"></param>
        public BaseStationWindow(BlApi.IBL bl, ListsViewWindow listsViewWindow)
        {
            InitializeComponent();

            this.bl = bl;
            this.listsViewWindow = listsViewWindow;
            baseStation = new(Location: new(), ChargingDrones: new());
            AddingStack.DataContext = baseStation;
            AddingStack.Visibility = Visibility.Visible;


        }

        /// <summary>
        /// Update base station
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="listsViewWindow"></param>
        /// <param name="baseStation"></param>
        public BaseStationWindow(BlApi.IBL bl, ListsViewWindow listsViewWindow, BaseStation baseStation)
        {
            InitializeComponent();

            this.bl = bl;
            this.listsViewWindow = listsViewWindow;
            this.baseStation = baseStation;
            OptionStack.DataContext = baseStation;
            OptionStack.Visibility = Visibility.Visible;
            listViewOfBaseStatin_ChargingDrones.DataContext = baseStation.ChargingDrones;
        }


        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void OnClickAddBaseStationButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (bl.AddBaseStation(baseStation))
                {
                    MessageBox.Show("baseStation added seccussfully");
                    // listsViewWindow.UpdateBaseStationList(); TODO: fix nullptr exception
                    Close();
                }

            }
            catch (Exception ex) when (ex is BO.IdAlreadyExistsException or FormatException)
            {
                BaseStationID.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickUpdateBaseStationButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (bl.UpdateBaseStation(baseStation.Id, baseStation.Name, baseStation.ChargeSlots))
                {
                    MessageBox.Show("baseStation updated seccussfully");
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

        /// <summary>
        /// hiding the x button of the window
        /// </summary>
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Loded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }


    }
}
