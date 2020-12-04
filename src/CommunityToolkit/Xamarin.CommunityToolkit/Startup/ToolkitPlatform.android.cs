using Android.Content;

namespace Xamarin.CommunityToolkit
{
	/// <summary>
	/// Platform extension methods.
	/// </summary>
	public static partial class ToolkitPlatform
	{
		/// <summary>
		/// Gets the <see cref="Context"/>.
		/// </summary>
		internal static Context Context { get; private set; }

		/// <summary>
		/// Initializes the Android <see cref="Context"/>.
		/// </summary>
		/// <param name="context">
		/// The current <see cref="Context"/>.
		/// </param>
		public static void Init(Context context)
		{
			Context = context;
		}
	}
}
