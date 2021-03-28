using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
    public class TranslateAnimation : AnimationBase
    {
        public static readonly BindableProperty TranslateXProperty =
            BindableProperty.Create(nameof(TranslateX), typeof(double), typeof(TranslateAnimation), default(double),
                BindingMode.TwoWay, null);

        public double TranslateX
        {
            get => (double)GetValue(TranslateXProperty);
            set => SetValue(TranslateXProperty, value);
        }

        public static readonly BindableProperty TranslateYProperty =
            BindableProperty.Create(nameof(TranslateY), typeof(double), typeof(TranslateAnimation), default(double),
                BindingMode.TwoWay, null);

        public double TranslateY
        {
            get => (double)GetValue(TranslateYProperty);
            set => SetValue(TranslateYProperty, value);
        }

        protected override uint DefaultDuration { get; set; } = 50;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.TranslateTo(TranslateX, TranslateY, Duration, Easing));
    }
}
