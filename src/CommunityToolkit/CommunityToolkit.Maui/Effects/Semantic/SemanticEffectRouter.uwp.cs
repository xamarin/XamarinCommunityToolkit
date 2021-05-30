using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Xamarin.CommunityToolkit.Effects;
using Effects = Xamarin.CommunityToolkit.UWP.Effects;

[assembly: Xamarin.Forms.ExportEffect(typeof(Effects.SemanticEffectRouter), nameof(SemanticEffectRouter))]

namespace CommunityToolkit.Maui.UWP.Effects
{
	/// <summary>
	/// UWP implementation of the <see cref="SemanticEffect" />
	/// </summary>
	public class SemanticEffectRouter : SemanticEffectRouterBase<SemanticEffectRouter>
	{
		public SemanticEffectRouter()
		{
		}

		protected override void Update(FrameworkElement view, SemanticEffectRouter effect)
		{
			var headingLevel = (AutomationHeadingLevel)((int)SemanticEffect.GetHeadingLevel(Element));
			AutomationProperties.SetHeadingLevel(view, headingLevel);

			AutomationProperties.SetName(view, SemanticEffect.GetDescription(Element) ?? string.Empty);
			AutomationProperties.SetHelpText(view, SemanticEffect.GetHint(Element) ?? string.Empty);
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