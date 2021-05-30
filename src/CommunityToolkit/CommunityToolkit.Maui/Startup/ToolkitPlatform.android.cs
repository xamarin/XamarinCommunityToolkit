using System;
using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace CommunityToolkit.Maui
{
	/// <summary>
	/// Platform extension methods.
	/// </summary>
	static class ToolkitPlatform
	{
		/// <summary>
		/// Gets the <see cref="Context"/>.
		/// </summary>
		internal static Context Context
		{
			get
			{
				var page = Xamarin.Forms.Application.Current.MainPage;
				var renderer = page.GetRenderer();
				return renderer.View.Context ?? throw new NullReferenceException($"{nameof(Context)} cannot be null");
			}
		}
	}
}