using System.Collections.Generic;
using Foundation;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() =>
			NSLocale.PreferredLanguages;

		private static partial CultureInfoParser GetCultureParser() => new iOSCultureInfoParser();

		class iOSCultureInfoParser : CultureInfoParser
		{
			protected override string ToDotnetFallbackLanguage(string languageCode)
			{
				languageCode = base.ToDotnetFallbackLanguage(languageCode);

				var netLanguage = languageCode switch
				{
					"pt" => "pt-PT", // Fallback to Portuguese (Portugal)
					_ => languageCode,
				};
				return netLanguage;
			}
		}
	}
}