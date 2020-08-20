using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	// TODO: Check if we can do these changes in Xamarin.Forms to not have our own version of the MediaSource
	public class CameraMediaSource : MediaSource
	{
		public static MediaSource FromStream(Func<Stream> stream) => new StreamMediaSource { Stream = token => Task.Run(stream, token) };

		public static MediaSource FromResource(string resource, Type resolvingType)
		{
			return FromResource(resource, resolvingType.GetTypeInfo().Assembly);
		}

		public static MediaSource FromResource(string resource, Assembly sourceAssembly = null)
		{
#if NETSTANDARD2_0
			sourceAssembly = sourceAssembly ?? Assembly.GetCallingAssembly();
#else
			if (sourceAssembly == null)
			{
				var callingAssemblyMethod = typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly");
				if (callingAssemblyMethod != null)
				{
					sourceAssembly = (Assembly)callingAssemblyMethod.Invoke(null, new object[0]);
				}
				else
				{
					Forms.Internals.Log.Warning("Warning", "Can not find CallingAssembly, pass resolvingType to FromResource to ensure proper resolution");
					return null;
				}
			}
#endif
			return FromStream(() => sourceAssembly.GetManifestResourceStream(resource));
		}
	}
}