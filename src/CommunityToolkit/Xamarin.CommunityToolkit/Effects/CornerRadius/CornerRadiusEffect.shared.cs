using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Xamarin.CommunityToolkit.Effects
{
	public class CornerRadiusEffect : NullEffect
	{
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached(
			nameof(CornerRadius),
			typeof(CornerRadius),
			typeof(CornerRadiusEffect),
			default(CornerRadius),
			propertyChanged: TryGenerateEffect);

		public static CornerRadius GetCornerRadius(BindableObject? bindable)
			=> (CornerRadius)(bindable?.GetValue(CornerRadiusProperty) ?? throw new ArgumentNullException(nameof(bindable)));

		public static void SetCornerRadius(BindableObject? bindable, CornerRadius value)
			=> bindable?.SetValue(CornerRadiusProperty, value);

		static void TryGenerateEffect(BindableObject? bindable, object oldValue, object newValue)
		{
			if (bindable is not VisualElement elementView)
				return;

			var cornerRadiusEffects = elementView.Effects.OfType<CornerRadiusEffect>();

			if (GetCornerRadius(bindable) == default)
			{
				foreach (var cornerRadiusEffect in cornerRadiusEffects.ToArray())
					elementView.Effects.Remove(cornerRadiusEffect);

				return;
			}

			if (!cornerRadiusEffects.Any())
				elementView.Effects.Add(new CornerRadiusEffect());
		}

		protected override void OnAttached()
		{
			if (Element is VisualElement elementView)
			{
				elementView.SizeChanged += OnElementSizeChanged;
				elementView.PropertyChanged += OnElementPropertyChanged;
				UpdateClip();
			}
		}

		protected override void OnDetached()
		{
			if (Element is VisualElement elementView)
			{
				elementView.SizeChanged -= OnElementSizeChanged;
				elementView.PropertyChanged -= OnElementPropertyChanged;
				elementView.Clip = null;
			}
		}

		void OnElementSizeChanged(object? sender, EventArgs e) => UpdateClip();

		void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CornerRadiusProperty.PropertyName)
			{
				UpdateClip();
			}
		}

		void UpdateClip()
		{
			if (Element is not VisualElement elementView)
				return;

			var rect = new Rect(0, 0, elementView.Width, elementView.Height);
			var cornerRadius = GetCornerRadius(rect);
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

		CornerRadius GetCornerRadius(Rect rect)
        {
	        var maxCornerRadius = Math.Min(rect.Width, rect.Height) / 2;
	        if (maxCornerRadius <= 0)
		        return default;

	        var cornerRadius = GetCornerRadius(Element);
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
