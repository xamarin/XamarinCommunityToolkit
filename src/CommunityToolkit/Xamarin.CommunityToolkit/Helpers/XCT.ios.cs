using UIKit;

namespace Xamarin.CommunityToolkit.Helpers
{
	static class XCT
	{
		internal static bool IsiOS13OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);
	}
}