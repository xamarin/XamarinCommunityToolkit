using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SafeAreaEffectPage
	{
		public SafeAreaEffectPage() => InitializeComponent();

		void ActivationToggle_Toggled(object sender, Forms.ToggledEventArgs e) =>
			SafeAreaEffect.SetSafeArea(stack, e.Value);
	}
}