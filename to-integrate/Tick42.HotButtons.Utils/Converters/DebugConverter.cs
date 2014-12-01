using System;
using System.Globalization;
using System.Windows.Data;

namespace Tick42.HotButtons.Utils.Converters
{
    /// <summary>
    /// Used to break into binding logic.
    /// </summary>
    public class DebugConverter : MarkupExtensionSelfProviderBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}