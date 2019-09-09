using System;
using System.Globalization;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Reports;
using Xamarin.Forms;

namespace FamilyMoney.Shared.NetStandard.Converters
{
    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IAccount account)
            {
                return $"{account.Name} ({account.Currency})";
            }

            if (value is Report1.CategoryAccountPair pair)
            {
                var category = pair.Category;
                var level = category.Level();
                return new String(' ', level * 4) + category?.Name;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
