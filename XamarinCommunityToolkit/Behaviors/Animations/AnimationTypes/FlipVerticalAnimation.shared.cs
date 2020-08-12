using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class FlipVerticalAnimation : RotateAnimation
	{
		protected override double DefaultRotation { get; set; } = 90;

		public override async Task Animate(View view)
		{
			await view.RotateXTo(Rotation, Duration, Easing);
			await view.RotateXTo(0, Duration, Easing);
		}
	}
}