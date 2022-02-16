using UIKit;

namespace Xamarin.CommunityToolkit.Helpers
{
	static partial class XCT
	{
		static bool? isiOS13OrNewer;

		internal static bool IsiOS13OrNewer => isiOS13OrNewer ??= UIDevice.CurrentDevice.CheckSystemVersion(13, 0);
	}
}