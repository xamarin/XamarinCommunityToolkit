using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.Extensions.Internals
{
	public abstract class ValueConverterExtension : BindableObject, IMarkupExtension<IValueConverter>
	{
		public IValueConverter ProvideValue(IServiceProvider serviceProvider)
			=> (IValueConverter)this;

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
			=> ((IMarkupExtension<IValueConverter>)this).ProvideValue(serviceProvider);
	}
}