using System;
using System.ComponentModel;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.CommunityToolkit.Effects;
using Effects = Xamarin.CommunityToolkit.UWP.Effects;

[assembly: Xamarin.Forms.ExportEffect(typeof(Effects.IconTintColorEffectRouter), nameof(IconTintColorEffectRouter))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class IconTintColorEffectRouter : PlatformEffect
	{
		private SpriteVisual? spriteVisual;
		private Vector2? originalImageSize;

		protected override void OnAttached() => ApplyTintColor();

		protected override void OnDetached()
		{
			switch (Control)
			{
				case Windows.UI.Xaml.Controls.Image image:
					image.SizeChanged -= OnInitializedImageSize;
					RestoreOriginalImageSize(image);
					break;
				case Windows.UI.Xaml.Controls.Button button:
					button.SizeChanged -= OnInitializedButtonImageSize;
					break;
			}

			RemoveTintColor();
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (!args.PropertyName.Equals(Image.SourceProperty.PropertyName) &&
			    !args.PropertyName.Equals(ImageButton.SourceProperty.PropertyName))
				return;

			ApplyTintColor();
		}

		private void ApplyTintColor()
		{
			if (Element == null || Control == null)
				return;

			switch (Control)
			{
				case Windows.UI.Xaml.Controls.Image image:
				{
					if (image.IsLoaded)
						ApplyImageTintColor(image);
					else
						WaitUntilImageSizeIsInitialized(image, OnInitializedImageSize);
					break;
				}
				case Windows.UI.Xaml.Controls.Button button:
				{
					var image = TryGetButtonImage(button);
					if (image == null)
						return;

					if (image.IsLoaded)
						ApplyButtonImageTintColor(button);
					else
						WaitUntilImageSizeIsInitialized(image, OnInitializedButtonImageSize);
					break;
				}
			}
		}

		private static void WaitUntilImageSizeIsInitialized(Windows.UI.Xaml.Controls.Image image, SizeChangedEventHandler callback)
		{
			image.SizeChanged += callback;
		}

		private void OnInitializedButtonImageSize(object sender, SizeChangedEventArgs e)
		{
			var button = GetButtonControl();
			button.SizeChanged -= OnInitializedButtonImageSize;

			ApplyButtonImageTintColor(button);
		}

		private void OnInitializedImageSize(object sender, SizeChangedEventArgs e)
		{
			var image = GetImageControl();
			image.SizeChanged -= OnInitializedImageSize;

			ApplyImageTintColor(image);
		}

		private void ApplyButtonImageTintColor(Windows.UI.Xaml.Controls.Button button)
		{
			var image = TryGetButtonImage(button);
			if (image == null)
				return;

			var offset = image.ActualOffset;
			var width = (float)image.ActualWidth;
			var height = (float)image.ActualHeight;

			var uri = TryGetSourceImageUri(image, Element as IImageElement);
			if (uri == null)
				return;

			// Hide possible visible pixels from original image
			image.Visibility = Visibility.Collapsed;

			ApplyTintCompositionEffect(width, height, offset, uri);
		}

		private void ApplyImageTintColor(Windows.UI.Xaml.Controls.Image image)
		{
			var uri = TryGetSourceImageUri(image, Element as IImageElement);
			if (uri == null)
				return;

			originalImageSize = GetTintImageSize(image);
			var width = originalImageSize.Value.X;
			var height = originalImageSize.Value.Y;

			// Hide possible visible pixels from original image.
			// Workaround as it's not possible to hide parents without also hiding children. Workaround requires position adjustment of tinted image.
			image.Width = image.Height = 0;
			// Offset to re-center tinted image
			var offset = new Vector3(-width * .5f, -height * .5f, 0f);

			ApplyTintCompositionEffect(width, height, offset, uri);
		}

		private Vector2 GetTintImageSize(Windows.UI.Xaml.Controls.Image image)
		{
			// ActualSize is set by the renderer when loaded. Without the zero size workaround, it's usually always what we want (default). 
			if (image.ActualSize != Vector2.Zero)
				return image.ActualSize;

			// (Fallback 1) Required when the Source property changes, because the size has been set to zero to hide the original image.
			if (originalImageSize.HasValue)
				return originalImageSize.Value;

			// (Fallback 2) Required when previous effect was removed and image was hidden using zero size workaround.
			// The image size is restored in Width/Height during OnDetach,
			// however the values are not reflected in the "ActualSize", therefore this extra fallback is required.
			return new Vector2((float)image.Width, (float)image.Height);
		}

		private void ApplyTintCompositionEffect(float width, float height, Vector3 offset, Uri surfaceMaskUri)
		{
			var color = IconTintColorEffect.GetTintColor(Element);

			var compositor = ElementCompositionPreview.GetElementVisual(Control).Compositor;

			var sourceColorBrush = compositor.CreateColorBrush();
			sourceColorBrush.Color = color.ToWindowsColor();

			var loadedSurfaceMask = LoadedImageSurface.StartLoadFromUri(surfaceMaskUri);

			var maskBrush = compositor.CreateMaskBrush();
			maskBrush.Source = sourceColorBrush;
			maskBrush.Mask = compositor.CreateSurfaceBrush(loadedSurfaceMask);

			spriteVisual = compositor.CreateSpriteVisual();
			spriteVisual.Brush = maskBrush;
			spriteVisual.Size = new Vector2(width, height);
			spriteVisual.AnchorPoint = Vector2.Zero;
			spriteVisual.CenterPoint = new Vector3(width * .5f, height * .5f, 0f);
			spriteVisual.Offset = offset;
			spriteVisual.BorderMode = CompositionBorderMode.Hard;
			// Image is loaded flipped
			spriteVisual.Scale = new Vector3(-1, 1, 1);

			ElementCompositionPreview.SetElementChildVisual(Control, spriteVisual);
		}

		private void RemoveTintColor()
		{
			if (spriteVisual == null)
				return;

			spriteVisual.Brush = null;
			ElementCompositionPreview.SetElementChildVisual(Control, null);
		}

		private void RestoreOriginalImageSize(Windows.UI.Xaml.Controls.Image image)
		{
			if (!originalImageSize.HasValue)
				return;

			image.Width = originalImageSize.Value.X;
			image.Height = originalImageSize.Value.Y;
		}

		private static Uri? TryGetSourceImageUri(Windows.UI.Xaml.Controls.Image? imageControl, IImageElement? imageElement)
		{
			if (imageElement?.Source is UriImageSource uriImageSource)
				return uriImageSource.Uri;

			if (imageControl?.Source is BitmapImage bitmapImage)
				return bitmapImage.UriSource;

			return null;
		}

		private Windows.UI.Xaml.Controls.Button GetButtonControl()
		{
			return (Windows.UI.Xaml.Controls.Button)Control;
		}

		private Windows.UI.Xaml.Controls.Image GetImageControl()
		{
			return (Windows.UI.Xaml.Controls.Image)Control;
		}

		private static Windows.UI.Xaml.Controls.Image? TryGetButtonImage(Windows.UI.Xaml.Controls.Button button)
		{
			return button.Content as Windows.UI.Xaml.Controls.Image;
		}
	}
}