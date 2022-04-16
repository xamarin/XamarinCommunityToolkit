using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Effects.Semantic;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public static class SemanticEffect
	{
		public static readonly BindableProperty HeadingLevelProperty =
		  BindableProperty.CreateAttached("HeadingLevel", typeof(HeadingLevel), typeof(SemanticEffect), HeadingLevel.None, propertyChanged: OnPropertyChanged);

		public static HeadingLevel GetHeadingLevel(BindableObject view) => (HeadingLevel)view.GetValue(HeadingLevelProperty);

		public static void SetHeadingLevel(BindableObject view, HeadingLevel value) => view.SetValue(HeadingLevelProperty, value);

		public static readonly BindableProperty SemanticInclusionProperty =
			BindableProperty.CreateAttached("SemanticInclusion", typeof(SemanticInclusion), typeof(SemanticEffect), SemanticInclusion.Default, propertyChanged: OnPropertyChanged);

		public static SemanticInclusion GetSemanticInclusion(BindableObject view) => (SemanticInclusion)view.GetValue(SemanticInclusionProperty);

		public static void SetSemanticInclusion(BindableObject view, SemanticInclusion value) => view.SetValue(SemanticInclusionProperty, value);

		public static readonly BindableProperty DescriptionProperty = BindableProperty.CreateAttached("Description", typeof(string), typeof(SemanticEffect), default(string), propertyChanged: OnPropertyChanged);

		public static string GetDescription(BindableObject bindable) => (string)bindable.GetValue(DescriptionProperty);

		public static void SetDescription(BindableObject bindable, string value) => bindable.SetValue(DescriptionProperty, value);

		public static readonly BindableProperty HintProperty = BindableProperty.CreateAttached("Hint", typeof(string), typeof(SemanticEffect), default(string), propertyChanged: OnPropertyChanged);

		public static string GetHint(BindableObject bindable) => (string)bindable.GetValue(HintProperty);

		public static void SetHint(BindableObject bindable, string value) => bindable.SetValue(HintProperty, value);

		static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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