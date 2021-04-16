using System.Collections.Generic;
using System.Globalization;
using Windows.System.UserProfile;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() =>
			GlobalizationPreferences.Languages;

		private static partial CultureInfoParser GetCultureParser() => new UwpCultureInfoParser();

		class UwpCultureInfoParser : CultureInfoParser
		{
			public override CultureInfo Parse(string platformCulture) =>
				CultureInfo.GetCultureInfo(platformCulture);
		}
	}
}