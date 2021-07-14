using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Animations;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class TadaAnimationType : AnimationBase
	{
		public static readonly BindableProperty RotationAngleProperty =
		   BindableProperty.Create(nameof(RotationAngle), typeof(double), typeof(AnimationBase), TadaAnimation.DefaultRotationAngle, BindingMode.TwoWay);

		public double RotationAngle
		{
			get => (double)GetValue(RotationAngleProperty);
			set => SetValue(RotationAngleProperty, value);
		}

		// Repeat?
		// MaximumScale
		// MinimumScale

		protected override uint DefaultDuration { get; set; } = RubberbandAnimation.DefaultLength;

		public override Task Animate(View? view)
		{
			if (view != null)
			{
				var taskCompletionSource = new TaskCompletionSource<bool>();

				new TadaAnimation(
					rotationAngle: RotationAngle,
					length: Duration,
					onFinished: (v, c) => taskCompletionSource.SetResult(c),
					views: view).Commit();

				return taskCompletionSource.Task;
			}

			// Find the alternative to Task.CompletedTask in netstandard1.0
			return Task.Delay(1);
		}
	}
}
