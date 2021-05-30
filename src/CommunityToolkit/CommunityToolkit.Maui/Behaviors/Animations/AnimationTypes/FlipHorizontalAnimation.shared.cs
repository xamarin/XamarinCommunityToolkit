using System.Threading.Tasks;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Behaviors
{
	public class FlipHorizontalAnimation : RotateAnimation
	{
		protected override double DefaultRotation { get; set; } = 90;

		protected override uint DefaultDuration { get; set; } = 300;

		public override async Task Animate(View? view)
		{
			if (view != null)
			{
				await view.RotateYTo(Rotation, Duration, Easing);
				await view.RotateYTo(0, Duration, Easing);
			}
		}
	}
}