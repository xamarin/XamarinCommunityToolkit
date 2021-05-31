using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		/// <summary>
		/// Gets the set of languages that are preferred by the user, in order of preference.
		/// </summary>
		/// <returns>A well-formed list of IETF BCP 47 language tags.</returns>
		public static IReadOnlyList<string> PreferredCultureStrings => GetPreferredCultureStrings();

		private static partial IReadOnlyList<string> GetPreferredCultureStrings();
	}
}