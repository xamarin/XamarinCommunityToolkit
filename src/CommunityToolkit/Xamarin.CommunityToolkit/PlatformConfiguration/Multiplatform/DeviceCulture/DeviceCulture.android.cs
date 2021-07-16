using System.Collections.Generic;
using Java.Util;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() =>
			new[] { Locale.Default.ToLanguageTag() };
	}
}