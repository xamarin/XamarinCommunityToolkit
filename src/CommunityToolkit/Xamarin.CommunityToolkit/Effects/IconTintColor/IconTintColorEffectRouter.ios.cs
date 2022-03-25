using System.ComponentModel;
using System.Threading.Tasks;
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
		private int _retryCount = 0;

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

		async void SetUIImageViewTintColor(UIImageView imageView, Color color)
		{
			if (imageView.Image == null)
			{
				if (_retryCount < 4)
				{
					_retryCount++;
					await Task.Delay(100);
					SetUIImageViewTintColor(imageView, color);
				}
				else
				{
					return;
				}
			}
			else
			{
				imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
				imageView.TintColor = color.ToUIColor();
				_retryCount = 0;
			}
		}

		async void SetUIButtonTintColor(UIButton button, Color color)
		{
			if (button.CurrentImage == null)
			{
				if (_retryCount < 4)
				{
					_retryCount++;
					await Task.Delay(100);
					SetUIButtonTintColor(button, color);
				}
				else
				{
					return;
				}
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