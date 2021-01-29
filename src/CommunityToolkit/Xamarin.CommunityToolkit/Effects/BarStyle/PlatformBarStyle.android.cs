using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using App = Android.App.Application;
using Color = Android.Graphics.Color;

[assembly: ExportEffect(typeof(PlatformBarStyle), nameof(BarStyle))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformBarStyle : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(BarStyle.GetStatusBarColor(Element).ToAndroid());
			SetStatusBarStyle(BarStyle.GetStatusBarStyle(Element));
			SetNavigationBarColor(BarStyle.GetNavigationBarColor(Element).ToAndroid());
			SetNavigationBarStyle(BarStyle.GetNavigationBarStyle(Element));
		}

		protected override void OnDetached()
		{
			if (BarStyle.GetStatusBarColor(Element) != (Forms.Color)BarStyle.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(Color.Black);
			}
			if (BarStyle.GetStatusBarStyle(Element) != (StatusBarStyle)BarStyle.StatusBarStyleProperty.DefaultValue)
			{
				SetStatusBarStyle(StatusBarStyle.Default);
			}
			if (BarStyle.GetNavigationBarColor(Element) != (Forms.Color)BarStyle.NavigationBarColorProperty.DefaultValue)
			{
				SetNavigationBarColor(Color.Black);
			}
			if (BarStyle.GetNavigationBarStyle(Element) != (NavigationBarStyle)BarStyle.NavigationBarStyleProperty.DefaultValue)
			{
				SetNavigationBarStyle(NavigationBarStyle.Default);
			}
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == BarStyle.StatusBarColorProperty.PropertyName)
			{
				SetStatusBarColor(BarStyle.GetStatusBarColor(Element).ToAndroid());
			}
			else if (args.PropertyName == BarStyle.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(BarStyle.GetStatusBarStyle(Element));
			}
			else if (args.PropertyName == BarStyle.NavigationBarColorProperty.PropertyName)
			{
				SetNavigationBarColor(BarStyle.GetNavigationBarColor(Element).ToAndroid());
			}
			else if (args.PropertyName == BarStyle.NavigationBarStyleProperty.PropertyName)
			{
				SetNavigationBarStyle(BarStyle.GetNavigationBarStyle(Element));
			}
		}

		public void SetStatusBarColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			var currentWindow = GetCurrentWindow();
			currentWindow?.SetStatusBarColor(color);
		}

		public void SetStatusBarStyle(StatusBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			SetBarAppearance((barAppearanceLegacy, barAppearance) =>
			{
				switch (style)
				{
					case StatusBarStyle.Default:
					case StatusBarStyle.LightContent:
						barAppearanceLegacy |= (StatusBarVisibility)SystemUiFlags.LightStatusBar;
						barAppearance |= WindowInsetsControllerAppearance.LightStatusBars;
						break;
					case StatusBarStyle.DarkContent:
						barAppearanceLegacy &= ~(StatusBarVisibility)SystemUiFlags.LightStatusBar;
						barAppearance &= ~WindowInsetsControllerAppearance.LightStatusBars;
						break;
				}
				return (barAppearanceLegacy, barAppearance);
			});
		}

		public void SetNavigationBarColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			var currentWindow = GetCurrentWindow();
			currentWindow?.SetNavigationBarColor(color);
		}

		public void SetNavigationBarStyle(NavigationBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			SetBarAppearance((appearanceLegacy, appearance) =>
			{
				switch (style)
				{
					case NavigationBarStyle.Default:
					case NavigationBarStyle.LightContent:
						appearanceLegacy |= (StatusBarVisibility)SystemUiFlags.LightNavigationBar;
						appearance |= WindowInsetsControllerAppearance.LightNavigationBars;
						break;
					case NavigationBarStyle.DarkContent:
						appearanceLegacy &= ~(StatusBarVisibility)SystemUiFlags.LightNavigationBar;
						appearance &= ~WindowInsetsControllerAppearance.LightNavigationBars;
						break;
				}
				return (appearanceLegacy, appearance);
			});
		}

		static void SetBarAppearance(Func<StatusBarVisibility, WindowInsetsControllerAppearance, (StatusBarVisibility, WindowInsetsControllerAppearance)> updateAppearance)
		{
			var currentWindow = GetCurrentWindow();
			if (currentWindow == null)
				return;

			StatusBarVisibility appearanceLegacy = 0;
			WindowInsetsControllerAppearance appearance = 0;
			if ((int)Build.VERSION.SdkInt < 30)
			{
#pragma warning disable CS0618 // Type or member is obsolete. Using new API for Sdk 30+
				appearanceLegacy = currentWindow.DecorView.SystemUiVisibility;
#pragma warning restore CS0618 // Type or member is obsolete
			}
			else
			{
				appearance = (WindowInsetsControllerAppearance)currentWindow.InsetsController.SystemBarsAppearance;
			}

			(appearanceLegacy, appearance) = updateAppearance(appearanceLegacy, appearance);

			if ((int)Build.VERSION.SdkInt < 30)
			{
#pragma warning disable CS0618 // Type or member is obsolete. Using new API for Sdk 30+
				currentWindow.DecorView.SystemUiVisibility = appearanceLegacy;
#pragma warning restore CS0618 // Type or member is obsolete
			}
			else
			{
				currentWindow.InsetsController.SetSystemBarsAppearance((int)appearance, (int)appearance);
			}
		}

		static Window GetCurrentWindow()
		{
			var window = App.Context.GetActivity()?.Window;
			return window;
		}
	}
}