using System;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizedString : INotifyPropertyChanged, IDisposable
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

		public event PropertyChangedEventHandler PropertyChanged;

		void Invalidate(object sender, PropertyChangedEventArgs e)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

		public void Dispose() => localizationManager.PropertyChanged -= Invalidate;

		~LocalizedString() => Dispose();
	}
#endif
}
