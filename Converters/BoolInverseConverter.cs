using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace AIChatBotAppRAG.Converters
{
    public class BoolInverseConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return !val;
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
