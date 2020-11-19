using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.MarkupSample
{
    public partial class App : Application
    {
		readonly SearchViewModel searchViewModel = new SearchViewModel();

		public App()
		{
			InitializeComponent();

			searchViewModel.Initialize();
			Resources = Styles.Implicit.Dictionary;
			MainPage = new SearchPage(searchViewModel);
		}
    }
}