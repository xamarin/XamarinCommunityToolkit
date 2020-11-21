using System.ComponentModel;
using System.Linq;
using Android.Graphics;
using Android.Widget;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Platform.Android;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: Xamarin.Forms.ExportEffect(typeof(Effects.IconTintColorEffectRouter), nameof(IconTintColorEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
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
				!args.PropertyName.Equals(Forms.Image.SourceProperty.PropertyName) &&
				!args.PropertyName.Equals(Forms.ImageButton.SourceProperty.PropertyName))
				return;

			ApplyTintColor();
		}

		void ApplyTintColor()
		{
			if (Control == null || Element == null)
				return;

			var color = IconTintColorEffect.GetTintColor(Element);

			switch (Control)
			{
				case ImageView image:
					SetImageViewTintColor(image, color);
					break;
				case Button button:
					SetButtonTintColor(button, color);
					break;
			}
		}

		void ClearTintColor()
		{
			switch (Control)
			{
				case ImageView image:
					image.ClearColorFilter();
					break;
				case Button button:
					var drawables = button.GetCompoundDrawables().Where(d => d != null);
					foreach (var img in drawables)
						img.ClearColorFilter();
					break;
			}
		}

		void SetImageViewTintColor(ImageView image, Forms.Color color)
		{
			if (color == Forms.Color.Default)
				image.ClearColorFilter();

			image.SetColorFilter(new PorterDuffColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcIn));
		}

		void SetButtonTintColor(Button button, Forms.Color color)
		{
			var drawables = button.GetCompoundDrawables().Where(d => d != null);

			if (color == Forms.Color.Default)
			{
				foreach (var img in drawables)
					img.ClearColorFilter();
			}

			foreach (var img in drawables)
				img.SetTint(color.ToAndroid());
		}
	}
}