using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.Views.Accessibility;
using AndroidX.Core.Content;
using Android.Graphics;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

using AOS = Android.OS;

using AColor = Android.Graphics.Color;

[assembly: ExportEffect(typeof(Effects.BackgroundAspectEffectRouter), nameof(BackgroundAspectEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class BackgroundAspectEffectRouter : PlatformEffect
	{
		//readonly Context context;

		public BackgroundAspectEffectRouter()
		{
		}

		// TODO: Pass context in so we can get image from handler
		//public BackgroundAspectEffectRouter(Context context)
		//{
		//	this.context = context;
		//}

		protected override async void OnAttached()
		{
			return;
			//Debugger.Break();

			var contentPage = Element as ContentPage;

			if (Element == null || contentPage == null || contentPage.BackgroundImageSource == null)
				return;

			//var image = await GetBackgroundImageFromSource(contentPage.BackgroundImageSource);

			//if (image == null)
			//	return;

			var fallbackColor = contentPage.BackgroundColor.ToAndroid();

			ApplyImageToView(null!, fallbackColor, contentPage);
		}

		protected override void OnDetached()
		{

		}

		void ApplyImageToView(Bitmap image, AColor color, ContentPage contentPage)
		{
			var effectParam = BackgroundAspectEffect.GetAspect(Element);

			var hotPink = AColor.HotPink;

			if (Container == null)
				return;

			// Working with colors

			var backgroundDrawable = Container.Background!;

			if (AOS.Build.VERSION.SdkInt >= AOS.BuildVersionCodes.Q)
			{
				var colorFilter = new BlendModeColorFilter(hotPink, BlendMode.SrcAtop!);

				backgroundDrawable.SetColorFilter(colorFilter);
			}
			else
			{
				var mode = PorterDuff.Mode.SrcAtop!;

				backgroundDrawable.SetColorFilter(hotPink, mode);
			}
		}

		async Task<Bitmap> GetBackgroundImageFromSource(ImageSource imageSource)
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

			return await handler.LoadImageAsync(imageSource, null)
				.ConfigureAwait(false);
		}
	}
}
