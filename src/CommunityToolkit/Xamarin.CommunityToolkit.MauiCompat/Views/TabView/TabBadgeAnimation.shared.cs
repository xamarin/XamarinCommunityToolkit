using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class TabBadgeAnimation : ITabBadgeAnimation
	{
		protected uint AnimationLength { get; } = 150;

		protected uint Offset { get; } = 24;

		double? translationY;

		public Task OnAppearing(TabBadgeView badgeView)
		{
			if (translationY == null)
			{
				translationY = badgeView.TranslationY;
			}

			var tcs = new TaskCompletionSource<bool>();

			var appearingAnimation = new Animation();

			appearingAnimation.WithConcurrent(
				(f) => badgeView.Opacity = f,
				0, 1, Easing.CubicOut);

			appearingAnimation.WithConcurrent(
				(f) => badgeView.TranslationY = f,
				translationY.Value + Offset, translationY.Value);

			appearingAnimation.Commit(badgeView, nameof(OnAppearing), length: AnimationLength,
			   finished: (v, t) => tcs.SetResult(true));

			return tcs.Task;
		}

		public Task OnDisappering(TabBadgeView badgeView)
		{
			if (translationY == null)
			{
				translationY = badgeView.TranslationY;
			}

			var tcs = new TaskCompletionSource<bool>();

			var disapperingAnimation = new Animation();

			disapperingAnimation.WithConcurrent(
				(f) => badgeView.Opacity = f,
				1, 0);

			disapperingAnimation.WithConcurrent(
				(f) => badgeView.TranslationY = f,
				translationY.Value, translationY.Value + Offset);

			disapperingAnimation.Commit(badgeView, nameof(OnAppearing), length: AnimationLength,
				finished: (v, t) => tcs.SetResult(true));

			return tcs.Task;
		}
	}
}