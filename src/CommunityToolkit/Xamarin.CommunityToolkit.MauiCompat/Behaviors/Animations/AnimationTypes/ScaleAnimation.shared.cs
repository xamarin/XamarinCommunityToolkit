using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

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

		public override async Task Animate(View? view)
		{
			if (view != null)
			{
				await view.ScaleTo(Scale, Duration, Easing);
				await view.ScaleTo(1, Duration, Easing);
			}
		}
	}
}