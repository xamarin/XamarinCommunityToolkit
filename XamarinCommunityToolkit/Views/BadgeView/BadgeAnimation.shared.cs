using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
    public class BadgeAnimation : IBadgeAnimation
    {
        protected uint AnimationLength { get; } = 150;
        protected uint Offset { get; } = 24;

        double? _translationY;

        public Task OnAppearing(View badgeView)
        {
            if (_translationY == null)
                _translationY = badgeView.TranslationY;

            var tcs = new TaskCompletionSource<bool>();

            var appearingAnimation = new Animation();

            appearingAnimation.WithConcurrent(
                (f) => badgeView.Opacity = f,
                0, 1, Easing.CubicOut);

            appearingAnimation.WithConcurrent(
                (f) => badgeView.TranslationY = f,
                _translationY.Value + Offset, _translationY.Value);

            appearingAnimation.Commit(badgeView, nameof(OnAppearing), length: AnimationLength,
               finished: (v, t) => tcs.SetResult(true));

            return tcs.Task;
        }

        public Task OnDisappering(View badgeView)
        {
            if (_translationY == null)
                _translationY = badgeView.TranslationY;

            var tcs = new TaskCompletionSource<bool>();

            var disapperingAnimation = new Animation();

            disapperingAnimation.WithConcurrent(
                (f) => badgeView.Opacity = f,
                1, 0);

            disapperingAnimation.WithConcurrent(
                (f) => badgeView.TranslationY = f,
                _translationY.Value, _translationY.Value + Offset);

            disapperingAnimation.Commit(badgeView, nameof(OnAppearing), length: AnimationLength,
                finished: (v, t) => tcs.SetResult(true));

            return tcs.Task;
        }
    }
}