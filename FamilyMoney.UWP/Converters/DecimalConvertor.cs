using System;
using Windows.UI.Xaml.Data;

namespace FamilyMoney.UWP.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var decimalValue = (decimal) value;
            return parameter == null ? decimalValue.ToString("F2") : string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Decimal.TryParse((string)value, out decimal result);
            return result;
        }
    }
}
