using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	static class ViewSwitcher
	{
		internal static readonly BindableProperty TransitionDurationProperty
			= BindableProperty.Create(nameof(IViewSwitcher.TransitionDuration), typeof(uint), typeof(ViewSwitcher), 350u);

		internal static readonly BindableProperty TransitionTypeProperty
			= BindableProperty.Create(nameof(IViewSwitcher.TransitionType), typeof(TransitionType), typeof(ViewSwitcher), TransitionType.Fade);
	}
}