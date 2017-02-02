using System.Linq;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    public static class ChangePickerColorEffect
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached ("Color", typeof (Color?), typeof (ChangePickerColorEffect), Color.Transparent, propertyChanged: OnColorChanged);
  //      public static readonly BindableProperty TrueColorProperty = BindableProperty.CreateAttached ("TrueColor", typeof (Color?), typeof (ChangeColorEffect), Color.Transparent, propertyChanged: OnColorChanged);

        private static void OnColorChanged (BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as Picker;
            if (control == null)
                return;

            var color = (Color)newValue;

            var attachedEffect = control.Effects.FirstOrDefault (e => e is ChangeColorPickerEffect);
            if (color != Color.Transparent && attachedEffect == null) {
                control.Effects.Add (new ChangeColorPickerEffect ());
            } else if (color == Color.Transparent && attachedEffect != null) {
                control.Effects.Remove (attachedEffect);
            }
        }

        public static Color GetColor (BindableObject view)
        {
            return (Color)view.GetValue (ColorProperty);
        }

        public static void SetColor (BindableObject view, string color)
        {
            view.SetValue (ColorProperty, color);
        }

    }

    public class ChangeColorPickerEffect : RoutingEffect
    {
        public ChangeColorPickerEffect() : base(EffectIds.ChangeColorPickerEffect)
        {
        }
    }
}
