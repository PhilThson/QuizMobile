using System;
using System.Globalization;
using Xamarin.Forms;

namespace Quiz.Mobile.Helpers.Converters
{
    public class TextChangedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var eventArgs = value as TextChangedEventArgs;
            if (eventArgs == null)
                return string.Empty;
            return eventArgs.NewTextValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

