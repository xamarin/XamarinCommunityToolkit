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
using Android.Graphics.Drawables;
using static Android.Widget.ImageView;
using AImageView = Android.Widget.ImageView;

[assembly: ExportEffect(typeof(Effects.BackgroundAspectEffectRouter), nameof(BackgroundAspectEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class BackgroundAspectEffectRouter : PlatformEffect
	{
		readonly Context context;
		Drawable oldBackground;
		AImageView imageView;

		public BackgroundAspectEffectRouter()
		{
			context = null!;
			imageView = null!; //new AImageView(context);
			oldBackground = null!;
		}

		//TODO: Pass context in so we can get image from handler
		//public BackgroundAspectEffectRouter(Context context)
		//{
		//	//this.context = Context.;
		//}

		protected override async void OnAttached()
		{
			return;
			//Debugger.Break();

			var contentPage = Element as ContentPage;

			if (Element == null || contentPage == null || contentPage.BackgroundImageSource == null)
				return;

			var image = await GetBackgroundImageFromSource(contentPage.BackgroundImageSource);

			if (image == null)
				return;

			//var fallbackColor = contentPage.BackgroundColor.ToAndroid();

			//ApplyBackgroundColorToView();

			var aspect = BackgroundAspectEffect.GetAspect(contentPage);

			var scaleType = GetScaleType(aspect);

			ApplyImageToView(image, scaleType, contentPage);
		}

		protected override void OnDetached()
		{
			if (oldBackground != null)
			{
				Container.Background = oldBackground;
				oldBackground = null!;
			}
		}

		void ApplyImageToView(Bitmap imageBitmap, ScaleType scaleType, ContentPage contentPage)
		{
			var bitmapDrawable = new BitmapDrawable(context.Resources, imageBitmap);

			imageView.SetImageDrawable(bitmapDrawable);
			imageView.SetScaleType(scaleType);

			var wrappedLayout = GetFormsImageViewLayout(imageView, contentPage.Content);

			contentPage.Content = wrappedLayout;
		}

		Xamarin.Forms.View GetFormsImageViewLayout(AImageView imageView, Forms.View pageContent)
		{
			//return imageView.ToView();
			var formsImageView = imageView.ToView();

			var absoluteLayout = new Xamarin.Forms.AbsoluteLayout()
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			AbsoluteLayout.SetLayoutFlags(formsImageView, AbsoluteLayoutFlags.All);
			Xamarin.Forms.AbsoluteLayout.SetLayoutBounds(formsImageView, new Rectangle(0, 0, 1, 1));

			absoluteLayout.Children.Add(formsImageView);

			Xamarin.Forms.AbsoluteLayout.SetLayoutFlags(pageContent, AbsoluteLayoutFlags.All);
			Xamarin.Forms.AbsoluteLayout.SetLayoutBounds(pageContent, new Rectangle(0, 0, 1, 1));

			absoluteLayout.Children.Add(pageContent);

			return absoluteLayout;
		}

		//void ApplyBackgroundColorToView()
		//{
		//	var effectParam = BackgroundAspectEffect.GetAspect(Element);

		//	var hotPink = AColor.HotPink;

		//	if (Container == null)
		//		return;

		//	// Working with colors

		//	var backgroundDrawable = Container.Background!;

		//	if (AOS.Build.VERSION.SdkInt >= AOS.BuildVersionCodes.Q)
		//	{
		//		var colorFilter = new BlendModeColorFilter(hotPink, BlendMode.SrcAtop!);

		//		backgroundDrawable.SetColorFilter(colorFilter);
		//	}
		//	else
		//	{
		//		var mode = PorterDuff.Mode.SrcAtop!;

		//		backgroundDrawable.SetColorFilter(hotPink, mode);
		//	}
		//}

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

		ScaleType GetScaleType(Aspect aspect)
		{
			switch (aspect)
			{
				default:
				case Aspect.Fill:
					return ScaleType.FitXy!;

				case Aspect.AspectFill:
					return ScaleType.CenterCrop!;

				case Aspect.AspectFit:
					return ScaleType.CenterInside!;
			}
		}
	}
}
