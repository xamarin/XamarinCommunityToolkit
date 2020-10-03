using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.AttachedProperties
{
	public static class ProgressBarAttachedProperties
	{
        public static BindableProperty AnimatedProgressProperty = BindableProperty.CreateAttached("AnimatedProgress", typeof(double), typeof(ProgressBar), 0.0d, BindingMode.OneWay, propertyChanged: (b, o, n) => ProgressBarProgressChanged((ProgressBar)b, (double)n));

        static void ProgressBarProgressChanged(ProgressBar progressBar, double progress)
        {
            ViewExtensions.CancelAnimations(progressBar);
            progressBar.ProgressTo(progress, Convert.ToUInt32(Math.Max(0, 500)), Easing.Linear);
        }
    }
}