using CommunityToolkit.Maui.Sample.ViewModels.Views.Tabs;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Sample.Pages.Views.TabView
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