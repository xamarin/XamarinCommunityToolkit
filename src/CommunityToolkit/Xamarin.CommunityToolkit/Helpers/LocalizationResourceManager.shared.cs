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

		CultureInfo currentCulture =
			CultureInfo.DefaultThreadCurrentUICulture ??
			CultureInfo.DefaultThreadCurrentCulture ??
			Thread.CurrentThread.CurrentUICulture;

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

		public CultureInfo CurrentCulture
		{
			get => currentCulture;
			set => SetProperty(ref currentCulture, value);
		}

		internal ResourceManager? DefaultResourceManager { get; private set; }
	}
#endif
}
