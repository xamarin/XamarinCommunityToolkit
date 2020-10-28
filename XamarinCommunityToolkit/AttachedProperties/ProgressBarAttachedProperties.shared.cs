using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.AttachedProperties
{
	public static class ProgressBarAttachedProperties
	{
		const string animatedProgress = "AnimatedProgres";

		public static BindableProperty AnimatedProgressProperty = BindableProperty.CreateAttached(animatedProgress, typeof(double), typeof(ProgressBar), 0.0d, propertyChanged: ProgressBarProgressChanged);

		static void ProgressBarProgressChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var progressBar = (ProgressBar)bindable;
			progressBar.ProgressTo((double)newValue, 500, Easing.Linear);
		}
    }
}