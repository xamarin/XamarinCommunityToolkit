using System;
using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit
{
	/// <summary>
	/// Platform extension methods.
	/// </summary>
	static class ToolkitPlatform
	{
		static Context? context;

		/// <summary>
		/// Gets the <see cref="Context"/>.
		/// </summary>
		internal static Context Context
		{
			get
			{
				var page = Forms.Application.Current.MainPage;
				var renderer = page.GetRenderer();

				if (renderer?.View.Context is not null)
					context = renderer.View.Context;

				return renderer?.View.Context ?? context ?? throw new NullReferenceException($"{nameof(Context)} cannot be null");
			}
		}
	}
}