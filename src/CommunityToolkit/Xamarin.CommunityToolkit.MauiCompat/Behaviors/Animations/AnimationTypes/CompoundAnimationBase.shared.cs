using System;using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	/// <summary>
	/// Represents a compound animation that can be used to animate multiple <see cref="View"/>s.
	/// </summary>
	public abstract class CompoundAnimationBase : AnimationBase
	{
		/// <summary>
		/// The <see cref="BindableProperty"/> backing store for the <see cref="IsRepeated"/> property.
		/// </summary>
		public static readonly BindableProperty IsRepeatedProperty =
		   BindableProperty.Create(nameof(IsRepeated), typeof(bool), typeof(CompoundAnimationBase), default, BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets a value indicating whether the animation will repeat.
		/// </summary>
		public bool IsRepeated
		{
			get => (bool)GetValue(IsRepeatedProperty);
			set => SetValue(IsRepeatedProperty, value);
		}

		/// <inheritdoc />
		public override Task Animate(View? view) => Animate(CancellationToken.None, view!);

		/// <summary>
		/// Runs the animation with the supplied parameters.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to allow for cancelling of the animation.</param>
		/// <param name="views">All <see cref="View"/>s to be animated.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public async Task Animate(CancellationToken? cancellationToken, params View[]? views)
		{
			if (views is null || views.Length == 0)
			{
				return;
			}

			AnimationWrapper? animation = null;

			var taskCompletionSource = new TaskCompletionSource<bool>();

			cancellationToken ??= CancellationToken.None;

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

						animation = null;
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

			await Task.WhenAny(cancellationToken.Value.WhenCanceled(), taskCompletionSource.Task);
			animation?.Abort();
		}

		AnimationWrapper CreateAnimation(
			uint rate,
			Action<double, bool> onFinished,
			Func<bool> shouldRepeat,
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

		/// <summary>
		/// Creates the animation ready for running.
		/// </summary>
		/// <param name="views">All <see cref="View"/>s to be animated.</param>
		/// <returns>The <see cref="Animation"/> that will be run.</returns>
		protected abstract Animation CreateAnimation(params View[] views);
	}
}