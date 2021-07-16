using System.Collections.Generic;
using Windows.System.UserProfile;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() =>
			GlobalizationPreferences.Languages;
	}
}