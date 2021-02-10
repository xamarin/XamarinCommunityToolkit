using System;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class ProgressBarAnimationBehavior : BaseBehavior<ProgressBar>
	{
		public static readonly BindableProperty AnimateProgressProperty =
			BindableProperty.CreateAttached(nameof(AnimateProgress), typeof(double), typeof(ProgressBar), 0.0d, propertyChanged: OnAnimateProgressPropertyChanged);

		public double AnimateProgress
		{
			get => (double)GetValue(AnimateProgressProperty);
			set => SetValue(AnimateProgressProperty, value);
		}

		static void OnAnimateProgressPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ProgressBarAnimationBehavior)bindable).Animate();

		void Animate()
		{
			View.ProgressTo(AnimateProgress, 500, Easing.Linear);
		}
	}
}