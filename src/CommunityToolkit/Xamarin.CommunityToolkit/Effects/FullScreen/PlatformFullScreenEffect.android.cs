using System.ComponentModel;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Xamarin.CommunityToolkit.Effects.FullScreenEffect;
using static Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific.FullScreenEffect;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(Effects.FullScreenEffectRouter), nameof(FullScreenEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class FullScreenEffectRouter : PlatformEffect
	{
		Page PageElement => (Page)Element;

		Window? Window => Container.Context.GetActivity()?.Window;

		StatusBarVisibility savedSystemUiVisibility;

		bool wasAlreadyReset;

		protected override void OnAttached()
		{
			// should we use WeakEventManager instead?
			PageElement.Disappearing += FullScreenEffectRouter_Disappearing;

			// TODO: Remove if not required
			InitialHasNavigationBar = NavigationPage.GetHasNavigationBar(PageElement);
			if (GetMode(Element) == FullScreenMode.Disabled && Window?.DecorView != null)
			{
				savedSystemUiVisibility = Window.DecorView.SystemUiVisibility;
			}

			UpdateStatusBar();
		}

		void FullScreenEffectRouter_Disappearing(object sender, System.EventArgs e)
		{
			PageElement.Disappearing -= FullScreenEffectRouter_Disappearing;
			ResetStatusBar();
		}

		protected override void OnDetached() => ResetStatusBar();

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (args.PropertyName.Equals(ModeProperty.PropertyName))
			{
				UpdateStatusBar();
			}
		}

		void UpdateStatusBar()
		{
			if (Container == null || Element == null)
			{
				return;
			}

			var fullScreenMode = GetMode(Element);
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
			if (wasAlreadyReset)
			{
				return;
			}

			var fullScreenMode = GetMode(Element);
			var isPersistent = GetIsPersistent(Element);
			wasAlreadyReset = true;

			if (fullScreenMode == FullScreenMode.Disabled)
			{
				RestoreStatusBar();
				return;
			}

			if (!isPersistent)
			{
				DisableFullScreen();
			}
		}

		void RestoreStatusBar()
		{
			if (Window?.DecorView != null)
			{
				Window.DecorView.SystemUiVisibility = savedSystemUiVisibility;
			}
		}

		// Still under work to handle different APIs
		void EnableFullScreen(FullScreenMode mode)
		{
			if (Window == null)
			{
				return;
			}

			// Supported in Android API > 17 (deprecated in API30 (Android 11) which is not supported by xct yet)
			if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
			{
				var view = Window.DecorView;
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
				Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
			}

			// TODO: Remove if not required
			NavigationPage.SetHasNavigationBar(PageElement, false);
		}

		void DisableFullScreen()
		{
			var window = Container.Context.GetActivity()?.Window;
			var view = window?.DecorView;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1 && view != null)
			{
				view.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
				NavigationPage.SetHasNavigationBar(PageElement, InitialHasNavigationBar);
			}

			// Android API < 17, not tested yet should we even support that?
			else
			{
				window?.ClearFlags(WindowManagerFlags.Fullscreen);
			}
		}
	}
}