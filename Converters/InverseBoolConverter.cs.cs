﻿using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MAUI_Tutorial1_TodoList.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            return value;
        }
    }
}
