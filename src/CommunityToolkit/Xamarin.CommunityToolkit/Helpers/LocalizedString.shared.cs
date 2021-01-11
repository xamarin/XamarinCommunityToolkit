using System;
using System.ComponentModel;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizedString : INotifyPropertyChanged
	{
		public LocalizedString(Func<string> generator = null)
		{
			Generator = generator;
			LocalizationResourceManager.Current.PropertyChanged += (_, _) => Invalidate();
		}

		Func<string> generator;

		public Func<string> Generator
		{
			get => generator;
			set
			{
				generator = value;
				Invalidate();
			}
		}

		public string Localized => Generator?.Invoke();

		public static implicit operator LocalizedString(Func<string> func) => new LocalizedString(func);

		public event PropertyChangedEventHandler PropertyChanged;

		public void Invalidate()
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
	}
#endif
}
