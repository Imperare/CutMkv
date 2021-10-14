using System;
using System.Globalization;
using System.Windows;

namespace CutMkv.Converters
{
    public class VisibilityCollapsedIfNumberIsLessThanTwo : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null && value is int && (int)value > 1) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
