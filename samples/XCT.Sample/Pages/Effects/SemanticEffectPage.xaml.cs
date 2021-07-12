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

		void HideButton_Clicked(System.Object sender, System.EventArgs e)
		{
			SemanticEffect.SetSemanticInclusion(this.mainLayout, SemanticInclusion.ExcludeWithChildren);
		}
	}
}