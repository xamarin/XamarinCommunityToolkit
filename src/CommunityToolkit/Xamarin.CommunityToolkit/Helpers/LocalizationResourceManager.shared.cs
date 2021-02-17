using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizationResourceManager : INotifyPropertyChanged
	{
		static readonly Lazy<LocalizationResourceManager> currentHolder = new Lazy<LocalizationResourceManager>(() => new LocalizationResourceManager());

		public static LocalizationResourceManager Current => currentHolder.Value;

		ResourceManager resourceManager;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;

		LocalizationResourceManager()
		{
		}

		public void Init(ResourceManager resource) =>
			resourceManager = resource;

		public void Init(ResourceManager resource, CultureInfo initialCulture)
		{
			resourceManager = resource;
			SetCulture(initialCulture);
		}

		public string GetValue(string text) =>
			resourceManager.GetString(text, CurrentCulture);

		public string this[string text] =>
			GetValue(text);

		public void SetCulture(CultureInfo language)
		{
			currentCulture = language;
			Invalidate();
		}

		public CultureInfo CurrentCulture => currentCulture;

		public event PropertyChangedEventHandler PropertyChanged;

		public void Invalidate() =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
	}
#endif
}