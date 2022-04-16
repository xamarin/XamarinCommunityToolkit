using System;using Microsoft.Extensions.Logging;using Microsoft.Extensions.DependencyInjection;
using Xamarin.CommunityToolkit.UI.Views;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.Extensions
{
	[ContentProperty(nameof(Email))]
	public class GravatarImageExtension : GravatarImageSource, IMarkupExtension<GravatarImageSource>
	{
		public GravatarImageSource ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			if (BindingContext == null && !IsSet(BindingContextProperty) && serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget valueTargetProvider)
			{
				SetBinding(BindingContextProperty, new Binding(nameof(BindingContext), source: valueTargetProvider.TargetObject));
			}

			return this;
		}

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) =>
			ProvideValue(serviceProvider);
	}
}