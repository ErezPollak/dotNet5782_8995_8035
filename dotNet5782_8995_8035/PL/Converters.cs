using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BL;
using BL.Models;

namespace PL
{

    #region General

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

    #endregion

    #region DroneConverters

    /// <summary>
    /// converter for the ChargeAndUnchargeButton TEXT in the battry pannel
    /// </summary>
    class StateToChargeState : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enums.DroneStatuses isInCharge = (Enums.DroneStatuses)value;
            if (isInCharge == Enums.DroneStatuses.MAINTENANCE)
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

    /// <summary>
    /// converter for the ChargeAndUnchargeButton VISABILITY in the battry pannel
    /// </summary>
    class DroneBattryToChargeVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Drone drone = (Drone)value;

            if (drone != null && (drone.Battery == 100 || drone.Status == Enums.DroneStatuses.DELIVERY))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// for the open parcel button in the parcel panel
    /// </summary>
    class DroneDeliveryToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enums.DroneStatuses isInDlivery = (Enums.DroneStatuses)value;
            if (isInDlivery == Enums.DroneStatuses.DELIVERY)
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

    /// <summary>
    /// for the auto button
    /// </summary>
    class DroneFreeToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enums.DroneStatuses isInDlivery = (Enums.DroneStatuses)value;
            if (isInDlivery == Enums.DroneStatuses.FREE)
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

    class DroneParcelStatusToProgressBarValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                PARCEL_STATE parcelState = (PARCEL_STATE)value;

                switch (parcelState)
                {
                    case PARCEL_STATE.ASSIGN:
                        return 33;
                    case PARCEL_STATE.PICKUP:
                        return 66;
                    case PARCEL_STATE.DELIVER:
                        return 100;
                    default:
                        return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// text of the delivery option button.
    /// </summary>
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


    #endregion

    #region ParcelConverter

    /// <summary>
    /// for the open parcel visability in the parcel panel.
    /// </summary>
    class DroneIdToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int)value;

            if (v != 0)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
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
                return "no parcel is assigned";
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

    class ParcelToProgressBarValvue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Parcel parcel = (Parcel)value;

            int progress = parcel.AssignedTime == null ? 25 :
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



    #endregion

    #region AccssesAtholerasationToVisability

    class AccssesAtholerasationToVisability : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ACCESS_AUTHORIZATION atholerazetion = (ACCESS_AUTHORIZATION)value;

            if (atholerazetion == ACCESS_AUTHORIZATION.GUEST)
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
            ACCESS_AUTHORIZATION accsses = (ACCESS_AUTHORIZATION)value;

            if ((int)accsses < (int)ACCESS_AUTHORIZATION.MANAGER)
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
            ACCESS_AUTHORIZATION accsses = (ACCESS_AUTHORIZATION)value;

            if ((int)accsses < (int)ACCESS_AUTHORIZATION.EMPLOYEE)
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
            ACCESS_AUTHORIZATION accsses = (ACCESS_AUTHORIZATION)value;

            if ((int)accsses < (int)ACCESS_AUTHORIZATION.CUSTOMER)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    #endregion

}
