using Xamarin.CommunityToolkit.Sample.ViewModels.Views.Tabs;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

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
					BackgroundColor = i % 2 == 0 ? Colors.Blue : Colors.Fuchsia
				};

				uniformGrid.Children.Add(box);
			}
		}
	}
}