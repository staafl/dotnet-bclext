using System.Windows.Media;

namespace Tick42.HotButtons.Utils.Converters
{
    /// <summary>
    ///     Creates a modified SolidColorBrush by grayscaling it, i.e. decreasing its saturation.
    /// </summary>
    public class SolidColorBrushGrayscaleConverter : SolidColorBrushModifierConverterBase
    {
        // ReSharper disable once EmptyConstructor
        public SolidColorBrushGrayscaleConverter()
        {
        }

        protected override Color ModifyColor(Color color, object parameter)
        {
            double scale;
            if (!double.TryParse(parameter + "", out scale))
            {
                scale = parameter as double? ?? 0;
            }
            return color.Grayscale((float) scale);
        }
    }
}