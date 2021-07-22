using Android.Views;
using TextAlignment = Xamarin.Forms.TextAlignment;
using ATextAlignment = Android.Views.TextAlignment;

// Copied from Xamarin.Forms (AlignmentExtensions)
namespace Xamarin.CommunityToolkit.Extensions.Internals
{
	static class AlignmentExtensions
	{
		internal static ATextAlignment ToTextAlignment(this TextAlignment alignment) => alignment switch
		{
			TextAlignment.Center => ATextAlignment.Center,
			TextAlignment.End => ATextAlignment.ViewEnd,
			_ => ATextAlignment.ViewStart,
		};

		internal static GravityFlags ToHorizontalGravityFlags(this TextAlignment alignment) => alignment switch
		{
			TextAlignment.Center => GravityFlags.CenterHorizontal,
			TextAlignment.End => GravityFlags.End,
			_ => GravityFlags.Start,
		};

		internal static GravityFlags ToVerticalGravityFlags(this TextAlignment alignment) => alignment switch
		{
			TextAlignment.Start => GravityFlags.Top,
			TextAlignment.End => GravityFlags.Bottom,
			_ => GravityFlags.CenterVertical,
		};
	}
}