using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Animations;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors.Animations.AnimationTypes
{
	public class RubberBandAnimationType : AnimationBase
	{
		protected override uint DefaultDuration { get; set; } = RubberBandAnimation.DefaultLength;

		public override Task Animate(View? view)
		{
			if (view != null)
			{
				var taskCompletionSource = new TaskCompletionSource<bool>();

				new RubberBandAnimation(
					length: Duration,
					onFinished: (v, c) => taskCompletionSource.SetResult(c),
					views: view).Commit();

				return taskCompletionSource.Task;
			}

			return Task.FromResult(false);
		}
	}
}
