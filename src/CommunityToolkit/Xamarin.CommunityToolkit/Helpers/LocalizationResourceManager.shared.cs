using System;
using System.Globalization;
using System.Resources;
using System.Threading;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizationResourceManager : ObservableObject
	{
		public static LocalizationResourceManager Current { get; } = new LocalizationResourceManager();

		ResourceManager resourceManager;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;

		public void Init(ResourceManager resource) =>
			resourceManager = resource;

		public void Init(ResourceManager resource, CultureInfo initialCulture)
		{
			resourceManager = resource;
			currentCulture = initialCulture;
		}

		public string GetValue(string text) =>
			resourceManager.GetString(text, CurrentCulture);

		public string this[string text] =>
			GetValue(text);

		[Obsolete("Use " + nameof(CurrentCulture) + " to set culture")]
		public void SetCulture(CultureInfo language) => CurrentCulture = language;

		public CultureInfo CurrentCulture
		{
			get => currentCulture;
			set => SetProperty(ref currentCulture, value, null);
		}
	}
#endif
}