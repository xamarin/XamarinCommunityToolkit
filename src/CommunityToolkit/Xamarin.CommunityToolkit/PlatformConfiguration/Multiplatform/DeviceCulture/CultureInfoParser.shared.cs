using System;
using System.Globalization;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public class CultureInfoParser
	{
		public virtual CultureInfo Parse(string platformCulture)
		{
			if (string.IsNullOrEmpty(platformCulture))
			{
				throw new ArgumentException("Expected culture identifier", nameof(platformCulture));
			}

			platformCulture = FormatCultureString(platformCulture);
			var netLanguage = ToDotnetLanguage(platformCulture);

			// this gets called a lot - try/catch can be expensive so consider caching or something
			CultureInfo ci;
			try
			{
				ci = CultureInfo.GetCultureInfo(netLanguage);
			}
			catch (CultureNotFoundException)
			{
				// Locale not valid .NET culture (eg. "en-ES" : English in Spain)
				// Fallback to first characters, in this case "en"
				var languageCode = GetLanguageCode(netLanguage);
				try
				{
					var fallback = ToDotnetFallbackLanguage(languageCode);
					ci = CultureInfo.GetCultureInfo(fallback);
				}
				catch (CultureNotFoundException)
				{
					// Language not valid .NET culture, falling back to English
					ci = CultureInfo.GetCultureInfo(DeviceCulture.FallbackCulture);
				}
			}
			return ci;
		}

		/// <summary>
		/// Formats culture string to xx-XX format (eg. "en-ES").
		/// </summary>
		protected virtual string FormatCultureString(string platformCulture) =>
			platformCulture.Replace("_", "-"); // .NET expects dash, not underscore

		/// <summary>
		/// Certain languages need to be converted to CultureInfo equivalent.
		///
		/// Add more application-specific cases here (if required).
		/// ONLY use cultures that have been tested and known to work.
		/// </summary>
		protected virtual string ToDotnetLanguage(string platformCulture)
		{
			var netLanguage = platformCulture switch
			{
				"ms-BN" or "ms-MY" or "ms-SG" => "ms", // "Malaysian (Brunei)" not supported .NET culture
				"gsw-CH" => "de-CH", // "Schwiizertüütsch (Swiss German)" not supported .NET culture
				_ => platformCulture,
			};
			return netLanguage;
		}

		/// <summary>
		/// Use the first part of the identifier (two chars, usually).
		///
		/// Add more application-specific cases here (if required).
		/// ONLY use cultures that have been tested and known to work.
		/// </summary>
		protected virtual string ToDotnetFallbackLanguage(string languageCode)
		{
			var netLanguage = languageCode switch
			{
				"gsw" => "de-CH", // equivalent to German (Switzerland)
				_ => languageCode,
			};
			return netLanguage;
		}

		protected virtual string GetLanguageCode(string platformCulture)
		{
			var dashIndex = platformCulture.IndexOf("-", StringComparison.Ordinal);
			if (dashIndex < 1)
			{
				return platformCulture;
			}

			var parts = platformCulture.Split('-');
			return parts[0];
		}
	}
}
