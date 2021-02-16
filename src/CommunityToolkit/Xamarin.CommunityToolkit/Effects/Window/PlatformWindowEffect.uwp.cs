using System.ComponentModel;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(PlatformWindowEffect), nameof(WindowEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformWindowEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToWindowsColor());
			SetStatusBarStyle(WindowEffect.GetStatusBarStyle(Element));
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == WindowEffect.StatusBarColorProperty.PropertyName)
			{
				SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToWindowsColor());
			}
			else if (args.PropertyName == WindowEffect.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(WindowEffect.GetStatusBarStyle(Element));
			}
		}

		public void SetStatusBarColor(Windows.UI.Color color)
		{
			if (ApiInformation.IsTypePresent(typeof(StatusBar).FullName ?? string.Empty))
			{
				var statusBar = StatusBar.GetForCurrentView();
				if (statusBar != null)
				{
					statusBar.BackgroundColor = color;
				}
			}
			else
			{
				var titleBar = ApplicationView.GetForCurrentView()?.TitleBar;
				if (titleBar != null)
				{
					titleBar.BackgroundColor = color;
				}
			}
		}

		public void SetStatusBarStyle(StatusBarStyle style)
		{
			var foregroundColor = style switch
			{
				StatusBarStyle.LightContent => Color.White,
				StatusBarStyle.DarkContent => Color.Black,
				_ => Color.Default,
			};

			if (ApiInformation.IsTypePresent(typeof(StatusBar).FullName ?? string.Empty))
			{
				var statusBar = StatusBar.GetForCurrentView();
				if (statusBar != null)
				{
					statusBar.ForegroundColor = foregroundColor.ToWindowsColor();
				}
			}
			else
			{
				var titleBar = ApplicationView.GetForCurrentView()?.TitleBar;
				if (titleBar != null)
				{
					titleBar.ForegroundColor = foregroundColor.ToWindowsColor();
				}
			}
		}
	}
}
