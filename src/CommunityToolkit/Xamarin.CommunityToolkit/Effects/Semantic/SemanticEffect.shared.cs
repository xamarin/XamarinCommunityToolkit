﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Effects.Semantic;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public static class SemanticEffect
	{
		public static readonly BindableProperty HeadingLevelProperty =
		  BindableProperty.CreateAttached("HeadingLevel", typeof(HeadingLevel), typeof(SemanticEffect), HeadingLevel.None, propertyChanged: OnPropertyChanged);

		public static HeadingLevel GetHeadingLevel(BindableObject view) => (HeadingLevel)view.GetValue(HeadingLevelProperty);

		public static void SetHeadingLevel(BindableObject view, HeadingLevel value) => view.SetValue(HeadingLevelProperty, value);

		public static readonly BindableProperty A11yVisibilityProperty =
			BindableProperty.CreateAttached("A11yVisibility", typeof(ImportantForA11y), typeof(SemanticEffect), ImportantForA11y.Auto, propertyChanged: OnPropertyChanged);

		public static ImportantForA11y GetA11yVisibility(BindableObject view) => (ImportantForA11y)view.GetValue(A11yVisibilityProperty);

		public static void SetA11yVisibility(BindableObject view, ImportantForA11y value) => view.SetValue(A11yVisibilityProperty, value);

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