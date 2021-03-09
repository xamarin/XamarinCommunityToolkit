using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Helpers
{
#if !NETSTANDARD1_0
	public class LocalizationResourceManager : ObservableObject
	{
		static readonly Lazy<LocalizationResourceManager> currentHolder = new (() => new LocalizationResourceManager());

		public static LocalizationResourceManager Current => currentHolder.Value;

		CultureInfo? currentCulture = CultureInfo.DefaultThreadCurrentUICulture ?? CultureInfo.DefaultThreadCurrentCulture;

		LocalizationResourceManager()
		{
		}

		public void Init(ResourceManager defaultResourceManager) => DefaultResourceManager = defaultResourceManager;

		public void Init(CultureInfo initialCulture) => CurrentCulture = initialCulture;

		public void Init(ResourceManager defaultResourceManager, CultureInfo initialCulture)
		{
			Init(defaultResourceManager);
			Init(initialCulture);
		}

		[Obsolete("Please, use " + nameof(CurrentCulture) + " to set culture")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetCulture(CultureInfo language) => CurrentCulture = language;

		public CultureInfo? CurrentCulture
		{
			get => currentCulture;
			set => SetProperty(ref currentCulture, value);
		}

		internal ResourceManager? DefaultResourceManager { get; private set; }

		[Obsolete("This method is no longer needed with new implementation of " + nameof(LocalizationResourceManager) + ". Please, remove all references to it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Invalidate() => OnPropertyChanged(nameof(CurrentCulture));
	}
#endif
}