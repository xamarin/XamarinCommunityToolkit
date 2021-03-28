using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class FadeAnimation : AnimationBase
	{
		public static readonly BindableProperty FadeProperty =
			BindableProperty.Create(nameof(Fade), typeof(double), typeof(AnimationBase), 0.3, BindingMode.TwoWay);

		public double Fade
		{
			get => (double)GetValue(FadeProperty);
			set => SetValue(FadeProperty, value);
		}

		protected override uint DefaultDuration { get; set; } = 300;

		public override Task Animate(View view) =>
			Device.InvokeOnMainThreadAsync(() =>
			{
				view.FadeTo(Fade, Duration, Easing);
				view.FadeTo(1, Duration, Easing);
			});
	}
}