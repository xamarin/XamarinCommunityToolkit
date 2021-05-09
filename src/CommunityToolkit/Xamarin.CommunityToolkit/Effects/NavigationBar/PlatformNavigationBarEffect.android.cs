using System.ComponentModel;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(PlatformNavigationBarEffect), nameof(NavigationBarEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformNavigationBarEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetColor(NavigationBarEffect.GetColor(Element));
			SetStyle(NavigationBarEffect.GetStyle(Element));
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == NavigationBarEffect.ColorProperty.PropertyName)
			{
				SetColor(NavigationBarEffect.GetColor(Element));
			}
			else if (args.PropertyName == NavigationBarEffect.StyleProperty.PropertyName)
			{
				SetStyle(NavigationBarEffect.GetStyle(Element));
			}
		}

		static void SetColor(Color color)
		{
			if (!BarStyle.IsSupported())
				return;

			BarStyle.GetCurrentWindow().SetNavigationBarColor(color.ToAndroid());
		}

		static void SetStyle(NavigationBarStyle style)
		{
			if (!BarStyle.IsSupported())
				return;

			switch (style)
			{
				case NavigationBarStyle.DarkContent:
					BarStyle.AddBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
				default:
					BarStyle.RemoveBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
			}
		}
	}
}