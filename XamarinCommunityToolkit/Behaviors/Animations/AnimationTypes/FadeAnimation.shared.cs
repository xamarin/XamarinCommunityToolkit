using System.Threading.Tasks;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class FadeAnimation : AnimationBase
    {
        public static readonly BindableProperty FadeProperty =
           BindableProperty.Create(nameof(Fade), typeof(double), typeof(AnimationBase), 0.3, BindingMode.TwoWay);

        public double Fade
        {
            get => (double)GetValue(FadeProperty); 
            set => SetValue(FadeProperty, value); 
        }

        protected override uint DefaultDuration { get; set; } = 300;

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);
            await view.FadeTo(Fade, Duration, easing);
            await view.FadeTo(1, Duration, easing);
        }
    }
}
