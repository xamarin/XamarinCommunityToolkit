using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	class AnimationWrapper
	{
		readonly Easing easing;
		readonly uint length;
		readonly Action<double, bool> onFinished;
		readonly Animation animation;
		readonly IAnimatable owner;
		readonly uint rate;
		readonly Func<bool> shouldRepeat;
		readonly string name;

		public AnimationWrapper(
			Animation animation,
			string name,
			IAnimatable owner,
			uint rate,
			uint length,
			Easing easing,
			Action<double, bool> onFinished,
			Func<bool> shouldRepeat)
		{
			this.name = name;
			this.length = length;
			this.easing = easing;
			this.onFinished = onFinished;
			this.animation = animation;
			this.owner = owner;
			this.rate = rate;
			this.shouldRepeat = shouldRepeat;
		}

		/// <summary>
		/// Stops the animation.
		/// </summary>
		/// <returns>True if successful, false otherwise.</returns>
		public bool Abort() => owner.AbortAnimation(name);

		/// <summary>
		/// Runs the animation.
		/// </summary>
		public void Commit() => animation.Commit(owner, name, rate, length, easing, onFinished, shouldRepeat);

		/// <summary>
		/// Gets a value indicating whether the animation is running.
		/// </summary>
		public bool IsRunning => owner.AnimationIsRunning(name);
	}
}