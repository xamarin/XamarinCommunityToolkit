using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class AnimationWrapper
	{
		readonly Easing? easing;
		readonly uint length = 250;
		readonly Action<double, bool>? onFinished;
		readonly Animation animation;
		readonly IAnimatable owner;
		readonly uint rate = 16;
		readonly Func<bool>? shouldRepeat;
		readonly string name;

		public AnimationWrapper(
			Animation animation,
			string name,
			IAnimatable owner,
			uint rate = 16,
			uint length = 250,
			Easing? easing = null,
			Action<double, bool>? onFinished = null,
			Func<bool>? shouldRepeat = null)
		{
			this.name = name + Guid.NewGuid().ToString();
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
