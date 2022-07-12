using System;
using System.ComponentModel;
using Windows.UI.ViewManagement;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(PlatformStatusBarEffect), nameof(StatusBarEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformStatusBarEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetColor(StatusBarEffect.GetColor(Element));
			SetStyle(StatusBarEffect.GetStyle(Element));
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == StatusBarEffect.ColorProperty.PropertyName)
			{
				SetColor(StatusBarEffect.GetColor(Element));
			}
			else if (args.PropertyName == StatusBarEffect.StyleProperty.PropertyName)
			{
				SetStyle(StatusBarEffect.GetStyle(Element));
			}
		}

		static void SetColor(Color color)
		{
			var windowsColor = color.ToWindowsColor();
			UpdateStatusBar(tb => tb.BackgroundColor = windowsColor);
		}

		static void SetStyle(StatusBarStyle style)
		{
			var foregroundColor = style switch
			{
				StatusBarStyle.LightContent => Color.White.ToWindowsColor(),
				StatusBarStyle.DarkContent => Color.Black.ToWindowsColor(),
				_ => Color.Default.ToWindowsColor(),
			};

			UpdateStatusBar(tb => tb.ForegroundColor = foregroundColor);
		}

		static void UpdateStatusBar(Action<ApplicationViewTitleBar> updateTitleBar)
		{
			var titleBar = ApplicationView.GetForCurrentView()?.TitleBar;
			if (titleBar != null)
			{
				updateTitleBar(titleBar);
			}
		}
	}
}