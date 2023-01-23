using System;
using System.Globalization;
using Quiz.Mobile.Shared.DTOs;
using Xamarin.Forms;

namespace Quiz.Mobile.Helpers.Converters
{
    public class AddressToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var address = value as AddressDto;
            return address != null ? address.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

