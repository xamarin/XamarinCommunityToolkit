using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		/// <summary>
		/// Gets the set of languages that are preferred by the user, in order of preference.
		/// </summary>
		/// <returns>One or more language identifiers for the user's preferred languages.</returns>
		public static IReadOnlyList<string> PreferredCultureStrings => GetPreferredCultureStrings();

		/// <summary>
		/// Gets the set of cultures that are preferred by the user, in order of preference.
		/// </summary>
		/// <returns>One or more culture identifiers for the user's preferred cultures.</returns>
		public static IReadOnlyList<CultureInfo> GetPreferredCultures() =>
			GetPreferredCultures(GetCultureParser());

		/// <summary>
		/// Gets the set of cultures that are preferred by the user, in order of preference.
		/// </summary>
		/// <returns>One or more culture identifiers for the user's preferred cultures.</returns>
		public static IReadOnlyList<CultureInfo> GetPreferredCultures(CultureInfoParser cultureParser) =>
			PreferredCultureStrings.Select(s => cultureParser.Parse(s)).ToList();

		private static partial IReadOnlyList<string> GetPreferredCultureStrings();

		private static partial CultureInfoParser GetCultureParser();

		internal static readonly string FallbackCulture = "en";
	}
}