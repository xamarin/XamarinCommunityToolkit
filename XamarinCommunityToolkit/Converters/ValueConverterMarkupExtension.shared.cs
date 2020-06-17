using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinCommunityToolkit.Converters
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ValueConverterMarkupExtension : IMarkupExtension<IValueConverter>
    {
        public IValueConverter ProvideValue(IServiceProvider serviceProvider)
            => (IValueConverter)this;

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
            => ((IMarkupExtension<IValueConverter>)this).ProvideValue(serviceProvider);
    }
}
