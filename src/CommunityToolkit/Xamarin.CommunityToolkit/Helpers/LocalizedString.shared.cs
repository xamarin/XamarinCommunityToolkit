using System;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizedString : ObservableObject
	{
		readonly Func<string> generator;

		public LocalizedString(Func<string> generator = null)
			: this(LocalizationResourceManager.Current, generator)
		{
		}

		public LocalizedString(LocalizationResourceManager localizationManager, Func<string> generator = null)
		{
			this.generator = generator;

			// This instance will be unsibscribed and GCed if none references it
			// since LocalizationResourceManager uses WeekEventManger
			localizationManager.PropertyChanged += (sender, e) => OnPropertyChanged(null);
		}

		public string Localized => generator?.Invoke();

		[Preserve(Conditional = true)]
		public static implicit operator LocalizedString(Func<string> func) => new LocalizedString(func);
	}
#endif
}
