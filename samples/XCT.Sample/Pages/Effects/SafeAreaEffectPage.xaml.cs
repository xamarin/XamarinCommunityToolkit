using Xamarin.CommunityToolkit.Effects;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class SafeAreaEffectPage
	{
		public SafeAreaEffectPage() => InitializeComponent();

		void ActivationToggle_Toggled(object? sender, Forms.ToggledEventArgs e) =>
			SafeAreaEffect.SetSafeArea(stack, e.Value);
	}
}