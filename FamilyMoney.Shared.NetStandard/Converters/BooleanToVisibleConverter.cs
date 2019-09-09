using System;
using System.Globalization;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace FamilyMoney.Shared.NetStandard.Converters
{
    public class BooleanToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var reverse = bool.Parse(parameter.ToString());
            if (boolValue)
            {
                return !reverse ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return !reverse ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    
}
