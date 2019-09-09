using System;
using System.Globalization;
using Xamarin.Forms;

namespace FamilyMoney.Shared.NetStandard.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var decimalValue = (decimal) value;
            if (decimalValue == 0) return string.Empty;
            return parameter == null ? decimalValue.ToString("F2") : string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
