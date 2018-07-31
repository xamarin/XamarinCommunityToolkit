using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using PlatformEffects = Xamarin.Toolkit.Effects.UWP;
using RoutingEffects = Xamarin.Toolkit.Effects;
using Xamarin.Toolkit.Effects.Models;
using System;

[assembly: ExportEffect(typeof(PlatformEffects.LinearGradient), nameof(RoutingEffects.LinearGradientEffect))]
namespace Xamarin.Toolkit.Effects.UWP
{
    public class LinearGradient : PlatformEffect
    {
        VisualElement element;
        LinearGradientEffect effect;
        Models.LinearGradient linearGradient;

        protected override void OnAttached()
        {
            //Preparing
            element = Element as VisualElement;

            //The control applying the effect must be a visual element
            if (element is null)
                return;

            effect = element.Effects.FirstOrDefault() as RoutingEffects.LinearGradientEffect;
            if (effect is null)
                return;

            /*Linear gradient consists of:
            Start and end point
            LinearGradientStopCollection*/

            linearGradient = effect.LinearGradient;

            //In order to get the StartPoint and EndPoint, we must get the flow and then assign the StartPoint and EndPoint

            Windows.Foundation.Point startPoint, endPoint;

            SetPoints(out startPoint, out endPoint);

            //LinearGradientStopCollection
            var xamarinGradientStops = linearGradient.LinearGradientStops;
            GradientStopCollection gradientStops = SetGradientStops(xamarinGradientStops);

            //creating the linear gradient brush
            var linearGradientBrush = new LinearGradientBrush() { GradientStops = gradientStops, StartPoint = startPoint, EndPoint = endPoint };

            //Filling the control with the liear gradient

            //Now we have 3 stratigies
            //1- A control that inherits Background property from Control
            //2- A control that declares it's own Background property. eg. Border
            //3- A shape that doesnt have a Background property. Instead it has Fill

            if (Control != null)
            {
                var property = typeof(Control).GetProperty("Background");
                if (property != null) //Stratigy 1 or 2
                {
                    //stratigy 1: A control that inherits Background property from Control
                    try
                    {
                        ((Control)Control).Background = linearGradientBrush;
                    }
                    catch
                    {
                        //2- A control that declares it's own Background property. eg. Border
                        this.Control.GetType().GetProperty("Background").SetValue(Control, linearGradientBrush);
                    }
                }
                else //stratigy 3
                {
                    this.Control.GetType().GetProperty("Fill").SetValue(Control, linearGradientBrush);
                }
            }
        }

        private static GradientStopCollection SetGradientStops(LinearGradientStopCollection xamarinGradientStops)
        {
            return xamarinGradientStops.ToUwpGradientStops();
        }

        private void SetPoints(out Windows.Foundation.Point startPoint, out Windows.Foundation.Point endPoint)
        {
            startPoint = linearGradient.Flow.ToUwpGradientFlow()[0];
            endPoint = linearGradient.Flow.ToUwpGradientFlow()[1];
        }


        protected override void OnDetached()
        {

        }
    }

    public static class Utils
    {
        public static Windows.UI.Color ToUwpColor(this Color color) =>
            new Windows.UI.Color() { A = (Byte)(color.A * 255), R = (Byte)(color.R * 255), G = (Byte)(color.G * 255), B = (Byte)(color.B * 255) };
        public static Windows.Foundation.Point ToUwpPoint(this Point point) =>
            new Windows.Foundation.Point(point.X, point.Y);
        public static Windows.Foundation.Point[] ToUwpGradientFlow(this Flow flow)
        {
            var result = new Windows.Foundation.Point[2];
            switch (flow)
            {
                case Flow.BottomToTop:
                    result[0] = new Windows.Foundation.Point(0.5, 1);
                    result[1] = new Windows.Foundation.Point(0.5, 0);
                    break;
                case Flow.BottomLeftToTopRight:
                    result[0] = new Windows.Foundation.Point(0, 1);
                    result[1] = new Windows.Foundation.Point(1, 0);
                    break;
                case Flow.LeftToRight:
                    result[0] = new Windows.Foundation.Point(0, 0.5);
                    result[1] = new Windows.Foundation.Point(1, 0.5);
                    break;
                case Flow.TopLeftToBottomRight:
                    result[0] = new Windows.Foundation.Point(0, 0);
                    result[1] = new Windows.Foundation.Point(1, 1);
                    break;
                case Flow.TopToBottom:
                    result[0] = new Windows.Foundation.Point(0.5, 0);
                    result[1] = new Windows.Foundation.Point(0.5, 1);
                    break;
                case Flow.TopRightToBottomLeft:
                    result[0] = new Windows.Foundation.Point(1, 0);
                    result[1] = new Windows.Foundation.Point(0, 1);
                    break;
                case Flow.RightToLeft:
                    result[0] = new Windows.Foundation.Point(1, 0.5);
                    result[1] = new Windows.Foundation.Point(0, 0.5);
                    break;
                case Flow.BottomRightToTopLeft:
                    result[0] = new Windows.Foundation.Point(1, 1);
                    result[1] = new Windows.Foundation.Point(0, 0);
                    break;
                default:
                    break;
            }
            return result;
        }
        public static GradientStopCollection ToUwpGradientStops(this LinearGradientStopCollection xamarinLinearGradientStops)
        {
            var result = new GradientStopCollection();
            foreach (var linearGradientStop in xamarinLinearGradientStops)
                result.Add(new GradientStop() { Color = linearGradientStop.Color.ToUwpColor(), Offset = linearGradientStop.Offset });
            return result;
        }
    }
}
