using System.ComponentModel;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Effects.Semantic;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.SemanticEffectRouter), nameof(SemanticEffectRouter))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	/// <summary>
	/// iOS implementation of the <see cref="SemanticEffect" />
	/// </summary>
	public class SemanticEffectRouter : SemanticEffectRouterBase<SemanticEffectRouter>
	{
		public SemanticEffectRouter()
		{
		}

		protected override void Update(UIView view, SemanticEffectRouter effect)
		{
			var isHeading = SemanticEffect.GetHeadingLevel(Element) != CommunityToolkit.Effects.Semantic.HeadingLevel.None;

			if (isHeading)
				view.AccessibilityTraits |= UIAccessibilityTrait.Header;
			else
				view.AccessibilityTraits &= ~UIAccessibilityTrait.Header;

			var semanticInclusion = SemanticEffect.GetSemanticInclusion(Element);
			switch (semanticInclusion)
			{
				case SemanticInclusion.Default:
					view.IsAccessibilityElement = view is UIControl;
					view.AccessibilityElementsHidden = false;
					break;
				case SemanticInclusion.Include:
					view.IsAccessibilityElement = true;
					view.AccessibilityElementsHidden = false;
					break;
				case SemanticInclusion.Exclude:
					view.IsAccessibilityElement = false;
					view.AccessibilityElementsHidden = false;
					break;
				case SemanticInclusion.ExcludeWithChildren:
					view.IsAccessibilityElement = false;
					view.AccessibilityElementsHidden = true;
					break;
			}

			var desc = SemanticEffect.GetDescription(Element);
			var hint = SemanticEffect.GetHint(Element);
			view.AccessibilityLabel = desc;
			view.AccessibilityHint = hint;

			// UIControl elements automatically have IsAccessibilityElement set to true
			if (view is not UIControl && (!string.IsNullOrWhiteSpace(hint) || !string.IsNullOrWhiteSpace(desc)))
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