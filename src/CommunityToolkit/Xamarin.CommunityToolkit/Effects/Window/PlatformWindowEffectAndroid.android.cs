using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportEffect(typeof(PlatformWindowEffectAndroid), nameof(WindowEffectAndroid))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformWindowEffectAndroid : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (WindowEffectAndroid.GetNavigationBarColor(Element) != (Forms.Color)WindowEffectAndroid.NavigationBarColorProperty.DefaultValue)
			{
				SetNavigationBarColor(WindowEffectAndroid.GetNavigationBarColor(Element).ToAndroid());
			}
			if (WindowEffectAndroid.GetNavigationBarStyle(Element) != (NavigationBarStyle)WindowEffectAndroid.NavigationBarStyleProperty.DefaultValue)
			{
				SetNavigationBarStyle(WindowEffectAndroid.GetNavigationBarStyle(Element));
			}
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == WindowEffectAndroid.NavigationBarColorProperty.PropertyName)
			{
				SetNavigationBarColor(WindowEffectAndroid.GetNavigationBarColor(Element).ToAndroid());
			}
			else if (args.PropertyName == WindowEffectAndroid.NavigationBarStyleProperty.PropertyName)
			{
				SetNavigationBarStyle(WindowEffectAndroid.GetNavigationBarStyle(Element));
			}
		}

		public void SetNavigationBarColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			GetCurrentWindow().SetNavigationBarColor(color);
		}

		public void SetNavigationBarStyle(NavigationBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			PlatformWindowEffect.SetBarAppearance(Activity, barAppearance =>
			{
				switch (style)
				{
					case NavigationBarStyle.Default:
					case NavigationBarStyle.LightContent:
						barAppearance &= ~(StatusBarVisibility)SystemUiFlags.LightNavigationBar;
						break;
					case NavigationBarStyle.DarkContent:
						barAppearance |= (StatusBarVisibility)SystemUiFlags.LightNavigationBar;
						break;
				}
				return barAppearance;
			});
		}

		FormsAppCompatActivity Activity
		{
			get
			{
				if (Control != null)
					return (FormsAppCompatActivity)Control.Context;
				else
					return (FormsAppCompatActivity)Container.Context;
			}
		}

		Window GetCurrentWindow() => PlatformWindowEffect.GetCurrentWindow(Activity);
	}
}