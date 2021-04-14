using Windows.System.UserProfile;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceInfo
	{
		static string CurrentCultureName => GlobalizationPreferences.Languages[0];
	}
}