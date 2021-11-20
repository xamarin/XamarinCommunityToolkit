using System.Linq;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public class IconTintColorEffect
	{
		public static readonly BindableProperty TintColorProperty
			= BindableProperty.CreateAttached("TintColor", typeof(Color), typeof(IconTintColorEffect), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnTintColorChanged);

		public static Color GetTintColor(BindableObject view)
			=> (Color)view.GetValue(TintColorProperty);

		public static void SetTintColor(BindableObject view, Color value)
			=> view.SetValue(TintColorProperty, value);

		static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is View view))
				return;

			var oldEffect = view.Effects.FirstOrDefault(e => e is IconTintColorEffectRouter);

			if (oldEffect != null)
				view.Effects.Remove(oldEffect);

			if (newValue == TintColorProperty.DefaultValue)
				return;

			view.Effects.Add(new IconTintColorEffectRouter());
		}
	}
}