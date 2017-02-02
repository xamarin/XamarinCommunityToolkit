using System.Linq;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    public static class BlurEffect
    {
        public static readonly BindableProperty BlurAmountProperty = BindableProperty.CreateAttached("BlurAmount", typeof(double), typeof(BlurEffect), 0.0, propertyChanged: OnBlurAmountChanged);

        private static void OnBlurAmountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
                return;

            double blurAmount = (double)newValue;
            var attachedEffect = view.Effects.FirstOrDefault(e => e is ViewBlurEffect);
            if (blurAmount > 0 && attachedEffect == null)
            {
                view.Effects.Add(new ViewBlurEffect());
            }
            else if (blurAmount == 0 && attachedEffect != null)
            {
                view.Effects.Remove(attachedEffect);
            }
        }

        public static double GetBlurAmount(BindableObject view)
        {
            return (double)view.GetValue(BlurAmountProperty);
        }

        public static void SetBlurAmount(BindableObject view, double amount)
        {
            view.SetValue(BlurAmountProperty, amount);
        }
    }

    public class ViewBlurEffect : RoutingEffect
    {
        public ViewBlurEffect() : base(EffectIds.ViewBlurEffect)
        {
        }
    }
}
