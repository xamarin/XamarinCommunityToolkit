using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Extensions
{
	public abstract class BindableObjectExtension : BindableObject, IMarkupExtension<BindableObject>
	{
		public BindableObject ProvideValue(IServiceProvider serviceProvider)
			=> this;

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
			=> ((IMarkupExtension<BindableObject>)this).ProvideValue(serviceProvider);
	}
}