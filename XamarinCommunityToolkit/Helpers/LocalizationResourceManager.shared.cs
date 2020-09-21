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

		public void Init(ResourceManager resource)
		{
			resourceManager = resource;
		}

		public string this[string text] => resourceManager.GetString(text, CurrentCulture);

		public void SetCulture(CultureInfo language)
		{
			Thread.CurrentThread.CurrentUICulture = language;
			Invalidate();
		}

		public string GetValue(string text) => resourceManager.GetString(text, CultureInfo.CurrentCulture);

		public CultureInfo CurrentCulture => Thread.CurrentThread.CurrentUICulture;

		public event PropertyChangedEventHandler PropertyChanged;

		public void Invalidate()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
		}
	}
#endif
}