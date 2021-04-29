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
		}


		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (args.PropertyName == SemanticEffect.HeadingLevelProperty.PropertyName)
			{
				Update();
			}
		}
	}
}
