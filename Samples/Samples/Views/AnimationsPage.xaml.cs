using Xamarin.Forms;
using Xamarin.Toolkit.Animations;

namespace Xamarin.Samples.Views
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
