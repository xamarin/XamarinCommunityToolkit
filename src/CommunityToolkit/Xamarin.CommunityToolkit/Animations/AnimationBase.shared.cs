using System;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Animations
{
    /// <summary>
	/// Base class representation of a pre-built animation.
	/// </summary>
    public abstract class AnimationBase
    {
        readonly Easing? easing;
        readonly uint length = 250;
        readonly Action<double, bool>? onFinished;
        readonly IAnimatable? owner;
        readonly uint rate = 16;
        readonly Func<bool>? shouldRepeat;
        readonly string name;
        protected readonly View[] views;

        protected AnimationBase(
            string name,
            IAnimatable? owner = null,
            uint rate = 16,
            uint length = 250,
            Easing? easing = null,
            Action<double, bool>? onFinished = null,
            Func<bool>? shouldRepeat = null,
            params View[] views)
        {
            if (views is null || !views.Any())
			{
                throw new ArgumentException("Views cannot be null or empty.", nameof(views));
			}

            this.name = name + Guid.NewGuid().ToString();
            this.length = length;
            this.views = views;
            this.easing = easing;
            this.onFinished = onFinished;
            this.owner = owner;
            this.rate = rate;
            this.shouldRepeat = shouldRepeat;
        }

        /// <summary>
		/// Stops the animation.
		/// </summary>
		/// <returns>True if successful, false otherwise.</returns>
        public bool Abort() => GetOwner().AbortAnimation(name);

        /// <summary>
		/// Runs the animation.
		/// </summary>
        public void Commit() => Create().Commit(GetOwner(), name, rate, length, easing, onFinished, shouldRepeat);

        /// <summary>
		/// Gets a value indicating whether the animation is running.
		/// </summary>
        public bool IsRunning => GetOwner().AnimationIsRunning(name);

        protected abstract Animation Create();

        IAnimatable GetOwner() => owner ?? views.First();
    }
}