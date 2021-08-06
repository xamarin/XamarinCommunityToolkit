using System.Diagnostics;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class BackgroundAspectEffectPage : BasePage
    {
        public BackgroundAspectEffectPage() => InitializeComponent();

        void OnOptionChecked(object sender, CheckedChangedEventArgs e)
        {
            var aspect = GetSelectedAspect(sender);

            if (aspect == null)
                return;

            Debug.WriteLine($"Change the current effect to: {aspect}");

            BackgroundAspectEffect.SetAspect(this, aspect.Value);
        }

        Aspect? GetSelectedAspect(object sender)
		{
            if (sender == RadioFill && RadioFill.IsChecked)
            {
                return Aspect.Fill;
            }
            else if (sender == RadioAspectFill && RadioAspectFill.IsChecked)
            {
                return Aspect.AspectFill;
            }
            else if (sender == RadioAspectFit && RadioAspectFit.IsChecked)
            {
                return Aspect.AspectFit;
            }
            else
            {
                return null;
            }
        }
    }
}
