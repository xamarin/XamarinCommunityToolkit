using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Xamarin.CommunityToolkit.Android.Effects.PlatformStatusBarEffect;
using Color = Android.Graphics.Color;

[assembly: ExportEffect(typeof(PlatformNavigationBarEffect), nameof(NavigationBarEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformNavigationBarEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetColor(NavigationBarEffect.GetColor(Element).ToAndroid());
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
				SetColor(NavigationBarEffect.GetColor(Element).ToAndroid());
			}
			else if (args.PropertyName == NavigationBarEffect.StyleProperty.PropertyName)
			{
				SetStyle(NavigationBarEffect.GetStyle(Element));
			}
		}

		public void SetColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			GetCurrentWindow().SetNavigationBarColor(color);
		}

		public void SetStyle(NavigationBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			switch (style)
			{
				case NavigationBarStyle.Default:
				case NavigationBarStyle.LightContent:
					RemoveBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
				case NavigationBarStyle.DarkContent:
					AddBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
			}
		}

		FormsAppCompatActivity Activity
		{
			get
			{
				if (Control != null)
					return (FormsAppCompatActivity)(Control.Context ?? throw new NullReferenceException());
				else
					return (FormsAppCompatActivity)(Container.Context ?? throw new NullReferenceException());
			}
		}

		Window GetCurrentWindow() => PlatformStatusBarEffect.GetCurrentWindow(Activity);
	}
}