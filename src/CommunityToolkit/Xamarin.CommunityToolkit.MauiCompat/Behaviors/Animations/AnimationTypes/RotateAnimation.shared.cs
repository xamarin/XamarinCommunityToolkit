using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class RotateAnimation : AnimationBase
	{
		public static readonly BindableProperty RotationProperty =
		   BindableProperty.Create(nameof(Rotation), typeof(double), typeof(AnimationBase), 180.0, BindingMode.TwoWay, defaultValueCreator: GetDefaulRotationProperty);

		public double Rotation
		{
			get => (double)GetValue(RotationProperty);
			set => SetValue(RotationProperty, value);
		}

		static object GetDefaulRotationProperty(BindableObject bindable)
			=> ((RotateAnimation)bindable).DefaultRotation;

		protected override uint DefaultDuration { get; set; } = 200;

		protected virtual double DefaultRotation { get; set; } = 180.0;

		public override async Task Animate(View? view)
		{
			if (view != null)
			{
				await view.RotateTo(Rotation, Duration, Easing);
				view.Rotation = 0;
			}
		}
	}
}