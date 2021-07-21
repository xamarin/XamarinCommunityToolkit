using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class TadaAnimationType : AnimationBase
	{
		public static readonly BindableProperty IsRepeatedProperty =
		   BindableProperty.Create(nameof(IsRepeated), typeof(bool), typeof(TadaAnimationType), default, BindingMode.TwoWay);

		// TODO: RepeatingAnimationBase...
		public bool IsRepeated
		{
			get => (bool)GetValue(IsRepeatedProperty);
			set => SetValue(IsRepeatedProperty, value);
		}

		public static readonly BindableProperty MaximumScaleProperty =
		   BindableProperty.Create(nameof(MaximumScale), typeof(double), typeof(TadaAnimationType), 1.1, BindingMode.TwoWay);

		public double MaximumScale
		{
			get => (double)GetValue(MaximumScaleProperty);
			set => SetValue(MaximumScaleProperty, value);
		}

		public static readonly BindableProperty MinimumScaleProperty =
		   BindableProperty.Create(nameof(MinimumScale), typeof(double), typeof(TadaAnimationType), 0.9, BindingMode.TwoWay);

		public double MinimumScale
		{
			get => (double)GetValue(MinimumScaleProperty);
			set => SetValue(MinimumScaleProperty, value);
		}

		public static readonly BindableProperty RotationAngleProperty =
		   BindableProperty.Create(nameof(RotationAngle), typeof(double), typeof(TadaAnimationType), 3.0, BindingMode.TwoWay);

		public double RotationAngle
		{
			get => (double)GetValue(RotationAngleProperty);
			set => SetValue(RotationAngleProperty, value);
		}

		protected override uint DefaultDuration { get; set; } = 1000;

		public override Task Animate(View? view)
		{
			if (view != null)
			{
				var taskCompletionSource = new TaskCompletionSource<bool>();

				CreateAnimation(
					16,
					onFinished: (v, c) =>
					{
						if (IsRepeated)
						{
							return;
						}

						taskCompletionSource.SetResult(c);
					},
					shouldRepeat: () => IsRepeated,
					view).Commit();

				return taskCompletionSource.Task;
			}

			return Task.FromResult(false);
		}

		protected override Animation CreateAnimation(params View[] views) => Create(RotationAngle, MinimumScale, MaximumScale, views);

		static Animation Create(double rotationAngle, double minimumScale, double maximumScale, params View[] views)
		{
			var animation = new Animation();

			foreach (var view in views)
			{
				animation.Add(0, 0.1, new Animation(v => view.Scale = v, 1, minimumScale));
				animation.Add(0.2, 0.3, new Animation(v => view.Scale = v, minimumScale, maximumScale));
				animation.Add(0.9, 1.0, new Animation(v => view.Scale = v, maximumScale, 1));

				animation.Add(0, 0.2, new Animation(v => view.Rotation = v, 0, -rotationAngle));
				animation.Add(0.2, 0.3, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.3, 0.4, new Animation(v => view.Rotation = v, rotationAngle, -rotationAngle));
				animation.Add(0.4, 0.5, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.5, 0.6, new Animation(v => view.Rotation = v, rotationAngle, -rotationAngle));
				animation.Add(0.6, 0.7, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.7, 0.8, new Animation(v => view.Rotation = v, rotationAngle, -rotationAngle));
				animation.Add(0.8, 0.9, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.9, 1.0, new Animation(v => view.Rotation = v, rotationAngle, 0));
			}

			return animation;
		}
	}
}
