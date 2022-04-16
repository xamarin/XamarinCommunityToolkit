using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
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