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
#pragma warning disable CS0618 // Type or member is obsolete
		public static LocalizationResourceManager Current { get; } = new LocalizationResourceManager();
#pragma warning restore CS0618 // Type or member is obsolete

		ResourceManager resourceManager;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;

		[Obsolete("Please use the Current property instead of creating a new instance of this class")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public LocalizationResourceManager()
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