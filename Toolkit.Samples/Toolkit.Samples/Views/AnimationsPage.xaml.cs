using Xamarin.Toolkit.Animations;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Samples.Views
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