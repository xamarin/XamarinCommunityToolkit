using System;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class DeviceInfo
	{
		static string CurrentCultureName => throw new PlatformNotSupportedException();
	}
}