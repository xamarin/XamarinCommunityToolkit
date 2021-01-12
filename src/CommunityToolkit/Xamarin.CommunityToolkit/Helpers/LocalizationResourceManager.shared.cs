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
		public static LocalizationResourceManager Current { get; } = new LocalizationResourceManager();

		ResourceManager resourceManager;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;

		public void Init(ResourceManager resource) =>
			resourceManager = resource;

		public void Init(ResourceManager resource, CultureInfo initialCulture)
		{
			resourceManager = resource;
			CurrentCulture = initialCulture;
		}

		public string GetValue(string text) =>
			resourceManager.GetString(text, CurrentCulture);

		public string this[string text] =>
			GetValue(text);

		public void SetCulture(CultureInfo language) => CurrentCulture = language;

		public CultureInfo CurrentCulture
		{
			get => currentCulture;
			private set
			{
				currentCulture = value;
				Invalidate();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void Invalidate() =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
	}
#endif
}