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
		static readonly Lazy<LocalizationResourceManager> currentHolder = new Lazy<LocalizationResourceManager>(() => new LocalizationResourceManager());

		public static LocalizationResourceManager Current => currentHolder.Value;

		LocalizationResourceManager()
		{
		}

		public void Init(ResourceManager defaultResourceManager) =>
			DefaultResourceManager = defaultResourceManager;

		public void Init(CultureInfo initialCulture) =>
			CurrentCulture = initialCulture;

		public void Init(ResourceManager defaultResourceManager, CultureInfo initialCulture)
		{
			DefaultResourceManager = defaultResourceManager;
			CurrentCulture = initialCulture;
		}

		[Obsolete("Please, use " + nameof(CurrentCulture) + " to set culture")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetCulture(CultureInfo language) => CurrentCulture = language;

		public CultureInfo CurrentCulture
		{
			get => CultureInfo.DefaultThreadCurrentUICulture ?? CultureInfo.DefaultThreadCurrentCulture;
			set
			{
				if (CurrentCulture == value)
				{
					return;
				}

				CultureInfo.DefaultThreadCurrentUICulture = value;
				CultureInfo.DefaultThreadCurrentCulture = value;
				OnPropertyChanged(nameof(CurrentCulture));
			}
		}

		public ResourceManager DefaultResourceManager { get; private set; }

		public void Invalidate() => OnPropertyChanged(nameof(CurrentCulture));
	}
#endif
}