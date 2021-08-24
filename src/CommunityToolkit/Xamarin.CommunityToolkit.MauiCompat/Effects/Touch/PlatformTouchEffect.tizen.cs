using ElmSharp;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Tizen.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen;
using EColor = ElmSharp.Color;

[assembly: ExportEffect(typeof(PlatformTouchEffect), nameof(TouchEffect))]

namespace Xamarin.CommunityToolkit.Tizen.Effects
{
	public class PlatformTouchEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		GestureLayer? gestureLayer;

		TouchEffect? effect;

		protected override void OnAttached()
		{
			effect = TouchEffect.PickFrom(Element);
			if (effect?.IsDisabled ?? true)
				return;

			effect.Element = (VisualElement)Element;
			gestureLayer = new TouchTapGestureRecognizer(Control, effect);
		}

		protected override void OnDetached()
		{
			if (effect?.Element == null)
				return;

			if (gestureLayer != null)
			{
				gestureLayer.ClearCallbacks();
				gestureLayer.Unrealize();
				gestureLayer = null;
			}
			effect.Element = null;
			effect = null;
		}
	}

	sealed class TouchTapGestureRecognizer : GestureLayer
	{
		readonly TouchEffect? effect;
		bool tapCompleted;
		bool longTapStarted;

		public TouchTapGestureRecognizer(EvasObject parent)
			: base(parent)
		{
			SetTapCallback(GestureType.Tap, GestureState.Start, OnTapStarted);
			SetTapCallback(GestureType.Tap, GestureState.End, OnGestureEnded);
			SetTapCallback(GestureType.Tap, GestureState.Abort, OnGestureAborted);

			SetTapCallback(GestureType.LongTap, GestureState.Start, OnLongTapStarted);
			SetTapCallback(GestureType.LongTap, GestureState.End, OnGestureEnded);
			SetTapCallback(GestureType.LongTap, GestureState.Abort, OnGestureAborted);
		}

		public TouchTapGestureRecognizer(EvasObject parent, TouchEffect effect)
			: this(parent)
		{
			Attach(parent);
			this.effect = effect;
		}

		public bool IsCanceled { get; set; } = true;

		void OnTapStarted(TapData data)
		{
			if (effect?.IsDisabled ?? true)
				return;

			IsCanceled = false;
			HandleTouch(TouchStatus.Started, TouchInteractionStatus.Started);
		}

		void OnLongTapStarted(TapData data)
		{
			if (effect?.IsDisabled ?? true)
				return;

			IsCanceled = false;

			longTapStarted = true;
			HandleTouch(TouchStatus.Started, TouchInteractionStatus.Started);
		}

		void OnGestureEnded(TapData data)
		{
			if (effect == null || effect.IsDisabled)
				return;

			HandleTouch(effect.Status == TouchStatus.Started ? TouchStatus.Completed : TouchStatus.Canceled, TouchInteractionStatus.Completed);
			IsCanceled = true;
			tapCompleted = true;
		}

		void OnGestureAborted(TapData data)
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (tapCompleted || longTapStarted)
			{
				tapCompleted = false;
				longTapStarted = false;
				return;
			}

			HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed);
			IsCanceled = true;
		}

		public void HandleTouch(TouchStatus status, TouchInteractionStatus? touchInteractionStatus = null)
		{
			if (IsCanceled || effect == null)
				return;

			if (effect.IsDisabled)
				return;

			if (touchInteractionStatus == TouchInteractionStatus.Started)
			{
				effect?.HandleUserInteraction(TouchInteractionStatus.Started);
				touchInteractionStatus = null;
			}

			effect?.HandleTouch(status);
			if (touchInteractionStatus.HasValue)
				effect?.HandleUserInteraction(touchInteractionStatus.Value);

			if (effect == null || !effect.NativeAnimation)
				return;

			if (longTapStarted && !tapCompleted)
				return;

			var control = effect.Element;
			if (Platform.GetOrCreateRenderer(control)?.NativeView is not Widget nativeView)
				return;

			if (status == TouchStatus.Started)
			{
				var startColor = nativeView.BackgroundColor;
				if (startColor.IsDefault())
					return;

				var endColor = effect.NativeAnimationColor.ToNative();
				if (endColor.IsDefault())
				{
					startColor = EColor.FromRgba(startColor.Red, startColor.Green, startColor.Blue, startColor.Alpha / 2);
					endColor = startColor;
				}

				var transit = new Transit
				{
					Repeat = 1,
					Duration = .2
				};
				var colorEffect = new ColorEffect(startColor, endColor);
				colorEffect.EffectEnded += (s, e) => { transit?.Dispose(); };
				transit.Objects.Add(nativeView);
				transit.AddEffect(colorEffect);
				transit.Go(.2);
			}
		}
	}
}