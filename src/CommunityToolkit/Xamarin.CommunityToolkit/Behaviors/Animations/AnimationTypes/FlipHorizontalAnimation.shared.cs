using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class FlipHorizontalAnimation : RotateAnimation
	{
		protected override double DefaultRotation { get; set; } = 90;

		protected override uint DefaultDuration { get; set; } = 300;

		public override async Task Animate(View view)
		{
			await view.RotateYTo(Rotation, Duration, Easing);
			await view.RotateYTo(0, Duration, Easing);
		}
	}
}