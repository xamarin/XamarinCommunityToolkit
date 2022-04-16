using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;
#if NETSTANDARD1_0 || WINDOWS
using System.Reflection;
#endif

namespace Xamarin.CommunityToolkit.Extensions
{
	/// <summary>
	/// Provides ImageSource by Resource Id from the current app's assembly.
	/// </summary>
	[ContentProperty(nameof(Id))]
	public class ImageResourceExtension : IMarkupExtension<ImageSource?>
	{
		/// <summary>
		/// The Resource Id of the image.
		/// </summary>
		public string? Id { get; set; }

		public ImageSource? ProvideValue(IServiceProvider serviceProvider)
			=> Id == null
				? null
				: ImageSource.FromResource(Id, Application.Current.GetType()
#if NETSTANDARD1_0 || WINDOWS
					.GetTypeInfo()
#endif
					.Assembly);

		object? IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
			=> ((IMarkupExtension<ImageSource?>)this).ProvideValue(serviceProvider);
	}
}