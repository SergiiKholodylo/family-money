using System;
using Windows.UI.Xaml.Data;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Reports;

namespace FamilyMoney.UWP.Converters
{
    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IAccount)
            {
                var account = (IAccount) value;
                return $"{account.Name} ({account.Currency})";
            }

            if (value is Report1.CategoryAccountPair)
            {
                var category = ((Report1.CategoryAccountPair) value).Category;
                var level = category.Level();
                return new String(' ', level*4)+category?.Name;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
