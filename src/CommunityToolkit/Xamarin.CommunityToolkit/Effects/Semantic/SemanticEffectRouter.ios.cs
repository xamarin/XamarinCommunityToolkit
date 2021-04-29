using System.ComponentModel;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.SemanticEffectRouter), nameof(SemanticEffectRouter))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	/// <summary>
	/// iOS implementation of the <see cref="SemanticEffect" />
	/// </summary>
	public class SemanticEffectRouter : SemanticEffectRouterBase<SemanticEffectRouter>
	{
		protected override void Update(UIView view, SemanticEffectRouter effect)
		{
			var isHeading = SemanticEffect.GetHeadingLevel(Element) != CommunityToolkit.Effects.Semantic.HeadingLevel.None;

			if (isHeading)
				view.AccessibilityTraits |= UIAccessibilityTrait.Header;
			else
				view.AccessibilityTraits &= ~UIAccessibilityTrait.Header;

			var desc = SemanticEffect.GetDescription(Element);
			var hint = SemanticEffect.GetHint(Element);
			view.AccessibilityLabel = desc;
			view.AccessibilityHint = hint;

			// UIControl elements automatically have IsAccessibilityElement set to true
			if (view is not UIControl && !string.IsNullOrWhiteSpace(hint) && !string.IsNullOrWhiteSpace(desc))
			{
				view.IsAccessibilityElement = true;
			}
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (args.PropertyName == SemanticEffect.HeadingLevelProperty.PropertyName ||
							args.PropertyName == SemanticEffect.DescriptionProperty.PropertyName ||
							args.PropertyName == SemanticEffect.HintProperty.PropertyName)
			{
				Update();
			}
		}
	}
}
