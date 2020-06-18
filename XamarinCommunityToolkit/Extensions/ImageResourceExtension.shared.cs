using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#if NETSTANDARD
using System.Reflection;
#endif

namespace XamarinCommunityToolkit.Extensions
{
    /// <summary>
    /// Provides ImageSource by Resource Id from the current app's assembly.
    /// </summary>
    [ContentProperty(nameof(Id))]
    public class ImageResourceExtension : IMarkupExtension<ImageSource>
    {
        /// <summary>
        /// The Resource Id of the image.
        /// </summary>
        public string Id { get; set; }

        public ImageSource ProvideValue(IServiceProvider serviceProvider)
            => Id == null
                ? null
                : ImageSource.FromResource(Id, Application.Current.GetType()
#if NETSTANDARD
                    .GetTypeInfo()
#endif
                    .Assembly);

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
            => ((IMarkupExtension<ImageSource>)this).ProvideValue(serviceProvider);
    }
}
