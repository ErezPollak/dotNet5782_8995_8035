
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        public ParcelWindow(BO.Parcel parcel)
        {
            InitializeComponent();
            Mainstack.DataContext = parcel;
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


    }
}
