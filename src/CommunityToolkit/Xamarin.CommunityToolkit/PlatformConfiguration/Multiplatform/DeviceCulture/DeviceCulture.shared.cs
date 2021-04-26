using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
#if !NETSTANDARD1_0
	public static partial class DeviceCulture
	{
		/// <summary>
		/// Gets the set of languages that are preferred by the user, in order of preference.
		/// </summary>
		/// <returns>One or more language identifiers for the user's preferred languages.</returns>
		public static IReadOnlyList<string> PreferredCultureStrings => GetPreferredCultureStrings();

		private static partial IReadOnlyList<string> GetPreferredCultureStrings();

		internal static readonly string FallbackCulture = "en";
	}
#endif
}