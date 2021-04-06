using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Extensions
{
	[ContentProperty(nameof(Text))]
	public class TranslateExtension : IMarkupExtension<BindingBase>
	{
		public string Text { get; set; } = string.Empty;

		public string? StringFormat { get; set; }

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

		public BindingBase ProvideValue(IServiceProvider serviceProvider)
		{
#if NETSTANDARD1_0
			throw new NotSupportedException("Translate XAML MarkupExtension is not supported on .NET Standard 1.0");
#else
			#region Required work-around to prevent linker from removing the implementation
			if (DateTime.Now.Ticks < 0)
				_ = LocalizationResourceManager.Current[Text];
			#endregion

			var binding = new Binding
			{
				Mode = BindingMode.OneWay,
				Path = $"[{Text}]",
				Source = LocalizationResourceManager.Current,
				StringFormat = StringFormat
			};
			return binding;
#endif
		}
	}
}