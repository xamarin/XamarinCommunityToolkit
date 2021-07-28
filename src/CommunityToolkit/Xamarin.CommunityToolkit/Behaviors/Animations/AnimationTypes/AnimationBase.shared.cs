using System;
using System.Linq;
using System.Threading;
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
	}

	public abstract class AnimationBase : AnimationBase<View>
	{
	}

	public abstract class PreBuiltAnimationBase : AnimationBase
	{
		public static readonly BindableProperty IsRepeatedProperty =
		   BindableProperty.Create(nameof(IsRepeated), typeof(bool), typeof(PreBuiltAnimationBase), default, BindingMode.TwoWay);

		public bool IsRepeated
		{
			get => (bool)GetValue(IsRepeatedProperty);
			set => SetValue(IsRepeatedProperty, value);
		}

		public override Task Animate(View? view) => Animate(CancellationToken.None, view!);

		public async Task Animate(CancellationToken? cancellationToken, params View[]? views)
		{
			if (views != null)
			{
				AnimationWrapper? animation = null;

				try
				{
					var taskCompletionSource = new TaskCompletionSource<bool>();

					if (cancellationToken is null)
						cancellationToken = CancellationToken.None;

					animation = CreateAnimation(
						16,
						onFinished: (v, c) =>
						{
							try
							{
								if (cancellationToken != null)
								{
									cancellationToken.Value.ThrowIfCancellationRequested();
								}

								if (IsRepeated)
								{
									return;
								}

								taskCompletionSource.SetResult(c);
							}
							catch (OperationCanceledException)
							{
								taskCompletionSource.SetCanceled();
								animation?.Abort();
							}
						},
						shouldRepeat: () => IsRepeated,
						views);

					animation.Commit();

					// Does not take in to account IsRepeated
					await Task.WhenAny(Task.Delay((int)Duration, cancellationToken.Value), taskCompletionSource.Task).Unwrap();
				}
				catch (OperationCanceledException)
				{
					animation?.Abort();
				}
			}
		}

		AnimationWrapper CreateAnimation(
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

		protected abstract Animation CreateAnimation(params View[] views);
	}
}