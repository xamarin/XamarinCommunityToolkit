using UIKit;

namespace Xamarin.CommunityToolkit.Helpers
{
	public static class FormsToolkit
	{
		static bool? isiOS9OrNewer;

		public static bool IsiOS9OrNewer
		{
			get
			{
				if (!isiOS9OrNewer.HasValue)
					isiOS9OrNewer = UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

				return isiOS9OrNewer.Value;
			}
		}
	}
}
