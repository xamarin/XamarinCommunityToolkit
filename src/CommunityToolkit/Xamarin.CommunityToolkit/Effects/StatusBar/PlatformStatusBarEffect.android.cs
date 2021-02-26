using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportEffect(typeof(PlatformStatusBarEffect), nameof(StatusBarEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformStatusBarEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (StatusBarEffect.GetColor(Element) != (Forms.Color)StatusBarEffect.ColorProperty.DefaultValue)
			{
				SetColor(StatusBarEffect.GetColor(Element).ToAndroid());
			}
			if (StatusBarEffect.GetStyle(Element) != (StatusBarStyle)StatusBarEffect.StyleProperty.DefaultValue)
			{
				SetStyle(StatusBarEffect.GetStyle(Element));
			}
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == StatusBarEffect.ColorProperty.PropertyName)
			{
				SetColor(StatusBarEffect.GetColor(Element).ToAndroid());
			}
			else if (args.PropertyName == StatusBarEffect.StyleProperty.PropertyName)
			{
				SetStyle(StatusBarEffect.GetStyle(Element));
			}
		}

		public void SetColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			Activity.SetStatusBarColor(color);
		}

		public void SetStyle(StatusBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			switch (style)
			{
				case StatusBarStyle.Default:
				case StatusBarStyle.LightContent:
					RemoveBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
				case StatusBarStyle.DarkContent:
					AddBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
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

		internal static void AddBarAppearanceFlag(FormsAppCompatActivity activity, StatusBarVisibility flag) =>
			SetBarAppearance(activity, barAppearance => barAppearance |= flag);

		internal static void RemoveBarAppearanceFlag(FormsAppCompatActivity activity, StatusBarVisibility flag) =>
			SetBarAppearance(activity, barAppearance => barAppearance &= ~flag);

		internal static void SetBarAppearance(FormsAppCompatActivity activity, Func<StatusBarVisibility, StatusBarVisibility> updateAppearance)
		{
			var currentWindow = GetCurrentWindow(activity);

			var appearance = currentWindow.DecorView.SystemUiVisibility;
			appearance = updateAppearance(appearance);
			currentWindow.DecorView.SystemUiVisibility = appearance;
		}

		internal static Window GetCurrentWindow(FormsAppCompatActivity activity)
		{
			var window = activity.Window;
			window.ClearFlags(WindowManagerFlags.TranslucentStatus);
			window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			return window;
		}
	}
}