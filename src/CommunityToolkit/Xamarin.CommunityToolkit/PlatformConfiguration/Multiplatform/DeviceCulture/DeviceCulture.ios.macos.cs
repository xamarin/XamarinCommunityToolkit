using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() =>
			NSBundle.MainBundle.PreferredLocalizations.Select(l => l.Replace('_', '-')).ToList();
	}
}