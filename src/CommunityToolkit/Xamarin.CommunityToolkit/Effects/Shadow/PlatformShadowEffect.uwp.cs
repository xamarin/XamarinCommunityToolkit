using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Grid = Xamarin.Forms.Grid;
using Image = Windows.UI.Xaml.Controls.Image;

[assembly: ExportEffect(typeof(PlatformShadowEffect), nameof(ShadowEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformShadowEffect : PlatformEffect
	{
		enum ShadowEffectState
		{
			Initialized,
			PanelCreated,
			Attached
		}

		const float defaultRadius = 10f;

		const float defaultOpacity = 1f;

		ShadowEffectState state;

		SpriteVisual? spriteVisual;

		Layout<View>? shadowPanel;

		DropShadow? shadow;

		FrameworkElement? View => Control ?? Container;

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			switch (args.PropertyName)
			{
				case ShadowEffect.ColorPropertyName:
				case ShadowEffect.OpacityPropertyName:
				case ShadowEffect.RadiusPropertyName:
				case ShadowEffect.OffsetXPropertyName:
				case ShadowEffect.OffsetYPropertyName:
				case nameof(VisualElement.Width):
				case nameof(VisualElement.Height):
				case nameof(VisualElement.BackgroundColor):
					UpdateShadow();
					break;
			}
		}

		protected override void OnAttached()
		{
			if (Element is not View elementView)
				return;

			switch (state)
			{
				case ShadowEffectState.Initialized:
					shadowPanel = new StackLayout()
					{
						Children = { new Grid() }
					};

					state = ShadowEffectState.PanelCreated;
					MoveElementTo(elementView, shadowPanel);
					break;
				case ShadowEffectState.PanelCreated:
					AppendShadow();
					state = ShadowEffectState.Attached;
					break;
				default:
					break;
			}
		}

		protected override void OnDetached()
		{
			if (state != ShadowEffectState.Attached)
				return;

			if (View != null)
			{
				View.SizeChanged -= ViewSizeChanged;
			}

			shadow?.Dispose();
			shadow = null;
			spriteVisual?.Dispose();
			spriteVisual = null;

			state = ShadowEffectState.PanelCreated;
		}

		void AppendShadow()
		{
			if (View == null)
				return;

			var view = ElementCompositionPreview.GetElementVisual(View);

			if (view == null)
				return;

			var compositor = view.Compositor;
			shadow ??= compositor.CreateDropShadow();
			UpdateShadow();
			spriteVisual = compositor.CreateSpriteVisual();
			spriteVisual.Shadow = shadow;
#if UWP_18362
			spriteVisual.Size = View.ActualSize;
#else
			spriteVisual.Size = new Vector2(Convert.ToSingle(View.ActualWidth), Convert.ToSingle(View.ActualHeight));
#endif

			View.SizeChanged += ViewSizeChanged;

			var renderer = shadowPanel?.Children.First().GetOrCreateRenderer();
			spriteVisual.ParentForTransform = ElementCompositionPreview.GetElementVisual(View);
			ElementCompositionPreview.SetElementChildVisual(renderer?.ContainerElement, spriteVisual);
		}

		void MoveElementTo(View element, Layout<View> to)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (Element.Parent is Layout<View> layout)
				{
					var index = layout.Children.IndexOf(element);
					layout.Children.Insert(index, to);

					if (layout is Grid)
					{
						var row = Grid.GetRow(element);
						var rowSpan = Grid.GetRowSpan(element);
						var column = Grid.GetColumn(element);
						var columnSpan = Grid.GetColumnSpan(element);

						Grid.SetRow(to, row);
						Grid.SetRowSpan(to, rowSpan);
						Grid.SetColumn(to, column);
						Grid.SetColumnSpan(to, columnSpan);
					}
				}
				else if (Element.Parent is ScrollView scrollView)
				{
					scrollView.Content = to;
				}
				else if (Element.Parent is ContentView contentView)
				{
					contentView.Content = to;
				}

				to.Children.Add(element);
			});
		}

		void UpdateShadow()
		{
			if (shadow == null)
				return;

			var radius = (float)ShadowEffect.GetRadius(Element);
			var opacity = (float)ShadowEffect.GetOpacity(Element);
			var color = ShadowEffect.GetColor(Element).ToWindowsColor();
			var offsetX = (float)ShadowEffect.GetOffsetX(Element);
			var offsetY = (float)ShadowEffect.GetOffsetY(Element);

			shadow.Color = color;
			shadow.BlurRadius = radius < 0 ? defaultRadius : radius;
			shadow.Opacity = opacity < 0 ? defaultOpacity : opacity;
			shadow.Offset = new Vector3(offsetX, offsetY, 0);

			UpdateShadowMask();
		}

		void UpdateShadowMask()
		{
			if (shadow == null)
				return;

			shadow.Mask = View switch
			{
				TextBlock textBlock => textBlock.GetAlphaMask(),
				Shape shape => shape.GetAlphaMask(),
				Image image => image.GetAlphaMask(),
				_ => shadow.Mask
			};
		}

		void ViewSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (spriteVisual == null || View == null)
				return;
#if UWP_18362
			spriteVisual.Size = View.ActualSize;
#else
			spriteVisual.Size = new Vector2(Convert.ToSingle(View.ActualWidth), Convert.ToSingle(View.ActualHeight));
#endif

			UpdateShadowMask();
		}
	}
}