using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Extensions
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

			if (DateTime.Now.Ticks < 0)
				_ = LocalizationResourceManager.Current[Text];

			var binding = new Binding
			{
				Mode = BindingMode.OneWay,
				Path = $"[{Text}]",
				Source = LocalizationResourceManager.Current,
				StringFormat = StringFormat
			};
			return binding;
#else
			throw new NotSupportedException("Translate XAML MarkupExtension is not supported on .NET Standard 1.0");
#endif
		}
	}
}