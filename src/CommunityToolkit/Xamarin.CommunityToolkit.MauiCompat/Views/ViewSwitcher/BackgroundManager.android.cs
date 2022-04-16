using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using AView = Android.Views.View;

// Copied from Xamarin.Forms (BackgroundManager)
namespace Xamarin.CommunityToolkit.UI.Views
{
	static class BackgroundManager
	{
		public static void Init(IVisualElementRenderer renderer)
		{
			_ = renderer ?? throw new ArgumentNullException($"{nameof(BackgroundManager)}.{nameof(Init)} {nameof(renderer)} cannot be null");

			renderer.ElementPropertyChanged += OnElementPropertyChanged;
			renderer.ElementChanged += OnElementChanged;
		}

		public static void Dispose(IVisualElementRenderer renderer)
		{
			_ = renderer ?? throw new ArgumentNullException($"{nameof(BackgroundManager)}.{nameof(Init)} {nameof(renderer)} cannot be null");

			renderer.ElementPropertyChanged -= OnElementPropertyChanged;
			renderer.ElementChanged -= OnElementChanged;

			if (renderer.Element != null)
			{
				renderer.Element.PropertyChanged -= OnElementPropertyChanged;
			}
		}

		static void UpdateBackgroundColor(AView? control, VisualElement? element, Color? color = null)
		{
			if (element == null || control == null)
				return;

			var finalColor = color ?? element.BackgroundColor;
			if (finalColor.IsDefault())
				control.SetBackground(null);
			else
				control.SetBackgroundColor(finalColor.ToAndroid());
		}

		static void UpdateBackground(AView? control, VisualElement? element)
		{
			if (element == null || control == null)
				return;

			var background = element.Background;

			control.UpdateBackground(background);
		}

		static void OnElementChanged(object sender, VisualElementChangedEventArgs e)
		{
			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
			}

			if (e.NewElement != null)
			{
				var renderer = sender as IVisualElementRenderer;
				e.NewElement.PropertyChanged += OnElementPropertyChanged;
				UpdateBackgroundColor(renderer?.View, renderer?.Element);
				UpdateBackground(renderer?.View, renderer?.Element);
			}
		}

		static void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			var renderer = sender as IVisualElementRenderer;

			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
				UpdateBackgroundColor(renderer?.View, renderer?.Element);
			else if (e.PropertyName == VisualElement.BackgroundProperty.PropertyName)
				UpdateBackground(renderer?.View, renderer?.Element);
		}
	}
}