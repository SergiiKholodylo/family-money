using System;
using System.Globalization;
using Xamarin.Forms;

namespace FamilyMoney.Shared.NetStandard.Converters
{
    public class DateTimeToTimeSpanConverter : IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                try
                {
                    if (value == null) return new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    var date = (DateTime)value;
                    return new TimeSpan(date.Hour,date.Millisecond,date.Second);
                }
                catch (Exception)
                {
                    return DateTimeOffset.MinValue;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                try
                {
                    var dto = (DateTimeOffset)value;
                    return dto.DateTime;
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
    }
}
