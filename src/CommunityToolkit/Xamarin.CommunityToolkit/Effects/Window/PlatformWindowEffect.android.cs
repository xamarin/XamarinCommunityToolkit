using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportEffect(typeof(PlatformWindowEffect), nameof(WindowEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformWindowEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (WindowEffect.GetStatusBarColor(Element) != (Forms.Color)WindowEffect.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToAndroid());
			}
			if (WindowEffect.GetStatusBarStyle(Element) != (StatusBarStyle)WindowEffect.StatusBarStyleProperty.DefaultValue)
			{
				SetStatusBarStyle(WindowEffect.GetStatusBarStyle(Element));
			}
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == WindowEffect.StatusBarColorProperty.PropertyName)
			{
				SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToAndroid());
			}
			else if (args.PropertyName == WindowEffect.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(WindowEffect.GetStatusBarStyle(Element));
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

			SetBarAppearance(barAppearance =>
			{
				switch (style)
				{
					case StatusBarStyle.Default:
					case StatusBarStyle.LightContent:
						barAppearance &= ~(StatusBarVisibility)SystemUiFlags.LightStatusBar;
						break;
					case StatusBarStyle.DarkContent:
						barAppearance |= (StatusBarVisibility)SystemUiFlags.LightStatusBar;
						break;
				}
				return barAppearance;
			});
		}

		void SetBarAppearance(Func<StatusBarVisibility, StatusBarVisibility> updateAppearance)
		{
			var currentWindow = GetCurrentWindow();

			var appearance = currentWindow.DecorView.SystemUiVisibility;
			appearance = updateAppearance(appearance);
			currentWindow.DecorView.SystemUiVisibility = appearance;
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