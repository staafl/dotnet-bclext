using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Tick42.HotButtons.Utils.Converters
{
    /// <summary>
    /// Converts a string to a Brush
    /// </summary>
    public class StringToBrushConverter : MarkupExtensionSelfProviderBase, IValueConverter
    {
        // ReSharper disable once EmptyConstructor
        public StringToBrushConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string asString = value + "";
            return new BrushConverter().ConvertFromString(asString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Can't convert back");
        }
    }
}