using Xamarin.Forms;
using WStretch = Windows.UI.Xaml.Media.Stretch;

namespace CommunityToolkit.Maui.UI.Views
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