using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for ContactUsWindow.xaml
    /// </summary>
    public partial class ContactUsWindow
    {
        /// <summary>
        /// ctor, opens the window.
        /// </summary>
        public ContactUsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// sending the message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Your Message was Received successfully In The System \n Thank you for your cooperation");

            System.Threading.Thread.Sleep(3000);

            Close();
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
    }
}
