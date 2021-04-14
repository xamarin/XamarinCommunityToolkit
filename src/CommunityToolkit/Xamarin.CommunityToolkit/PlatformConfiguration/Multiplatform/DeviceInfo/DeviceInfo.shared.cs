using System.Globalization;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceInfo
	{
		public static CultureInfo CurrentCulture => new CultureInfo(CurrentCultureName);
	}
}