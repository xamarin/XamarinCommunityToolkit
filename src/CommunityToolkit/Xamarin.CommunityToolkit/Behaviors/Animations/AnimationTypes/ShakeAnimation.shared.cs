using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class ShakeAnimation : AnimationBase
	{
		public static readonly BindableProperty StartFactorProperty =
		  BindableProperty.Create(nameof(StartFactor), typeof(double), typeof(AnimationBase), 15.0, BindingMode.TwoWay);

		public double StartFactor
		{
			get => (double)GetValue(StartFactorProperty);
			set => SetValue(StartFactorProperty, value);
		}

		protected override uint DefaultDuration { get; set; } = 50;

		public override Task Animate(View view) =>
			Device.InvokeOnMainThreadAsync(() =>
			{
				for (var i = StartFactor; i > 0; i -= 5)
				{
					view.TranslateTo(-i, 0, Duration, Easing);
					view.TranslateTo(i, 0, Duration, Easing);
				}

				view.TranslationX = 0;
			});
	}
}