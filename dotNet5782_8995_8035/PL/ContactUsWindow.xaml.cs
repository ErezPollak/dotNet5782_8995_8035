using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ContactUsWindow.xaml
    /// </summary>
    public partial class ContactUsWindow : Window
    {
        public ContactUsWindow()
        {
            InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Comment.Text = "Your Message was Resived succssesfully In The System \n Thank you for your cooperation";
            Comment.IsReadOnly = true;

            System.Threading.Thread.Sleep(3000);

            Close();
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
