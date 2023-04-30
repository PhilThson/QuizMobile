using System;
using System.Globalization;
using Xamarin.Forms;

namespace Quiz.Mobile.Helpers.Converters
{
	public class ItemTypeToVisibilityConverter : IValueConverter
	{

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return false;
            //Ponieważ obszary mają dodatkowo właściwość ExtendedDescription 
            return value.ToString() == QuizApiSettings.Areas ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

