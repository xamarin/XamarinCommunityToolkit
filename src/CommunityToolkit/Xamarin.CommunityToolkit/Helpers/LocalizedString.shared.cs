using System;
using System.ComponentModel;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizedString : ObservableObject
	{
		readonly Func<string> generator;
		readonly LocalizationResourceManager localizationManager;

		public LocalizedString(Func<string> generator = null)
            : this(LocalizationResourceManager.Current, generator)
        {
        }

		public LocalizedString(LocalizationResourceManager localizationManager, Func<string> generator = null)
		{
			this.localizationManager = localizationManager;
			this.generator = generator;
			localizationManager.PropertyChanged += Invalidate;
		}

		public string Localized => generator?.Invoke();

		[Preserve(Conditional = true)]
		public static implicit operator LocalizedString(Func<string> func) => new LocalizedString(func);

		void Invalidate(object sender, PropertyChangedEventArgs e) =>
			OnPropertyChanged(null);

		public void Dispose() => localizationManager.PropertyChanged -= Invalidate;

		~LocalizedString() => Dispose();
	}
#endif
}
