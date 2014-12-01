using System.Windows.Media;

namespace Tick42.HotButtons.Utils.Converters
{
    /// <summary>
    ///     Creates a modified SolidColorBrush by scaling its color components (i.e. modifying its brightness).
    /// </summary>
    public class SolidColorBrushChangeBrightnessConverter : SolidColorBrushModifierConverterBase
    {
        // ReSharper disable once EmptyConstructor
        public SolidColorBrushChangeBrightnessConverter()
        {
        }

        protected override Color ModifyColor(Color color, object parameter)
        {
            double scale;
            if (!double.TryParse(parameter + "", out scale))
            {
                scale = parameter as double? ?? 0;
            }
            return color.ChangeBrightness((float) scale);
        }
    }
}