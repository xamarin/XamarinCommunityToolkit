using System;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizedString : ObservableObject
	{
		readonly Func<string> generator;

		public LocalizedString(Func<string> generator)
			: this(LocalizationResourceManager.Current, generator)
		{
		}

		public LocalizedString(LocalizationResourceManager localizationManager, Func<string> generator)
		{
			this.generator = generator;

			// This instance will be unsubscribed and GCed if no one references it
			// since LocalizationResourceManager uses WeekEventManger
			localizationManager.PropertyChanged += (sender, e) => OnPropertyChanged(null);
		}

		[Preserve(Conditional = true)]
		public string Localized => generator();

		[Preserve(Conditional = true)]
		public static implicit operator LocalizedString(Func<string> func) => new LocalizedString(func);
	}
#endif
}