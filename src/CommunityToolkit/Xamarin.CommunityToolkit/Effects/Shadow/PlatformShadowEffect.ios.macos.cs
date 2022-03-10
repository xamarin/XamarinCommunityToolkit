using System;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

#if __IOS__
using NativeView = UIKit.UIView;
using Xamarin.Forms.Platform.iOS;
using Xamarin.CommunityToolkit.iOS.Effects;
#elif __MACOS__
using NativeView = AppKit.NSView;
using Xamarin.Forms.Platform.MacOS;
using Xamarin.CommunityToolkit.macOS.Effects;
#endif

[assembly: ExportEffect(typeof(PlatformShadowEffect), nameof(ShadowEffect))]

#if __IOS__
namespace Xamarin.CommunityToolkit.iOS.Effects
#elif __MACOS__
namespace Xamarin.CommunityToolkit.macOS.Effects
#endif
{
public class PlatformShadowEffect : PlatformEffect
{
	const float defaultRadius = 10f;

	const float defaultOpacity = .5f;

	NativeView? View
	{
		get
		{
			var view = Control ?? Container;
			return Element is Frame ? view?.Subviews.FirstOrDefault() ?? view : view;
		}
	}

	protected override void OnAttached()
	{
		if (View == null)
			return;

		Update(View);
	}

	protected override void OnDetached()
	{
		if (View?.Layer == null)
			return;

		View.Layer.ShadowOpacity = 0;
	}

	protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
	{
		base.OnElementPropertyChanged(args);

		if (View == null)
			return;

		switch (args.PropertyName)
		{
			case ShadowEffect.ColorPropertyName:
				UpdateColor(View);
				break;
			case ShadowEffect.OpacityPropertyName:
				UpdateOpacity(View);
				break;
			case ShadowEffect.RadiusPropertyName:
				UpdateRadius(View);
				break;
			case ShadowEffect.OffsetXPropertyName:
			case ShadowEffect.OffsetYPropertyName:
				UpdateOffset(View);
				break;
			case nameof(VisualElement.Width):
			case nameof(VisualElement.Height):
			case nameof(VisualElement.BackgroundColor):
			case nameof(IBorderElement.CornerRadius):
				Update(View);
				break;
		}
	}

	void UpdateColor(in NativeView view)
	{
		if (view.Layer != null)
			view.Layer.ShadowColor = ShadowEffect.GetColor(Element).ToCGColor();
	}

	void UpdateOpacity(in NativeView view)
	{
		if (view.Layer != null)
		{
			var opacity = (float)ShadowEffect.GetOpacity(Element);
			view.Layer.ShadowOpacity = opacity < 0 ? defaultOpacity : opacity;
		}
	}

	void UpdateRadius(in NativeView view)
	{
		if (view.Layer != null)
		{
			var radius = (nfloat)ShadowEffect.GetRadius(Element);
			view.Layer.ShadowRadius = radius < 0 ? defaultRadius : radius;
		}
	}

	void UpdateOffset(in NativeView view)
	{
		if (view.Layer != null)
			view.Layer.ShadowOffset = new CGSize((double)ShadowEffect.GetOffsetX(Element), (double)ShadowEffect.GetOffsetY(Element));
	}

	void Update(in NativeView view)
	{
		UpdateColor(view);
		UpdateOpacity(view);
		UpdateRadius(view);
		UpdateOffset(view);
	}
}
}