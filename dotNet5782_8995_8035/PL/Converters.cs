using System;
using System.Globalization;
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
            if (isInCharge == BO.Enums.DroneStatuses.MAINTENANCE)
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
                    case PARCEL_STATE.PICKUP:
                        return "Pick Up A Parcel";
                    case PARCEL_STATE.DELIVER:
                        return "Dlivering Parcel";
                    default:
                        return "";
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

            if (drone != null && (drone.Battery == 100 || drone.Status == BO.Enums.DroneStatuses.DELIVERY))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class AccssesAtholerasationToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccssesAtholerazetion atholerazetion = (AccssesAtholerazetion)value;

            if (atholerazetion == AccssesAtholerazetion.GUEST)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ManagerAccssesAtholerasationToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccssesAtholerazetion accsses = (AccssesAtholerazetion)value;

            if ((int)accsses < (int)AccssesAtholerazetion.MANAGER)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class EmployeeAccssesAtholerasationToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccssesAtholerazetion accsses = (AccssesAtholerazetion)value;

            if ((int)accsses < (int)AccssesAtholerazetion.EMPLOYEE)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class CostumerAccssesAtholerasationToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccssesAtholerazetion accsses = (AccssesAtholerazetion)value;

            if ((int)accsses < (int)AccssesAtholerazetion.CUSTOMER)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    class ParcelToProgressBarValvue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Parcel parcel = (BO.Parcel)value;

            int progress = parcel.AssigedTime == null ? 25 :
                             (parcel.PickupTime == null ? 50 :
                             (parcel.DeliveringTime == null ? 75 :
                             100));
            return progress;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
