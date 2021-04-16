using System;
using System.Globalization;
using Windows.System.UserProfile;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		static string PlatformInstalledUICulture => GlobalizationPreferences.Languages[0];

		static CultureInfo PlatformGetCurrentUICulture(Func<string, CultureInfo> mappingOverride) =>
			CultureInfo.CurrentUICulture;
	}
}