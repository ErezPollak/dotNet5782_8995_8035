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


    class ParcelStausToDeliveringOptionText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                PARCEL_STATE parcelState = (PARCEL_STATE)value;
                switch (parcelState)
                {
                    case PARCEL_STATE.ASSIGN:
                        return "Assign Parcel To Drone";
                        break;
                    case PARCEL_STATE.PICKUP:
                        return "Pick Up A Parcel";
                        break;
                    case PARCEL_STATE.DELIVER:
                        return "Dlivering Parcel";
                        break;
                    default:
                        return "";
                        break;
                }
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    class DroneBattryToChargeVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Drone drone = (BO.Drone)value;

            if (drone != null &&( drone.Battery == 100 || drone.Status == BO.Enums.DroneStatuses.DELIVERY))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
