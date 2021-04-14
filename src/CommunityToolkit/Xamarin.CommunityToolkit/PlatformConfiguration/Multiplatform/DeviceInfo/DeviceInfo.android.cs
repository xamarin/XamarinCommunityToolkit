using Java.Util;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceInfo
	{
		static string CurrentCultureName => Locale.Default.Language;
	}
}