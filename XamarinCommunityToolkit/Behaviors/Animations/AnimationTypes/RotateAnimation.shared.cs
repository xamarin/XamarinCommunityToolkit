using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public class RotateAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
           BindableProperty.Create(nameof(Rotation), typeof(double), typeof(AnimationBase), 180.0, BindingMode.TwoWay);

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public RotateAnimation()
        {
            Duration = 200;
        }

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);
            await view.RotateTo(Rotation, Duration, easing);
            view.Rotation = 0;
        }
    }
}
