using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Xamarin.CommunityToolkit.Effects
{
	public class CornerRadiusEffect : NullEffect
	{
		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.CreateAttached("CornerRadius", typeof(Forms.CornerRadius), typeof(CornerRadiusEffect),
				default(Forms.CornerRadius), propertyChanged: TryAttachEffect);

		public static Forms.CornerRadius GetCornerRadius(BindableObject bindable)
			=> (CornerRadius) bindable.GetValue(CornerRadiusProperty);

		public static void SetCornerRadius(BindableObject bindable, CornerRadius value)
			=> bindable.SetValue(CornerRadiusProperty, value);

		static void TryAttachEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is VisualElement element))
				return;

			var cornerRadiusEffects = element.Effects.OfType<CornerRadiusEffect>();

			if (GetCornerRadius(bindable) == default)
			{
				foreach (var cornerRadiusEffect in cornerRadiusEffects)
				{
					element.Effects.Remove(cornerRadiusEffect);
				}

				return;
			}

			if (!cornerRadiusEffects.Any())
				element.Effects.Add(new CornerRadiusEffect());
		}

		protected override void OnAttached()
		{
			if (Element is VisualElement elementView)
			{
				elementView.SizeChanged += OnElementSizeChanged;
				elementView.PropertyChanged += OnElementPropertyChanged;
				CreateClip(elementView);
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

		void OnElementSizeChanged(object? sender, EventArgs e) => UpdateSize();

		void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CornerRadiusProperty.PropertyName)
			{
				UpdateCornerRadius();
			}
		}

		void UpdateCornerRadius()
		{
			if (Element is not VisualElement elementView)
				return;

			if (elementView.Clip is RoundRectangleGeometry roundRectangleGeometry)
			{
				var cornerRadius = GetCornerRadius(Element);
				roundRectangleGeometry.CornerRadius = cornerRadius;
			}
			else
			{
				CreateClip(elementView);
			}
		}

		void UpdateSize()
		{
			if (Element is not VisualElement elementView)
				return;

			if (elementView.Clip is RoundRectangleGeometry roundRectangleGeometry)
			{
				var rect = new Rect(0, 0, elementView.Width, elementView.Height);
				roundRectangleGeometry.Rect = rect;
			}
			else
			{
				CreateClip(elementView);
			}
		}

		static void CreateClip(VisualElement elementView)
		{
			var cornerRadius = GetCornerRadius(elementView);
			var rect = new Rect(0, 0, elementView.Width, elementView.Height);
			elementView.Clip = new RoundRectangleGeometry(cornerRadius, rect);
		}
	}
}
