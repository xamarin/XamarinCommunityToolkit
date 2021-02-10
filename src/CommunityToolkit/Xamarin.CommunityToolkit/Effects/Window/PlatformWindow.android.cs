using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Effects = Xamarin.CommunityToolkit.Effects;
using Color = Android.Graphics.Color;
using Window = Android.Views.Window;

[assembly: ExportEffect(typeof(PlatformWindow), nameof(Effects.Window))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformWindow : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (Effects::Window.GetStatusBarColor(Element) != (Forms.Color)Effects::Window.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(Effects::Window.GetStatusBarColor(Element).ToAndroid());
			}
			if (Effects::Window.GetStatusBarStyle(Element) != (StatusBarStyle)Effects::Window.StatusBarStyleProperty.DefaultValue)
			{
				SetStatusBarStyle(Effects::Window.GetStatusBarStyle(Element));
			}
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == Effects::Window.StatusBarColorProperty.PropertyName)
			{
				SetStatusBarColor(Effects::Window.GetStatusBarColor(Element).ToAndroid());
			}
			else if (args.PropertyName == Effects::Window.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(Effects::Window.GetStatusBarStyle(Element));
			}
		}

		public void SetStatusBarColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			Activity.SetStatusBarColor(color);
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
						barAppearanceLegacy &= ~(StatusBarVisibility)SystemUiFlags.LightStatusBar;
						barAppearance &= ~WindowInsetsControllerAppearance.LightStatusBars;
						break;
					case StatusBarStyle.DarkContent:
						barAppearanceLegacy |= (StatusBarVisibility)SystemUiFlags.LightStatusBar;
						barAppearance |= WindowInsetsControllerAppearance.LightStatusBars;
						break;
				}
				return (barAppearanceLegacy, barAppearance);
			});
		}

		void SetBarAppearance(Func<StatusBarVisibility, WindowInsetsControllerAppearance, (StatusBarVisibility, WindowInsetsControllerAppearance)> updateAppearance)
		{
			var currentWindow = GetCurrentWindow();

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

		Window GetCurrentWindow()
		{
			var window = Activity.Window;
			window.ClearFlags(WindowManagerFlags.TranslucentStatus);
			window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			return window;
		}
	}
}