using System.ComponentModel;
using Android.Views;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Platform.Android;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

using Xamarin.Forms;
using Android.OS;

[assembly: ExportEffect(typeof(Effects.FullScreenEffectRouter), nameof(FullScreenEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class FullScreenEffectRouter : PlatformEffect
	{
		bool isDetaching;

		protected override void OnAttached()
		{
			(Element as Page).Disappearing += FullScreenEffectRouter_Disappearing;

			// TODO: Remove if not required
			FullScreenEffect.InitialHasNavigationBar = NavigationPage.GetHasNavigationBar(Element as Page);
			UpdateStatusBar();
		}

		void FullScreenEffectRouter_Disappearing(object sender, System.EventArgs e) => ResetStatusBar();

		protected override void OnDetached()
		{
			(Element as Page).Disappearing -= FullScreenEffectRouter_Disappearing;
			isDetaching = true;
			ResetStatusBar();
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (!args.PropertyName.Equals(FullScreenEffect.ModeProperty.PropertyName) || isDetaching)
				return;

			UpdateStatusBar();
		}

		void UpdateStatusBar()
		{
			if (Container == null || Element == null)
			{
				return;
			}

			var fullScreenMode = FullScreenEffect.GetMode(Element);
			if (fullScreenMode != FullScreenMode.Disabled)
			{
				EnableFullScreen(fullScreenMode);
			}
			else
			{
				DisableFullScreen();
			}
		}

		void ResetStatusBar()
		{
			var fullScreenMode = FullScreenEffect.GetMode(Element);
			var isPersistent = FullScreenEffect.GetIsPersistent(Element);
			if (fullScreenMode != FullScreenMode.Disabled && !isPersistent)
			{
				DisableFullScreen();
			}
		}

		// Still under work to handle different APIs
		void EnableFullScreen(FullScreenMode mode)
		{
			var window = Container.Context.GetActivity()?.Window;

			// Supported in Android API > 17 (deprecated in API30, not supported by xct yet)
			if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
			{
				var view = window?.DecorView;
				if (view != null)
				{
					var flags = mode switch
					{
						FullScreenMode.LeanBackDroid => SystemUiFlags.Fullscreen | SystemUiFlags.HideNavigation,
						FullScreenMode.ImmersiveDroid => SystemUiFlags.Fullscreen | SystemUiFlags.Immersive |
													SystemUiFlags.HideNavigation,
						FullScreenMode.StickyImmersiveDroid => SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky,
						FullScreenMode.Enabled => SystemUiFlags.Fullscreen,
						_ => SystemUiFlags.Visible
					};
					view.SystemUiVisibility = (StatusBarVisibility)flags;
				}
			}

			// Android API < 17, not tested yet should we support that?
			else
			{
				window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
			}

			// TODO: Remove if not required
			NavigationPage.SetHasNavigationBar(Element as Page, false);
		}

		void DisableFullScreen()
		{
			var window = Container.Context.GetActivity()?.Window;
			var view = window?.DecorView;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
			{
				if (view != null)
				{
					view.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
					NavigationPage.SetHasNavigationBar(Element as Page, FullScreenEffect.InitialHasNavigationBar);
				}
			}

			// Android API < 17, not tested yet should we even support that?
			else
			{
				window?.ClearFlags(WindowManagerFlags.Fullscreen);
			}
		}
	}
}