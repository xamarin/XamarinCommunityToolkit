using CommunityToolkit.Maui.Effects;

namespace CommunityToolkit.Maui.Sample.Pages.Effects
{
	public partial class SafeAreaEffectPage
	{
		public SafeAreaEffectPage() => InitializeComponent();

		void ActivationToggle_Toggled(object? sender, Forms.ToggledEventArgs e) =>
			SafeAreaEffect.SetSafeArea(stack, e.Value);
	}
}