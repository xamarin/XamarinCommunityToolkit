using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Animations;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class TadaAnimationType : AnimationBase
	{
		public static readonly BindableProperty IsRepeatedProperty =
		   BindableProperty.Create(nameof(IsRepeated), typeof(bool), typeof(TadaAnimationType), default, BindingMode.TwoWay);

		public bool IsRepeated
		{
			get => (bool)GetValue(IsRepeatedProperty);
			set => SetValue(IsRepeatedProperty, value);
		}

		public static readonly BindableProperty MaximumScaleProperty =
		   BindableProperty.Create(nameof(MaximumScale), typeof(double), typeof(TadaAnimationType), TadaAnimation.DefaultMaximumScale, BindingMode.TwoWay);

		public double MaximumScale
		{
			get => (double)GetValue(MaximumScaleProperty);
			set => SetValue(MaximumScaleProperty, value);
		}

		public static readonly BindableProperty MinimumScaleProperty =
		   BindableProperty.Create(nameof(MinimumScale), typeof(double), typeof(TadaAnimationType), TadaAnimation.DefaultMinimumScale, BindingMode.TwoWay);

		public double MinimumScale
		{
			get => (double)GetValue(MinimumScaleProperty);
			set => SetValue(MinimumScaleProperty, value);
		}

		public static readonly BindableProperty RotationAngleProperty =
		   BindableProperty.Create(nameof(RotationAngle), typeof(double), typeof(TadaAnimationType), TadaAnimation.DefaultRotationAngle, BindingMode.TwoWay);

		public double RotationAngle
		{
			get => (double)GetValue(RotationAngleProperty);
			set => SetValue(RotationAngleProperty, value);
		}

		protected override uint DefaultDuration { get; set; } = RubberbandAnimation.DefaultLength;

		public override Task Animate(View? view)
		{
			if (view != null)
			{
				var taskCompletionSource = new TaskCompletionSource<bool>();

				new TadaAnimation(
					rotationAngle: RotationAngle,
					length: Duration,
					shouldRepeat: () => IsRepeated,
					onFinished: (v, c) => taskCompletionSource.SetResult(c),
					views: view).Commit();

				return taskCompletionSource.Task;
			}

			return Task.FromResult(false);
		}
	}
}
