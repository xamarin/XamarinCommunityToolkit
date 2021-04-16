using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Java.Util;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		static string PlatformInstalledUICulture => Locale.Default.ToString() ?? FallbackCulture;

		static CultureInfo PlatformGetCurrentUICulture(Func<string, CultureInfo>? mappingOverride = null)
		{
			var netLanguage = ToDotnetLanguage(InstalledUICulture.Replace("_", "-"));

			// this gets called a lot - try/catch can be expensive so consider caching or something
			CultureInfo ci;
			try
			{
				ci = CultureInfo.GetCultureInfo(netLanguage);
			}
			catch (CultureNotFoundException)
			{
				if (mappingOverride != null)
				{
					return mappingOverride(InstalledUICulture);
				}

				// locale not valid .NET culture (eg. "en-ES" : English in Spain)
				// fallback to first characters, in this case "en"
				try
				{
					var fallback = ToDotnetFallbackLanguage(new InternalCulture(netLanguage));
					ci = CultureInfo.GetCultureInfo(fallback);
				}
				catch (CultureNotFoundException)
				{
					// language not valid .NET culture, falling back to English
					ci = CultureInfo.GetCultureInfo("en");
				}
			}
			return ci;
		}

		static string ToDotnetLanguage(string androidLanguage)
		{
			// Certain languages need to be converted to CultureInfo equivalent

			// Add more application-specific cases here (if required)
			// ONLY use cultures that have been tested and known to work
			var netLanguage = androidLanguage switch
			{
				"ms-BN" or "ms-MY" or "ms-SG" => "ms", // "Malaysian (Brunei)" not supported .NET culture
				"in-ID" => "id-ID", // "Indonesian (Indonesia)" has different code in  .NET
				"gsw-CH" => "de-CH", // "Schwiizertüütsch (Swiss German)" not supported .NET culture
				"iw-IL" => "he-IL", // Hebrew
				_ => androidLanguage,
			};
			return netLanguage;
		}

		static string ToDotnetFallbackLanguage(InternalCulture platCulture)
		{
			// Use the first part of the identifier (two chars, usually)

			// Add more application-specific cases here (if required)
			// ONLY use cultures that have been tested and known to work
			var netLanguage = platCulture.LanguageCode switch
			{
				"gsw" => "de-CH", // equivalent to German (Switzerland) for this app
				"iw" => "he", // Hebrew
				_ => platCulture.LanguageCode,
			};
			return netLanguage;
		}
	}
}