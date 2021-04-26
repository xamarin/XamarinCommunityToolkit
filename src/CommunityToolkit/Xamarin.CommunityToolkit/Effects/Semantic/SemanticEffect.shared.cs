using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public static class SemanticEffect
	{
		public static readonly BindableProperty IsHeadingProperty =
		  BindableProperty.CreateAttached("IsHeading", typeof(bool), typeof(SemanticEffect), false, propertyChanged: OnIsHeadingChanged);

		public static bool GetIsHeading(BindableObject view) => (bool)view.GetValue(IsHeadingProperty);

		public static void SetIsHeading(BindableObject view, bool value) => view.SetValue(IsHeadingProperty, value);

		static void OnIsHeadingChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not View view)
				return;

			if (view.Effects.FirstOrDefault(x => x is SemanticEffectRouter) == null)
			{
				view.Effects.Add(new SemanticEffectRouter());
			}
		}
	}
}
