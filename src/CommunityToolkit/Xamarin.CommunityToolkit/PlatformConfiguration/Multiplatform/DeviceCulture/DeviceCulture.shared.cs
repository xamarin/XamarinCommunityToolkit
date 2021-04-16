using System;
using System.Collections.Generic;
using System.Globalization;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		internal static readonly string FallbackCulture = "en";

		public static string InstalledUICulture =>
			PlatformInstalledUICulture;

		public static CultureInfo GetCurrentUICulture() =>
			PlatformGetCurrentUICulture(null);

		public static CultureInfo GetCurrentUICulture(Func<string, CultureInfo> mappingOverride) =>
			PlatformGetCurrentUICulture(mappingOverride);

		internal class InternalCulture
		{
			internal InternalCulture(string platformCultureString)
			{
				if (string.IsNullOrEmpty(platformCultureString))
				{
					throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));
				}

				PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
				var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
				if (dashIndex > 0)
				{
					var parts = PlatformString.Split('-');
					LanguageCode = parts[0];
					LocaleCode = parts[1];
				}
				else
				{
					LanguageCode = PlatformString;
					LocaleCode = string.Empty;
				}
			}

			public string PlatformString { get; }

			public string LanguageCode { get; }

			public string LocaleCode { get; }

			public override string ToString() => PlatformString;
		}
	}
}