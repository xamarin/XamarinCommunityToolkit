using System.Linq;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    public static class ChangeColorEffect
    {
        public static readonly BindableProperty FalseColorProperty = BindableProperty.CreateAttached ("FalseColor", typeof (Color?), typeof (ChangeColorEffect), Color.Transparent, propertyChanged: OnColorChanged);
        public static readonly BindableProperty TrueColorProperty = BindableProperty.CreateAttached ("TrueColor", typeof (Color?), typeof (ChangeColorEffect), Color.Transparent, propertyChanged: OnColorChanged);

        private static void OnColorChanged (BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as Switch;
            if (control == null)
                return;

            var color = (Color)newValue;

            var attachedEffect = control.Effects.FirstOrDefault (e => e is ChangeColorSwitchEffect);
            if (color != Color.Transparent && attachedEffect == null) {
                control.Effects.Add (new ChangeColorSwitchEffect ());
            } else if (color == Color.Transparent && attachedEffect != null) {
                control.Effects.Remove (attachedEffect);
            }
        }

        public static Color GetFalseColor (BindableObject view)
        {
            return (Color)view.GetValue (FalseColorProperty);
        }

        public static void SetFalseColor (BindableObject view, string color)
        {
            view.SetValue (FalseColorProperty, color);
        }

        public static Color GetTrueColor (BindableObject view)
        {
            return (Color)view.GetValue (TrueColorProperty);
        }

        public static void SetTrueColor (BindableObject view, Color color)
        {
            view.SetValue (TrueColorProperty, color);
        }

    }

    public class ChangeColorSwitchEffect : RoutingEffect
    {
        public ChangeColorSwitchEffect () : base ("FormsCommunityToolkit.Effects.ChangeColorSwitchEffect")
        {
        }
    }
}
