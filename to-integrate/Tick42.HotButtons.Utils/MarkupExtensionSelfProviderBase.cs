using System;
using System.Windows.Markup;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     Allows derived class objects to be instantiated as markup extensions in XAML.
    /// </summary>
    public abstract class MarkupExtensionSelfProviderBase : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}