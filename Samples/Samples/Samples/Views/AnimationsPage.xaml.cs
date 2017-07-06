using FormsCommunityToolkit.Animations;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Samples.Views
{
    public partial class AnimationsPage : TabbedPage
    {
        public AnimationsPage()
        {
            InitializeComponent();

            AnimationExtensionButton.Clicked += async (sender, args) =>
            {
                await AnimationBox.Animate(new HeartAnimation());
            };
        }
    }
}