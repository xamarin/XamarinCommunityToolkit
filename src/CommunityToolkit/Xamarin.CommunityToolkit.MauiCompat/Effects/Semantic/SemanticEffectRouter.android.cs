using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System.ComponentModel;
using Android.OS;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Core.View.Accessibility;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Effects.Semantic;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

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

			var a11yVisibility = SemanticEffect.GetSemanticInclusion(Element);
			switch (a11yVisibility)
			{
				case SemanticInclusion.Default:
					ViewCompat.SetImportantForAccessibility(view, ViewCompat.ImportantForAccessibilityAuto);
					break;
				case SemanticInclusion.Include:
					ViewCompat.SetImportantForAccessibility(view, ViewCompat.ImportantForAccessibilityYes);
					break;
				case SemanticInclusion.Exclude:
					ViewCompat.SetImportantForAccessibility(view, ViewCompat.ImportantForAccessibilityNo);
					break;
				case SemanticInclusion.ExcludeWithChildren:
					ViewCompat.SetImportantForAccessibility(view, ViewCompat.ImportantForAccessibilityNoHideDescendants);
					break;
			}

			var desc = SemanticEffect.GetDescription(Element);
			var hint = SemanticEffect.GetHint(Element);

			if (!string.IsNullOrWhiteSpace(hint) || !string.IsNullOrWhiteSpace(desc))
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

				string? newText = null;
				string? newContentDescription = null;

				var desc = SemanticEffect.GetDescription(Element);
				if (!string.IsNullOrEmpty(desc))
				{
					// Edit Text fields won't read anything for the content description
					if (host is EditText et)
					{
						if (!string.IsNullOrEmpty(et.Text))
							newText = $"{desc}, {et.Text}";
						else
							newText = $"{desc}";
					}
					else
						newContentDescription = desc;
				}

				var hint = SemanticEffect.GetHint(Element);
				if (!string.IsNullOrEmpty(hint))
				{
					// info HintText won't read anything back when using TalkBack pre API 26
					if (XCT.SdkInt >= (int)BuildVersionCodes.O)
					{
						info.HintText = hint;

						if (host is EditText)
							info.ShowingHintText = false;
					}
					else
					{
						if (host is EditText et)
						{
							newText = newText ?? et.Text;
							newText = $"{newText}, {hint}";
						}
						else if (host is TextView tv)
						{
							if (newContentDescription != null)
							{
								newText = $"{newContentDescription}, {hint}";
							}
							else if (!string.IsNullOrEmpty(tv.Text))
							{
								newText = $"{tv.Text}, {hint}";
							}
							else
							{
								newText = $"{hint}";
							}
						}
						else
						{
							if (newContentDescription != null)
							{
								newText = $"{newContentDescription}, {hint}";
							}
							else
							{
								newText = $"{hint}";
							}
						}

						newContentDescription = null;
					}
				}

				if (!string.IsNullOrWhiteSpace(newContentDescription))
					info.ContentDescription = newContentDescription;

				if (!string.IsNullOrWhiteSpace(newText))
					info.Text = newText;
			}
		}
	}
}