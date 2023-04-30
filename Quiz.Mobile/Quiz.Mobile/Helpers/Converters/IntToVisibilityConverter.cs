using System;
using System.Globalization;
using Xamarin.Forms;

namespace Quiz.Mobile.Helpers.Converters
{
	public class IntToVisibilityConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return false;
            if (!int.TryParse(value.ToString(), out int number))
                return false;
            return number == default ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

