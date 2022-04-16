using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;

[assembly: ExportEffect(typeof(PlatformTouchEffect), nameof(TouchEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformTouchEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		const string pointerDownAnimationKey = "PointerDownAnimation";

		const string pointerUpAnimationKey = "PointerUpAnimation";

		TouchEffect? effect;

		bool isPressed;

		bool isIntentionalCaptureLoss;

		Storyboard? pointerDownStoryboard;

		Storyboard? pointerUpStoryboard;

		protected override void OnAttached()
		{
			effect = TouchEffect.PickFrom(Element);
			if (effect == null || effect.IsDisabled)
				return;

			effect.Element = (VisualElement)Element;
			if (effect.NativeAnimation)
			{
				var nativeControl = Container;
				if (string.IsNullOrEmpty(nativeControl.Name))
					nativeControl.Name = Guid.NewGuid().ToString();

				if (nativeControl.Resources.ContainsKey(pointerDownAnimationKey))
				{
					pointerDownStoryboard = (Storyboard)nativeControl.Resources[pointerDownAnimationKey];
				}
				else
				{
					pointerDownStoryboard = new Storyboard();
					var downThemeAnimation = new PointerDownThemeAnimation();

					Storyboard.SetTargetName(downThemeAnimation, nativeControl.Name);

					pointerDownStoryboard.Children.Add(downThemeAnimation);

					nativeControl.Resources.Add(new KeyValuePair<object, object>(pointerDownAnimationKey, pointerDownStoryboard));
				}

				if (nativeControl.Resources.ContainsKey(pointerUpAnimationKey))
				{
					pointerUpStoryboard = (Storyboard)nativeControl.Resources[pointerUpAnimationKey];
				}
				else
				{
					pointerUpStoryboard = new Storyboard();
					var upThemeAnimation = new PointerUpThemeAnimation();

					Storyboard.SetTargetName(upThemeAnimation, nativeControl.Name);

					pointerUpStoryboard.Children.Add(upThemeAnimation);

					nativeControl.Resources.Add(new KeyValuePair<object, object>(pointerUpAnimationKey, pointerUpStoryboard));
				}
			}

			if (Container != null)
			{
				Container.PointerPressed += OnPointerPressed;
				Container.PointerReleased += OnPointerReleased;
				Container.PointerCanceled += OnPointerCanceled;
				Container.PointerExited += OnPointerExited;
				Container.PointerEntered += OnPointerEntered;
				Container.PointerCaptureLost += OnPointerCaptureLost;
			}
		}

		protected override void OnDetached()
		{
			if (effect?.Element == null)
				return;

			effect.Element = null;
			effect = null;

			if (Container != null)
			{
				Container.PointerPressed -= OnPointerPressed;
				Container.PointerReleased -= OnPointerReleased;
				Container.PointerCanceled -= OnPointerCanceled;
				Container.PointerExited -= OnPointerExited;
				Container.PointerEntered -= OnPointerEntered;
				Container.PointerCaptureLost -= OnPointerCaptureLost;

				isPressed = false;
			}
		}

		void OnPointerEntered(object? sender, PointerRoutedEventArgs e)
		{
			if (effect?.Element == null || effect.IsDisabled)
				return;

			effect?.HandleHover(HoverStatus.Entered);

			if (isPressed)
			{
				effect?.HandleTouch(TouchStatus.Started);
				AnimateTilt(pointerDownStoryboard);
			}
		}

		void OnPointerExited(object? sender, PointerRoutedEventArgs e)
		{
			if (effect?.Element == null || effect.IsDisabled)
				return;

			if (isPressed)
			{
				effect?.HandleTouch(TouchStatus.Canceled);
				AnimateTilt(pointerUpStoryboard);
			}

			effect?.HandleHover(HoverStatus.Exited);
		}

		void OnPointerCanceled(object? sender, PointerRoutedEventArgs e)
		{
			if (effect?.Element == null || effect.IsDisabled)
				return;

			isPressed = false;

			effect?.HandleTouch(TouchStatus.Canceled);
			effect?.HandleUserInteraction(TouchInteractionStatus.Completed);
			effect?.HandleHover(HoverStatus.Exited);

			AnimateTilt(pointerUpStoryboard);
		}

		void OnPointerCaptureLost(object? sender, PointerRoutedEventArgs e)
		{
			if (effect?.Element == null || effect.IsDisabled)
				return;

			if (isIntentionalCaptureLoss)
				return;

			isPressed = false;

			if (effect?.Status != TouchStatus.Canceled)
				effect?.HandleTouch(TouchStatus.Canceled);

			effect?.HandleUserInteraction(TouchInteractionStatus.Completed);

			if (effect?.HoverStatus != HoverStatus.Exited)
				effect?.HandleHover(HoverStatus.Exited);

			AnimateTilt(pointerUpStoryboard);
		}

		void OnPointerReleased(object? sender, PointerRoutedEventArgs e)
		{
			if (effect?.Element == null || effect.IsDisabled)
				return;

			if (isPressed && (effect.HoverStatus == HoverStatus.Entered))
			{
				effect?.HandleTouch(TouchStatus.Completed);
				AnimateTilt(pointerUpStoryboard);
			}
			else if (effect.HoverStatus != HoverStatus.Exited)
			{
				effect?.HandleTouch(TouchStatus.Canceled);
				AnimateTilt(pointerUpStoryboard);
			}

			effect?.HandleUserInteraction(TouchInteractionStatus.Completed);

			isPressed = false;
			isIntentionalCaptureLoss = true;
		}

		void OnPointerPressed(object? sender, PointerRoutedEventArgs e)
		{
			if (effect?.Element == null || effect.IsDisabled)
				return;

			isPressed = true;

			Container.CapturePointer(e.Pointer);

			effect?.HandleUserInteraction(TouchInteractionStatus.Started);
			effect?.HandleTouch(TouchStatus.Started);

			AnimateTilt(pointerDownStoryboard);

			isIntentionalCaptureLoss = false;
		}

		void AnimateTilt(Storyboard? storyboard)
		{
			if (storyboard != null && effect?.Element != null && effect.NativeAnimation)
			{
				try
				{
					storyboard.Stop();
					storyboard.Begin();
				}
				catch
				{
					// Suppress
				}
			}
		}
	}
}