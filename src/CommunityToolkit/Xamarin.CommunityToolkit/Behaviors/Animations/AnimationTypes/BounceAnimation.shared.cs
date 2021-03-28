using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class BounceInAnimation : AnimationBase
	{
		protected override uint DefaultDuration { get; set; } = 300;

		public override Task Animate(View view) =>
			Device.InvokeOnMainThreadAsync(() => view.Animate("BounceIn", BounceIn(view), 16, Duration));

		internal Animation BounceIn(View view)
		{
			var animation = new Animation();

			animation.WithConcurrent(
				f => view.Scale = f,
				0.5, 1,
				Easing.Linear, 0, 1);

			animation.WithConcurrent(
				(f) => view.Opacity = f,
				0, 1,
				null,
				0, 0.25);

			return animation;
		}
	}

	public class BounceOutAnimation : AnimationBase
	{
		protected override uint DefaultDuration { get; set; } = 300;

		public override Task Animate(View view) =>
			Device.InvokeOnMainThreadAsync(() => view.Animate("BounceOut", BounceOut(view), 16, Duration));

		internal Animation BounceOut(View view)
		{
			var animation = new Animation();

			view.Opacity = 1;

			animation.WithConcurrent(
				(f) => view.Opacity = f,
				1, 0,
				null,
				0.5, 1);

			animation.WithConcurrent(
				(f) => view.Scale = f,
				1, 0.3,
				Easing.Linear, 0, 1);

			return animation;
		}
	}
}
