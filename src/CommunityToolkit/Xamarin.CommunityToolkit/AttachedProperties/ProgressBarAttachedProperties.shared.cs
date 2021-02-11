using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.AttachedProperties
{
	/// <summary>
	/// This class defines attached properties that can be uses with a <see cref="ProgressBar"/>.
	/// </summary>
	public static class ProgressBarAttachedProperties
	{
		const string animatedProgress = "AnimatedProgress";

			/// <summary>
		/// Backing BindableProperty for the <see cref="AnimatedProgress"/> property.
		/// </summary>
		public static BindableProperty AnimatedProgressProperty = BindableProperty.CreateAttached(animatedProgress, typeof(double), typeof(ProgressBar), 0.0d, propertyChanged: OnAnimatedProgressChanged);

		static void OnAnimatedProgressChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var progressBar = (ProgressBar)bindable;
			progressBar.ProgressTo((double)newValue, 500, Easing.Linear);
		}
    }
}
