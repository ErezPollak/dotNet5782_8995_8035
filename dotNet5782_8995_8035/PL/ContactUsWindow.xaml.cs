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
            Close();
            MessageBox.Show("Your Message was Resived succssesfully In The System \n Thank you for your cooperation");
        }
    }
}
