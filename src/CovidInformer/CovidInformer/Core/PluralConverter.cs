using System;
using System.Globalization;
using CovidInformer.Services;
using Xamarin.Forms;

namespace CovidInformer.Core
{
    internal sealed class PluralConverter : IValueConverter
    {
        public PluralConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pluralService = PluralService.Current;
            return pluralService.GetQuantityString((string)parameter, (ulong)value, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}