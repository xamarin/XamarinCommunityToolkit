using System.Collections.Generic;
using Foundation;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() =>
			NSBundle.MainBundle.PreferredLocalizations;
	}
}