using System;
using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
#if !NETSTANDARD1_0
	public static partial class DeviceCulture
	{
		private static partial IReadOnlyList<string> GetPreferredCultureStrings() => throw new PlatformNotSupportedException();

		private static partial CultureInfoParser GetCultureParser() => throw new PlatformNotSupportedException();
	}
#endif
}