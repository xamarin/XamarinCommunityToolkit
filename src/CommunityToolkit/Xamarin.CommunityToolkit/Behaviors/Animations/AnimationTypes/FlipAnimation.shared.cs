using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
    public class FlipHorizontalAnimation : AnimationBase
    {
        public enum FlipHorizontalDirection
        {
            Left,
            Right
        }

        public static readonly BindableProperty DirectionProperty =
            BindableProperty.Create(nameof(Direction), typeof(FlipHorizontalDirection), typeof(FlipHorizontalAnimation), FlipHorizontalDirection.Right,
                BindingMode.TwoWay, null);

        public FlipHorizontalDirection Direction
        {
            get => (FlipHorizontalDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        protected override uint DefaultDuration { get; set; } = 300;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.Animate("Flip", Flip(view), 16, Duration));

        internal Animation Flip(View view)
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => view.Opacity = f, 0.5, 1);
            animation.WithConcurrent((f) => view.RotationY = f, (Direction == FlipHorizontalDirection.Left) ? 90 : -90, 0, Easing.Linear);

            return animation;
        }
    }

    public class FlipVerticalAnimation : AnimationBase
    {
        public enum FlipVerticalDirection
        {
            Top,
            Bottom
        }

        public static readonly BindableProperty DirectionProperty =
            BindableProperty.Create(nameof(Direction), typeof(FlipVerticalDirection), typeof(FlipHorizontalAnimation), FlipVerticalDirection.Top,
                BindingMode.TwoWay, null);

        public FlipVerticalDirection Direction
        {
            get => (FlipVerticalDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        protected override uint DefaultDuration { get; set; } = 300;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.Animate("Flip", Flip(view), 16, Duration));

        internal Animation Flip(View view)
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => view.Opacity = f, 0.5, 1);
            animation.WithConcurrent((f) => view.RotationX = f, (Direction == FlipVerticalDirection.Top) ? 90 : -90, 0, Easing.Linear);

            return animation;
        }
    }
}
