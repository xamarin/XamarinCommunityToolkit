using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportEffect(typeof(PlatformBarStyle), nameof(BarStyle))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformBarStyle : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (BarStyle.GetStatusBarColor(Element) != (Forms.Color)BarStyle.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(BarStyle.GetStatusBarColor(Element).ToAndroid());
			}
			if (BarStyle.GetStatusBarStyle(Element) != (StatusBarStyle)BarStyle.StatusBarStyleProperty.DefaultValue)
			{
				SetStatusBarStyle(BarStyle.GetStatusBarStyle(Element));
			}
		}

		protected override void OnDetached()
		{
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
		}

		public void SetStatusBarColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
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