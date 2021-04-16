using System.Collections.Generic;
using Java.Util;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() =>
			new[] { Locale.Default.ToString() ?? FallbackCulture };

		private static partial CultureInfoParser GetCultureParser() => new AndroidCultureInfoParser();

		class AndroidCultureInfoParser : CultureInfoParser
		{
			protected override string ToDotnetLanguage(string androidLanguage)
			{
				androidLanguage = base.ToDotnetLanguage(androidLanguage);

				var netLanguage = androidLanguage switch
				{
					"in-ID" => "id-ID", // "Indonesian (Indonesia)" has different code in  .NET
					"iw-IL" => "he-IL", // Hebrew
					_ => androidLanguage,
				};
				return netLanguage;
			}

			protected override string ToDotnetFallbackLanguage(string languageCode)
			{
				languageCode = base.ToDotnetFallbackLanguage(languageCode);

				var netLanguage = languageCode switch
				{
					"iw" => "he", // Hebrew
					_ => languageCode,
				};
				return netLanguage;
			}
		}
	}
}