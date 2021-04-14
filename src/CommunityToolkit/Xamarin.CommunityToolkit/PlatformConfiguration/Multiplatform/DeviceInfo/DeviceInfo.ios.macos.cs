using Foundation;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceInfo
	{
		static string CurrentCultureName => NSLocale.CurrentLocale.LanguageCode;
	}
}