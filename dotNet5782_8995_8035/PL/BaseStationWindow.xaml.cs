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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow
    {
        private readonly IBl _bl = BlFactory.GetBl();
        private readonly ListsViewWindow _listsViewWindow;
        private readonly BaseStation _baseStation;

        /// <summary>
        /// ctor for adding
        /// </summary>
        /// <param name="listsViewWindow"></param>
        public BaseStationWindow(ListsViewWindow listsViewWindow)
        {
            InitializeComponent();

            _listsViewWindow = listsViewWindow;
            _baseStation = new BaseStation(Location: new Location(), ChargingDrones: new List<DroneInCharge>());
            AddingStack.DataContext = _baseStation;
            AddingStack.Visibility = Visibility.Visible;


        }

        /// <summary>
        /// Update base station
        /// </summary>
        /// <param name="listsViewWindow"></param>
        /// <param name="baseStation"></param>
        public BaseStationWindow(ListsViewWindow listsViewWindow, BaseStation baseStation)
        {
            InitializeComponent();

            _listsViewWindow = listsViewWindow;
            _baseStation = baseStation;
            OptionStack.DataContext = baseStation;
            OptionStack.Visibility = Visibility.Visible;
            listViewOfBaseStatin_ChargingDrones.DataContext = baseStation.ChargingDrones;
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
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// add base station button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddBaseStationClick(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!_bl.AddBaseStation(_baseStation)) return;
                _listsViewWindow.AddBaseStation(_baseStation);

                MessageBox.Show("baseStation added successfully");
                _listsViewWindow.UpdateBaseStationList();
                Close();

            }
            catch (Exception ex) when (ex is IdAlreadyExistsException or FormatException)
            {
                BaseStationID.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// updates the name of teh base station.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickUpdateBaseStationButton(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!_bl.UpdateBaseStation(_baseStation.Id, _baseStation.Name, _baseStation.ChargeSlots)) return;
                MessageBox.Show("baseStation updated successfully");
                _listsViewWindow.UpdateLists();// TODO: fix nullptr exception
                Close();

            }
            catch (Exception ex) when (ex is IdAlreadyExistsException or FormatException)
            {
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
        

    }
}
