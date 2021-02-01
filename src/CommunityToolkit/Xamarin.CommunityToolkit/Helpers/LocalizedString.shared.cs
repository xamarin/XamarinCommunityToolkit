using System;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.ObjectModel.Extensions;
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
			localizationManager.WeakSubscribe(this, (t, sender, e) => t.OnPropertyChanged(null));
		}

		public string Localized => generator?.Invoke();

		[Preserve(Conditional = true)]
		public static implicit operator LocalizedString(Func<string> func) => new LocalizedString(func);
	}
#endif
}
