using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Extensions
{
	[ContentProperty(nameof(Email))]
	public class GravatarImageExtension : GravatarImageSource, IMarkupExtension<GravatarImageSource>
	{
		public GravatarImageSource ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			if (BindingContext == null && !IsSet(BindingContextProperty))
			{
				var valueTargetProvider = serviceProvider.GetService<IProvideValueTarget>();
				SetBinding(BindingContextProperty, new Binding(nameof(BindingContext), source: valueTargetProvider.TargetObject));
			}

			return this;
		}

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) =>
			ProvideValue(serviceProvider);
	}
}