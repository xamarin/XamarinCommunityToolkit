using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui.Controls;
namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class SafeAreaEffectPage
	{
		public SafeAreaEffectPage() => InitializeComponent();

		void ActivationToggle_Toggled(object? sender, ToggledEventArgs e) =>
			SafeAreaEffect.SetSafeArea(stack, e.Value);
	}
}