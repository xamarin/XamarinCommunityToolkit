using Xamarin.CommunityToolkit.Sample.ViewModels.Views.Tabs;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class NormalTestView : ContentView
	{
		public NormalTestView()
		{
			InitializeComponent();
			BindingContext = NormalTestViewModel.Current;

			NormalTestViewModel.Current.LoadedViews += "NormalTestLoaded \n";
		}
	}
}