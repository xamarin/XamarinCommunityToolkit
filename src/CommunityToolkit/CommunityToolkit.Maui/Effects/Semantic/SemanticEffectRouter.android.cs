using System.ComponentModel;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Core.View.Accessibiity;
using CommunityToolkit.Maui.Effects;
using Xamarin.Forms;
using Effects = CommunityToolkit.Maui.Android.Effects;

[assembly: ExportEffect(typeof(Effects.SemanticEffectRouter), nameof(SemanticEffectRouter))]

namespace CommunityToolkit.Maui.Android.Effects
{
	/// <summary>
	/// Android implementation of the <see cref="SemanticEffect" />
	/// </summary>
	public class SemanticEffectRouter : SemanticEffectRouterBase<SemanticEffectRouter>
	{
		SemanticAccessibilityDelegate? semanticAccessibilityDelegate;

		protected override void Update(global::Android.Views.View view, SemanticEffectRouter effect)
		{
			var isHeading = SemanticEffect.GetHeadingLevel(Element) != CommunityToolkit.Maui.Effects.Semantic.HeadingLevel.None;
			ViewCompat.SetAccessibilityHeading(view, isHeading);
			var desc = SemanticEffect.GetDescription(Element);
			var hint = SemanticEffect.GetHint(Element);

			if (!string.IsNullOrEmpty(hint) || !string.IsNullOrEmpty(desc))
			{
				if (semanticAccessibilityDelegate == null)
				{
					semanticAccessibilityDelegate = new SemanticAccessibilityDelegate(Element);
					ViewCompat.SetAccessibilityDelegate(view, semanticAccessibilityDelegate);
				}
			}
			else if (semanticAccessibilityDelegate != null)
			{
				semanticAccessibilityDelegate = null;
				ViewCompat.SetAccessibilityDelegate(view, null);
			}

			if (semanticAccessibilityDelegate != null)
			{
				semanticAccessibilityDelegate.Element = Element;
				view.ImportantForAccessibility = global::Android.Views.ImportantForAccessibility.Yes;
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
				if (!string.IsNullOrEmpty(desc))
				{
					info.ContentDescription = desc;
				}
			}
		}
	}
}