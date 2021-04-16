using System;
using System.Globalization;
using Foundation;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		static string PlatformInstalledUICulture => NSLocale.PreferredLanguages[0] ?? FallbackCulture;

		static CultureInfo PlatformGetCurrentUICulture(Func<string, CultureInfo> mappingOverride)
		{
			var netLanguage = ToDotnetLanguage(InstalledUICulture);

			// this gets called a lot - try/catch can be expensive so consider caching or something
			CultureInfo ci;
			try
			{
				ci = new CultureInfo(netLanguage);
			}
			catch (CultureNotFoundException)
			{
				if (mappingOverride != null)
				{
					return mappingOverride(InstalledUICulture);
				}

				// iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
				// fallback to first characters, in this case "en"
				try
				{
					var fallback = ToDotnetFallbackLanguage(new InternalCulture(netLanguage));
					ci = new CultureInfo(fallback);
				}
				catch (CultureNotFoundException)
				{
					// iOS language not valid .NET culture, falling back to English
					ci = new CultureInfo("en");
				}
			}
			return ci;
		}

		static string ToDotnetLanguage(string iOSLanguage)
		{
			// Certain languages need to be converted to CultureInfo equivalent

			var netLanguage = iOSLanguage switch
			{
				"ms-MY" or "ms-SG" => "ms", // "Malaysian (Malaysia)" not supported .NET culture
				"gsw-CH" => "de-CH", // "Schwiizertüütsch (Swiss German)" not supported .NET culture
				_ => iOSLanguage,
			};
			return netLanguage;
		}

		static string ToDotnetFallbackLanguage(InternalCulture platCulture)
		{
			var netLanguage = platCulture.LanguageCode switch
			{
				"pt" => "pt-PT", // Fallback to Portuguese (Portugal)
				"gsw" => "de-CH", // Equivalent to German (Switzerland) for this app
				_ => platCulture.LanguageCode,
			};
			return netLanguage;
		}
	}
}