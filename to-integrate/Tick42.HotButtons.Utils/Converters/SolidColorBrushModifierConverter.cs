using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Tick42.HotButtons.Utils.Converters
{
    /// <summary>
    /// Base class for converters that manipulate colors or SolidColorBrushes
    /// </summary>
    public abstract class SolidColorBrushModifierConverterBase : MarkupExtensionSelfProviderBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var asSolidColorBrush = value as SolidColorBrush;
            Color color;
            if (asSolidColorBrush == null)
            {
                var asColor = (value as Color? ?? ColorConverter.ConvertFromString(value + "")) as Color?;
                if (asColor == null)
                {
                    return null;
                }
                color = asColor.Value;
            }
            else
            {
                color = asSolidColorBrush.Color;
            }
            Color newColor = ModifyColor(color, parameter);
            return new SolidColorBrush(newColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Can't convert back");
        }

        protected abstract Color ModifyColor(Color color, object parameter);
    }
}