using System;
using System.Resources;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Extensions
{
	[ContentProperty(nameof(Text))]
	public class TranslateExtension : IMarkupExtension<BindingBase>
	{
		public string Text { get; set; } = string.Empty;

		public string? StringFormat { get; set; }

		public ResourceManager? ResourceManager { get; set; }

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

		public BindingBase ProvideValue(IServiceProvider serviceProvider)
		{
#if NETSTANDARD1_0
			throw new NotSupportedException("Translate XAML MarkupExtension is not supported on .NET Standard 1.0");
#else
			ResourceManager ??= LocalizationResourceManager.Current.DefaultResourceManager;

			if (ResourceManager == null)
			{
				throw new ArgumentNullException(nameof(ResourceManager), $"Call LocalizationResourceManager.Current.Init(defaultResourceManager) first or provide {nameof(ResourceManager)} argument. Text: {Text}");
			}

			var binding = new Binding
			{
				Mode = BindingMode.OneWay,
				Path = $"[{Text}]",
				Source = new ObservableResourceManager(ResourceManager),
				StringFormat = StringFormat
			};
			return binding;
#endif
		}

#if !NETSTANDARD1_0
		class ObservableResourceManager : ObservableObject
		{
			readonly ResourceManager resourceManager;

			public ObservableResourceManager(ResourceManager resourceManager)
			{
				this.resourceManager = resourceManager;
				LocalizationResourceManager.Current.PropertyChanged += (sender, e) => OnPropertyChanged(null);
			}

			[Preserve(Conditional = true)]
			public string? this[string name] =>
				resourceManager.GetString(name, LocalizationResourceManager.Current.CurrentCulture);
		}
#endif
	}
}