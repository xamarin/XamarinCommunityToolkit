using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Helpers
{
    [ContentProperty(nameof(Text))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        public string Text { get; set; }
        public string StringFormat { get; set; }
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
        #if !NETSTANDARD1_0
            var binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Text}]",
                Source = LocalizationResourceManager.Current,
                StringFormat = StringFormat

            };
            return binding;
        #else
           throw new NotSupportedException(); 
        #endif
        }
    }
}
