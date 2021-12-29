using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private readonly IBL bl = BlFactory.GetBl();
        private Customer customer;

        public SignUpWindow()
        {
            InitializeComponent();

            this.customer = new(Location: new Location(), FromCustomer: new List<BO.ParcelByCustomer>(), ToCustomer: new List<BO.ParcelByCustomer>());

            SignUpStack.DataContext = customer;

        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bl.AddCustumer(customer))
                {
                    Close();
                    MessageBox.Show("You have Finnished!!!! \n you were been added seccussfully \n Welcome :)");
                }

            }
            catch (Exception ex) when (ex is BO.IdAlreadyExistsException or FormatException)
            {
                customerID.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
            }
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
