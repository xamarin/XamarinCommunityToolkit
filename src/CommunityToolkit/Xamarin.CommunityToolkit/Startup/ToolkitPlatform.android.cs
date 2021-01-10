using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit
{
	/// <summary>
	/// Platform extension methods.
	/// </summary>
	static class ToolkitPlatform
	{
		/// <summary>
		/// Gets the <see cref="Context"/>.
		/// </summary>
		public static Context Context
		{
			get
			{
				var page = Forms.Application.Current.MainPage;
				var renderer = page.GetRenderer();
				return renderer.View.Context;
			}
		}
	}
}