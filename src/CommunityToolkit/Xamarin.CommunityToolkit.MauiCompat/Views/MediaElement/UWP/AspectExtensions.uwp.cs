using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using WStretch = Microsoft.UI.Xaml.Media.Stretch;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public static class AspectExtensions
	{
		public static WStretch ToStretch(this Aspect aspect) => aspect switch
		{
			Aspect.Fill => WStretch.Fill,
			Aspect.AspectFill => WStretch.UniformToFill,
			_ => WStretch.Uniform,
		};
	}
}