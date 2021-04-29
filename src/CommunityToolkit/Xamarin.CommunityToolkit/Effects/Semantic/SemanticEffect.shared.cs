using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Effects.Semantic;

namespace Xamarin.CommunityToolkit.Effects
{
	public static class SemanticEffect
	{
		public static readonly BindableProperty HeadingLevelProperty =
		  BindableProperty.CreateAttached("HeadingLevel", typeof(bool), typeof(SemanticEffect), HeadingLevel.None, propertyChanged: OnHeadingLevelChanged);

		public static HeadingLevel GetHeadingLevel(BindableObject view) => (HeadingLevel)view.GetValue(HeadingLevelProperty);

		public static void SetHeadingLevel(BindableObject view, HeadingLevel value) => view.SetValue(HeadingLevelProperty, value);

		static void OnHeadingLevelChanged(BindableObject bindable, object oldValue, object newValue)
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
