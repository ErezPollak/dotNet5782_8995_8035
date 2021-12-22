using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL
{
    class DoubleToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double val = (Double)value;
            return (int)val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class StateToChargeState : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Enums.DroneStatuses isInCharge = (BO.Enums.DroneStatuses)value;
            if(isInCharge == BO.Enums.DroneStatuses.MAINTENANCE)
            {
                return "Uncharge";
            }
            else
            {
                return "Charge";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ParcelNumberToParcelState : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int parcelNumber = (int)value;
            if (parcelNumber == 0)
            {
                return "no parcel in assigned";
            }
            else
            {
                return parcelNumber + "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class StatusToParcelVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Enums.DroneStatuses isInDlivery = (BO.Enums.DroneStatuses)value;
            if (isInDlivery == BO.Enums.DroneStatuses.DELIVERY)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
