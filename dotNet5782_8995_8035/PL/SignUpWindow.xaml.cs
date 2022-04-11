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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow
    {
        private readonly IBl bl = BlFactory.GetBl();
        private readonly Customer customer;

        public SignUpWindow()
        {
            InitializeComponent();

            customer = new Customer(Location: new Location(), FromCustomer: new List<ParcelByCustomer>(), ToCustomer: new List<ParcelByCustomer>());

            SignUpStack.DataContext = customer;

        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bl.AddCustomer(customer))
                {
                    Close();
                    MessageBox.Show("You have Finished!!!! \n you were been added successfully \n Welcome :)");
                }

            }
            catch (Exception ex) when (ex is IdAlreadyExistsException or FormatException)
            {
                customerID.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
            }
        }

        private void XButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
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
