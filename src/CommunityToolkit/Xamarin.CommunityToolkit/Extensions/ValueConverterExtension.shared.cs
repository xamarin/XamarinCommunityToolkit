using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Extensions.Internals
{
	public abstract class ValueConverterExtension : IMarkupExtension<IValueConverter>
	{
		public IValueConverter ProvideValue(IServiceProvider serviceProvider)
			=> (IValueConverter)this;

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
			=> ((IMarkupExtension<IValueConverter>)this).ProvideValue(serviceProvider);
	}
}