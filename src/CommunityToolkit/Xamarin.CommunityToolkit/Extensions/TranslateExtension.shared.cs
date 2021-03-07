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
		public string Text { get; set; }

		public string StringFormat { get; set; }

		public ResourceManager ResourceManager { get; set; } = LocalizationResourceManager.Current.DefaultResourceManager;

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

		public BindingBase ProvideValue(IServiceProvider serviceProvider)
		{
#if !NETSTANDARD1_0
			var binding = new Binding
			{
				Mode = BindingMode.OneWay,
				Path = $"[{Text}]",
				Source = new ObservableResourceManager(ResourceManager),
				StringFormat = StringFormat
			};
			return binding;
#else
			throw new NotSupportedException("Translate XAML MarkupExtension is not supported on .NET Standard 1.0");
#endif
		}

		public class ObservableResourceManager : ObservableObject
		{
			readonly ResourceManager resourceManager;

			ObservableResourceManager()
			{
			}

			public ObservableResourceManager(ResourceManager resourceManager)
			{
				this.resourceManager = resourceManager;
				LocalizationResourceManager.Current.PropertyChanged += (sender, e) => OnPropertyChanged(null);
			}

			[Preserve(Conditional = true)]
			public string this[string name] =>
				resourceManager.GetString(name, LocalizationResourceManager.Current.CurrentCulture);
		}
	}
}