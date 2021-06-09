using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.BackgroundAspectEffectRouter), nameof(BackgroundAspectEffectRouter))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class BackgroundAspectEffectRouter : PlatformEffect
	{
		UIImageView? imageView;

		CancellationTokenSource? cancellationTokenSource;

		public BackgroundAspectEffectRouter()
		{
			cancellationTokenSource = new CancellationTokenSource();
		}

		protected override void OnAttached()
			=> Task.Run(ApplyBackgroundImageAsync, cancellationTokenSource!.Token);

		protected override void OnDetached()
			=> ClearBackgroundImage();

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (!args.PropertyName.Equals(BackgroundAspectEffect.AspectProperty.PropertyName))
				return;

			cancellationTokenSource?.Cancel();

			Task.Run(ApplyBackgroundImageAsync, cancellationTokenSource!.Token);
		}

		async Task ApplyBackgroundImageAsync()
		{
			var contentPage = Element as ContentPage;

			if (Element == null || contentPage == null || contentPage.BackgroundImageSource == null)
				return;

			var image = await GetBackgroundImageFromSource(contentPage.BackgroundImageSource);

			if (image is null)
				return;

			var fallbackColor = contentPage.BackgroundColor.ToUIColor();

			Device.BeginInvokeOnMainThread(() => ApplyImageToView(image, fallbackColor));
		}

		void ApplyImageToView(UIImage image, UIColor color)
		{
			var effectParam = BackgroundAspectEffect.GetAspect(Element);

			imageView = CreateImageView(image, effectParam);

			Container.BackgroundColor = color;

			Container.InsertSubview(imageView, 0);

			ApplyConstraints(imageView, Container);
		}

		void ClearBackgroundImage()
		{
			if (imageView != null)
			{
				imageView.RemoveFromSuperview();

				imageView.Dispose();
			}

			if (cancellationTokenSource != null)
				DisposeCancellationToken();
		}

		void DisposeCancellationToken()
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
		}

		async Task<UIImage> GetBackgroundImageFromSource(ImageSource imageSource)
		{
			IImageSourceHandler handler;

			if (imageSource is FileImageSource)
			{
				handler = new FileImageSourceHandler();
			}
			else if (imageSource is StreamImageSource)
			{
				handler = new StreamImagesourceHandler();
			}
			else if (imageSource is UriImageSource)
			{
				handler = new ImageLoaderSourceHandler();
			}
			else
			{
				throw new NotSupportedException($"ImageSource is not supported by this effect: {imageSource.GetType().Name}");
			}

			return await handler.LoadImageAsync(imageSource, cancellationTokenSource!.Token)
				.ConfigureAwait(false);
		}

		UIImageView CreateImageView(UIImage uIImage, Aspect aspect)
		{
			var imageView = new UIImageView();
			imageView.Image = uIImage;
			imageView.ContentMode = GetContentModeForAspect(aspect);
			imageView.TranslatesAutoresizingMaskIntoConstraints = false;

			return imageView;
		}

		void ApplyConstraints(UIView child, UIView parent)
		{
			var constraints = new NSLayoutConstraint[]
			{
				child.TopAnchor.ConstraintEqualTo(parent.TopAnchor),
				child.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor),
				child.TrailingAnchor.ConstraintEqualTo(parent.TrailingAnchor),
				child.BottomAnchor.ConstraintEqualTo(parent.BottomAnchor)
			};

			NSLayoutConstraint.ActivateConstraints(constraints);
		}

		UIViewContentMode GetContentModeForAspect(Aspect aspect)
		{
			switch (aspect)
			{
				default:
				case Aspect.Fill:
					return UIViewContentMode.ScaleToFill;

				case Aspect.AspectFill:
					return UIViewContentMode.ScaleAspectFill;

				case Aspect.AspectFit:
					return UIViewContentMode.ScaleAspectFit;
			}
		}
	}
}
