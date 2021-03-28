using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class ScaleAnimation : AnimationBase
	{
		public static readonly BindableProperty ScaleProperty =
		   BindableProperty.Create(nameof(Scale), typeof(double), typeof(AnimationBase), 1.2, BindingMode.TwoWay);

		public double Scale
		{
			get => (double)GetValue(ScaleProperty);
			set => SetValue(ScaleProperty, value);
		}

		protected override uint DefaultDuration { get; set; } = 170;

		public override Task Animate(View view) =>
			Device.InvokeOnMainThreadAsync(() =>
			{
				view.ScaleTo(Scale, Duration, Easing);
				view.ScaleTo(1, Duration, Easing);
			});
	}

	public class RelScaleAnimation : AnimationBase
	{
		public static readonly BindableProperty ScaleProperty =
			BindableProperty.Create(nameof(Scale), typeof(double), typeof(RelScaleAnimation), default(double),
				BindingMode.TwoWay, null);

		public double Scale
		{
			get => (double)GetValue(ScaleProperty);
			set => SetValue(ScaleProperty, value);
		}

		protected override uint DefaultDuration { get; set; } = 170;

		public override Task Animate(View view) =>
			Device.InvokeOnMainThreadAsync(() => view.RelScaleTo(Scale, Duration, Easing));
	}
}