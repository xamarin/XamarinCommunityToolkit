using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizationResourceManager : ObservableObject
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
			currentCulture = initialCulture;
		}

		public string GetValue(string text) =>
			resourceManager.GetString(text, CurrentCulture);

		public string this[string text] =>
			GetValue(text);

		[Obsolete("Please, use " + nameof(CurrentCulture) + " to set culture")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetCulture(CultureInfo language) => CurrentCulture = language;

		public CultureInfo CurrentCulture
		{
			get => currentCulture;
			set => SetProperty(ref currentCulture, value, null);
		}

		[Obsolete("This method is no longer needed with new implementation of " + nameof(LocalizationResourceManager) + ". Please, remove all references to it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Invalidate() => OnPropertyChanged(null);
	}
#endif
}