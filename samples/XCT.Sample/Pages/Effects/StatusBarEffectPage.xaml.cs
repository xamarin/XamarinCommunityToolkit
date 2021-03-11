using Xamarin.CommunityToolkit.Sample.ViewModels.Effects;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class StatusBarEffectPage
	{
		public StatusBarEffectPage()
		{
			InitializeComponent();
			BindingContext = new StatusBarEffectViewModel();
		}
	}
}
