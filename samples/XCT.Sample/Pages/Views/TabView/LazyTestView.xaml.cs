using Xamarin.CommunityToolkit.Sample.ViewModels.Views.Tabs;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class LazyTestView : ContentView
	{
		public LazyTestView()
		{
			InitializeComponent();

			Build();
			NormalTestViewModel.Current.LoadedViews += "LazyView Loaded \n";
		}

		void Build()
		{
			for (var i = 0; i < 117; i++)
			{
				var box = new BoxView
				{
					BackgroundColor = i % 2 == 0 ? Color.Blue : Color.Fuchsia
				};

				uniformGrid.Children.Add(box);
			}
		}
	}
}