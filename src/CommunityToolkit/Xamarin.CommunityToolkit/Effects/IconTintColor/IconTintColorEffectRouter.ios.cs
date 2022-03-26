using System;
using System.ComponentModel;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.IconTintColorEffectRouter), nameof(IconTintColorEffectRouter))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class IconTintColorEffectRouter : PlatformEffect
	{
		protected override void OnAttached()
			=> ApplyTintColor();

		protected override void OnDetached()
			=> ClearTintColor();

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (!args.PropertyName.Equals(IconTintColorEffect.TintColorProperty.PropertyName) &&
			    !args.PropertyName.Equals(Image.SourceProperty.PropertyName) &&
			    !args.PropertyName.Equals(ImageButton.SourceProperty.PropertyName))
				return;

			ApplyTintColor();
		}

		void ApplyTintColor()
		{
			if (Control == null || Element == null || !(Element is VisualElement))
				return;

			var color = IconTintColorEffect.GetTintColor(Element);

			switch (Control)
			{
				case UIImageView imageView:
					SetUIImageViewTintColor(imageView, color);
					break;
				case UIButton button:
					SetUIButtonTintColor(button, color);
					break;
			}
		}

		void ClearTintColor()
		{
			switch (Control)
			{
				case UIImageView imageView:
					if (imageView.Image != null)
						imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
					break;
				case UIButton button:
					if (button.CurrentImage != null)
					{
						var originalImage = button.CurrentImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
						button.SetImage(originalImage, UIControlState.Normal);
					}

					break;
			}
		}

		void SetUIImageViewTintColor(UIImageView imageView, Color color)
		{
			if (imageView.Image == null)
			{
				Element.PropertyChanged += (_, e) =>
				{
					if (e.PropertyName == Image.IsLoadingProperty.PropertyName)
					{
						var element = Element as Xamarin.Forms.Image ?? throw new NullReferenceException();

						if (!element.IsLoading)
						{
							SetUIImageViewTintColor(imageView, color);
						}
					}
				};
			}
			else
			{
				imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
				imageView.TintColor = color.ToUIColor();
			}
		}

		void SetUIButtonTintColor(UIButton button, Color color)
		{
			if (button.ImageView.Image == null)
			{
				Element.PropertyChanged += (_, e) =>
				{
					if (e.PropertyName == ImageButton.IsLoadingProperty.PropertyName)
					{
						var element = Element as Xamarin.Forms.ImageButton ?? throw new NullReferenceException();
						if (!element.IsLoading)
						{
							SetUIButtonTintColor(button, color);
						}
					}
				};
			}
			else
			{
				var templatedImage = button.CurrentImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

				button.SetImage(null, UIControlState.Normal);

				button.TintColor = color.ToUIColor();
				button.ImageView.TintColor = color.ToUIColor();
				button.SetImage(templatedImage, UIControlState.Normal);
			}
		}
	}
}