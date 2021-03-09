using System.Threading.Tasks;
using ElmSharp;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Tizen.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using EColor = ElmSharp.Color;

[assembly: ExportEffect(typeof(PlatformTouchEffect), nameof(TouchEffect))]

namespace Xamarin.CommunityToolkit.Tizen.Effects
{
	public class PlatformTouchEffect : PlatformEffect
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
			SetTapCallback(GestureType.Tap, GestureState.Start, async data => await OnTapStarted(data));
			SetTapCallback(GestureType.Tap, GestureState.End, async data => await OnGestureEnded(data));
			SetTapCallback(GestureType.Tap, GestureState.Abort, async data => await OnGestureAborted(data));

			SetTapCallback(GestureType.LongTap, GestureState.Start, async data => await OnLongTapStarted(data));
			SetTapCallback(GestureType.LongTap, GestureState.End, async data => await OnGestureEnded(data));
			SetTapCallback(GestureType.LongTap, GestureState.Abort, async data => await OnGestureAborted(data));
		}

		public TouchTapGestureRecognizer(EvasObject parent, TouchEffect effect)
			: this(parent)
		{
			Attach(parent);
			this.effect = effect;
		}

		public bool IsCanceled { get; set; } = true;

		async Task OnTapStarted(TapData data)
		{
			if (effect?.IsDisabled ?? true)
				return;

			IsCanceled = false;
			await HandleTouch(TouchStatus.Started, TouchInteractionStatus.Started);
		}

		async Task OnLongTapStarted(TapData data)
		{
			if (effect?.IsDisabled ?? true)
				return;

			IsCanceled = false;

			longTapStarted = true;
			await HandleTouch(TouchStatus.Started, TouchInteractionStatus.Started);
		}

		async Task OnGestureEnded(TapData data)
		{
			if (effect == null || effect.IsDisabled)
				return;

			await HandleTouch(effect.Status == TouchStatus.Started ? TouchStatus.Completed : TouchStatus.Canceled, TouchInteractionStatus.Completed);
			IsCanceled = true;
			tapCompleted = true;
		}

		async Task OnGestureAborted(TapData data)
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (tapCompleted || longTapStarted)
			{
				tapCompleted = false;
				longTapStarted = false;
				return;
			}

			await HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed);
			IsCanceled = true;
		}

		public async Task HandleTouch(TouchStatus status, TouchInteractionStatus? touchInteractionStatus = null)
		{
			if (IsCanceled || effect == null)
				return;

			if (effect.IsDisabled)
				return;

			if (touchInteractionStatus == TouchInteractionStatus.Started)
			{
				effect.HandleUserInteraction(TouchInteractionStatus.Started);
				touchInteractionStatus = null;
			}

			await effect.HandleTouch(status);
			if (touchInteractionStatus.HasValue)
				effect.HandleUserInteraction(touchInteractionStatus.Value);

			if (!effect.NativeAnimation)
				return;

			if (longTapStarted && !tapCompleted)
				return;

			var control = effect.Element;
			if (!(Platform.GetOrCreateRenderer(control)?.NativeView is Widget nativeView))
				return;

			if (status == TouchStatus.Started)
			{
				var startColor = nativeView.BackgroundColor;
				if (startColor.IsDefault)
					return;

				var endColor = effect.NativeAnimationColor.ToNative();
				if (endColor.IsDefault)
				{
					startColor = EColor.FromRgba(startColor.R, startColor.G, startColor.B, startColor.A / 2);
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