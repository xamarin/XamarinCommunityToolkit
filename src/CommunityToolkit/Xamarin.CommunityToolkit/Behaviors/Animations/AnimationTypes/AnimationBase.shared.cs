using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public abstract class AnimationBase<TView> : BindableObject
		where TView : View
	{
		public static readonly BindableProperty DurationProperty =
			BindableProperty.Create(nameof(Duration), typeof(uint), typeof(AnimationBase<TView>), default(uint),
				BindingMode.TwoWay, defaultValueCreator: GetDefaultDurationProperty);

		public uint Duration
		{
			get => (uint)GetValue(DurationProperty);
			set => SetValue(DurationProperty, value);
		}

		public static readonly BindableProperty EasingTypeProperty =
		   BindableProperty.Create(nameof(Easing), typeof(Easing), typeof(AnimationBase<TView>), Easing.Linear,
			   BindingMode.TwoWay);

		public Easing Easing
		{
			get => (Easing)GetValue(EasingTypeProperty);
			set => SetValue(EasingTypeProperty, value);
		}

		static object GetDefaultDurationProperty(BindableObject bindable)
			=> ((AnimationBase<TView>)bindable).DefaultDuration;

		protected abstract uint DefaultDuration { get; set; }

		public abstract Task Animate(TView? view);

		// TODO: Wrap this (no pun intended) in another base class just for the pre-built types.
		public AnimationWrapper CreateAnimation(
			uint rate = 16,
			Action<double, bool>? onFinished = null,
			Func<bool>? shouldRepeat = null,
			params View[] views) =>
			new AnimationWrapper(
				CreateAnimation(views),
				Guid.NewGuid().ToString(),
				views.First(),
				rate,
				Duration,
				Easing,
				onFinished,
				shouldRepeat);

		protected virtual Animation CreateAnimation(params View[] views)
		{
			return new Animation();
		}
	}

	public abstract class AnimationBase : AnimationBase<View>
	{
	}
}