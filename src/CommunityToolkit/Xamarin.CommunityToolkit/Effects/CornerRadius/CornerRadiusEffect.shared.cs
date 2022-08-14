using System;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Xamarin.CommunityToolkit.Effects
{
	public class CornerRadiusEffect
	{
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached(
			nameof(CornerRadius),
			typeof(CornerRadius),
			typeof(CornerRadiusEffect),
			default(CornerRadius),
			propertyChanged: OnCornerRadiusPropertyChanged);

		static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not VisualElement elementView)
				return;

			elementView.SizeChanged -= ElementViewSizeChanged;
			elementView.SizeChanged += ElementViewSizeChanged;

			UpdateClip(elementView);
		}

		static void ElementViewSizeChanged(object? sender, EventArgs e)
		{
			if (sender == null)
				return;

			UpdateClip((VisualElement)sender);
		}

		public static CornerRadius GetCornerRadius(BindableObject? bindable)
			=> (CornerRadius)(bindable?.GetValue(CornerRadiusProperty) ?? throw new ArgumentNullException(nameof(bindable)));

		public static void SetCornerRadius(BindableObject? bindable, CornerRadius value)
			=> bindable?.SetValue(CornerRadiusProperty, value);

		static void UpdateClip(VisualElement elementView)
		{
			var rect = new Rect(0, 0, elementView.Width, elementView.Height);
			var cornerRadius = GetCornerRadius(rect, elementView);
			if (cornerRadius == default)
			{
				elementView.Clip = null;
				return;
			}

			if (elementView.Clip is not RoundRectangleGeometry roundRectangleGeometry)
			{
				elementView.Clip = new RoundRectangleGeometry(cornerRadius, rect);
				return;
			}

			roundRectangleGeometry.CornerRadius = cornerRadius;
			roundRectangleGeometry.Rect = rect;
		}

		static CornerRadius GetCornerRadius(Rect rect, VisualElement elementView)
		{
			var maxCornerRadius = Math.Min(rect.Width, rect.Height) / 2;
			if (maxCornerRadius <= 0)
				return default;

			var cornerRadius = GetCornerRadius(elementView);
			if (cornerRadius.TopLeft > maxCornerRadius ||
				cornerRadius.TopRight > maxCornerRadius ||
				cornerRadius.BottomLeft > maxCornerRadius ||
				cornerRadius.BottomRight > maxCornerRadius)
			{
				return new CornerRadius(
					Math.Min(cornerRadius.TopLeft, maxCornerRadius),
					Math.Min(cornerRadius.TopRight, maxCornerRadius),
					Math.Min(cornerRadius.BottomLeft, maxCornerRadius),
					Math.Min(cornerRadius.BottomRight, maxCornerRadius));
			}

			return cornerRadius;
		}
	}
}