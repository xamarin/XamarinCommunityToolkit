using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Effects.Semantic;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class SemanticEffectPage : BasePage
	{
		public SemanticEffectPage()
		{
			InitializeComponent();
		}

		void ExcludeButton_Clicked(object sender, System.EventArgs e)
		{
			SemanticEffect.SetSemanticInclusion(semanticInclusionSampleLayout, SemanticInclusion.ExcludeWithChildren);
		}
	}
}