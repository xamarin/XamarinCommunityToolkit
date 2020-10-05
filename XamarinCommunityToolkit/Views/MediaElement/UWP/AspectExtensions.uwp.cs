using Xamarin.Forms;
using WStretch = Windows.UI.Xaml.Media.Stretch;

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