using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class BackgroundAspectEffect
	{
		public static readonly BindableProperty AspectProperty =
			BindableProperty.CreateAttached("Aspect", typeof(Aspect?), typeof(BackgroundAspectEffect), null,
				propertyChanged: OnAspectChanged);

		public static Aspect GetAspect(BindableObject view)
			=> (Aspect)view.GetValue(AspectProperty);

		public static void SetAspect(BindableObject view, Aspect value)
			=> view.SetValue(AspectProperty, value);

		static void OnAspectChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is ContentPage contentPage))
				return;

			var oldEffect = contentPage.Effects.FirstOrDefault(e => e is BackgroundAspectEffectRouter);

			if (oldEffect != null)
				contentPage.Effects.Remove(oldEffect);

			if (!((Aspect?)newValue).HasValue)
				return;

			contentPage.Effects.Add(new BackgroundAspectEffectRouter());
		}
	}
}
