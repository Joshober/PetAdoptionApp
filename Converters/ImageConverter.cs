using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MAUI_Tutorial1_TodoList.Converters
{
    public class PhotoUrlConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] is PrimaryPhotoCroppedUrl, values[1] is PrimaryPhotoUrl
            var croppedUrl = values[0] as string;
            var primaryUrl = values[1] as string;
            return !string.IsNullOrWhiteSpace(croppedUrl) ? croppedUrl : primaryUrl;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
