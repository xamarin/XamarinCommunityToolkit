using Xamarin.CommunityToolkit.Animations;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Behaviors
{
	public partial class AnimationBehaviorPage : BasePage
	{
		public AnimationBehaviorPage() => InitializeComponent();

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var label = (View)sender;

			new TadaAnimation(
				rotationAngle: 30,
				length: 1000,
				views: label).Commit();
        }
    }
}