using AndroidX.Core.View;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Effects;
using Effects = Xamarin.CommunityToolkit.Android.Effects;
using AndroidX.Core.View.Accessibiity;
using Android.Widget;

[assembly: ExportEffect(typeof(Effects.SemanticEffectRouter), nameof(SemanticEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	/// <summary>
	/// Android implementation of the <see cref="SemanticEffect" />
	/// </summary>
	public class SemanticEffectRouter : SemanticEffectRouterBase<SemanticEffectRouter>
	{
		SemanticAccessibilityDelegate? semanticAccessibilityDelegate;

		protected override void Update(global::Android.Views.View view, SemanticEffectRouter effect)
		{
			var isHeading = SemanticEffect.GetHeadingLevel(Element) != CommunityToolkit.Effects.Semantic.HeadingLevel.None;
			ViewCompat.SetAccessibilityHeading(view, isHeading);
			var desc = SemanticEffect.GetDescription(Element);
			var hint = SemanticEffect.GetHint(Element);

			if (string.IsNullOrWhiteSpace(Element.AutomationId))
				view.ContentDescription = desc;

			if (!string.IsNullOrEmpty(hint) && semanticAccessibilityDelegate == null)
			{
				semanticAccessibilityDelegate = new SemanticAccessibilityDelegate(Element);
				ViewCompat.SetAccessibilityDelegate(view, semanticAccessibilityDelegate);
			}
			else if (semanticAccessibilityDelegate != null)
			{
				semanticAccessibilityDelegate = null;
				ViewCompat.SetAccessibilityDelegate(view, null);
			}

			if (semanticAccessibilityDelegate != null)
				semanticAccessibilityDelegate.Element = Element;

			if (!string.IsNullOrWhiteSpace(hint) || !string.IsNullOrWhiteSpace(desc))
			{
				view.ImportantForAccessibility = global::Android.Views.ImportantForAccessibility.Yes;
			}
			else
			{
				view.ImportantForAccessibility = global::Android.Views.ImportantForAccessibility.Auto;
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

		class SemanticAccessibilityDelegate : AccessibilityDelegateCompat
		{
			public Element Element { get; set; }

			public SemanticAccessibilityDelegate(Element element)
			{
				Element = element;
			}

			public override void OnInitializeAccessibilityNodeInfo(global::Android.Views.View host, AccessibilityNodeInfoCompat info)
			{
				base.OnInitializeAccessibilityNodeInfo(host, info);

				if (Element == null)
					return;

				if (info == null)
					return;

				var hint = SemanticEffect.GetHint(Element);
				if (!string.IsNullOrEmpty(hint))
				{
					info.HintText = hint;

					if (host is EditText)
						info.ShowingHintText = false;
				}

				var desc = SemanticEffect.GetDescription(Element);
				if (!string.IsNullOrEmpty(desc) && !string.IsNullOrWhiteSpace(Element.AutomationId))
				{
					info.ContentDescription = desc;
				}
			}
		}
	}
}
