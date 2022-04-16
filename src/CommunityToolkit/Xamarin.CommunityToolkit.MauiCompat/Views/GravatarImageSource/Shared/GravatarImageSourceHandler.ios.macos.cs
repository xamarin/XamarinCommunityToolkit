using System;using Microsoft.Extensions.Logging;
using Foundation;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler
	{
		static string GetCacheDirectory()
		{
			var dirs = NSSearchPath.GetDirectories(NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User);
			if (dirs == null || dirs.Length == 0)
				throw new NotSupportedException();

			return dirs[0];
		}
	}
}