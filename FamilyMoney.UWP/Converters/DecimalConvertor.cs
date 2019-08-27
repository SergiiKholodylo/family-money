using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace FamilyMoney.UWP.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var decimalValue = (decimal) value;
            if (decimalValue == 0) return string.Empty;
            return parameter == null ? decimalValue.ToString("F2") : string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Decimal.TryParse(ForceReplaceComaWithDot(value), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture.NumberFormat, out decimal result);
            return result;
        }

        private string ForceReplaceComaWithDot(object value)
        {
            return ((string)value).Replace(",",".");
        }
    }
}
