using System;
using Windows.UI.Xaml.Data;

namespace FamilyMoney.UWP.Converters
{
    public class DateTimeToTimeSpanConverter : IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                try
                {
                    if (value != null)
                    {
                        DateTime date = (DateTime)value;
                        return new TimeSpan(date.Hour,date.Millisecond,date.Second);
                    }
                    return new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                catch (Exception)
                {
                    return DateTimeOffset.MinValue;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                try
                {
                    DateTimeOffset dto = (DateTimeOffset)value;
                    return dto.DateTime;
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
    }
}
