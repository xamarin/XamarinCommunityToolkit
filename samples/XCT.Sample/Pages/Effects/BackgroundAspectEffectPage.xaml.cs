
using System.Diagnostics;
using System.Linq;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
    public partial class BackgroundAspectEffectPage : BasePage
    {
        public BackgroundAspectEffectPage() => InitializeComponent();

        void OnOptionChecked(object sender, CheckedChangedEventArgs e)
        {
            Aspect? aspect = null;

            if (sender == RadioFill && RadioFill.IsChecked)
            {
                aspect = Aspect.Fill;
            }
            else if (sender == RadioAspectFill && RadioAspectFill.IsChecked)
            {
                aspect = Aspect.AspectFill;
            }
            else if (sender == RadioAspectFit && RadioAspectFit.IsChecked)
            {
                aspect = Aspect.AspectFit;
            }
            else
            {
                return;
            }

            Debug.WriteLine($"Change the current effect to: {aspect}");

            BackgroundAspectEffect.SetAspect(this, aspect.Value);
        }
    }
}
