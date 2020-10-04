using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Core
{
	// TODO 2: Is this even used?! Not for iOS it seems...
	public class CameraMediaSource : MediaSource
	{
		public static MediaSource FromStream(Func<Stream> stream)
			=> new StreamMediaSource { Stream = token => Task.Run(stream, token) };

		public static MediaSource FromResource(string resource, Type resolvingType)
		=> FromResource(resource, resolvingType.GetTypeInfo().Assembly);

		public static MediaSource FromResource(string resource, Assembly sourceAssembly = null)
		{
#if NETSTANDARD2_0
			sourceAssembly = sourceAssembly ?? Assembly.GetCallingAssembly();
#else
			if (sourceAssembly == null)
			{
				var callingAssemblyMethod = typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly");
				if (callingAssemblyMethod == null)
				{
					Forms.Internals.Log.Warning("Warning", "Can not find CallingAssembly, pass resolvingType to FromResource to ensure proper resolution");
					return null;
				}
				sourceAssembly = (Assembly)callingAssemblyMethod.Invoke(null, new object[0]);
			}
#endif
			return FromStream(() => sourceAssembly.GetManifestResourceStream(resource));
		}
	}
}