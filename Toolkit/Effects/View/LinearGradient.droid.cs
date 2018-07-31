using System.Linq;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;
using Xamarin.Toolkit.Effects.Models;

[assembly: ExportEffect(typeof(PlatformEffects.LinearGradient), nameof(RoutingEffects.LinearGradientEffect))]
namespace Xamarin.Toolkit.Effects.Droid
{
    public class LinearGradient : PlatformEffect
    {
        VisualElement element;
        LinearGradientEffect effect;
        Models.LinearGradient linearGradient;
        public LinearGradient() { }

        protected override void OnAttached()
        {
            //Preparing
            element = Element as VisualElement;

            //The control applying the effect must be a visual element
            if (element is null)
                return;

            effect = element.Effects.FirstOrDefault() as LinearGradientEffect;
            if (effect is null)
                return;

            linearGradient = effect.LinearGradient;

            /*GradientDrawable consists of:
            Flow
            Color*/

            //Flow
            var flow = linearGradient.Flow.ToGradientDrawableFlow();


            //Colors
            var nativeColors = linearGradient.LinearGradientStops.Select(gs => gs.Color).ToArray();

            int[] colors = new int[nativeColors.Length];

            for (int i = 0; i < nativeColors.Length; i++)
                colors[i] = nativeColors[i].ToAndroid();

            //Definging the gradientDrawable
            GradientDrawable gradientDrawable = new GradientDrawable(flow, colors);

            if (Control != null)
                Control.SetBackground(gradientDrawable);
        }

        protected override void OnDetached()
        {

        }
    }
    public static class Utils
    {
        public static GradientDrawable.Orientation ToGradientDrawableFlow(this Flow flow)
        {
            GradientDrawable.Orientation orientation;
            switch (flow)
            {
                case Flow.BottomToTop:
                    orientation = GradientDrawable.Orientation.BottomTop;
                    break;
                case Flow.BottomLeftToTopRight:
                    orientation = GradientDrawable.Orientation.BlTr;
                    break;
                case Flow.LeftToRight:
                    orientation = GradientDrawable.Orientation.LeftRight;
                    break;
                case Flow.TopLeftToBottomRight:
                    orientation = GradientDrawable.Orientation.TlBr;
                    break;
                case Flow.TopToBottom:
                    orientation = GradientDrawable.Orientation.TopBottom;
                    break;
                case Flow.TopRightToBottomLeft:
                    orientation = GradientDrawable.Orientation.TrBl;
                    break;
                case Flow.RightToLeft:
                    orientation = GradientDrawable.Orientation.RightLeft;
                    break;
                case Flow.BottomRightToTopLeft:
                    orientation = GradientDrawable.Orientation.BrTl;
                    break;
                default:
                    orientation = null;
                    break;
            }
            return orientation;
        }
    }
}
