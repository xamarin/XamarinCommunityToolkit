using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;

namespace Xamarin.CommunityToolkit.MarkupSample
{
	public class MainPage : ContentPage
	{
		public MainPage()
		{
			Content = new Label { Text = "Hi" } .FontSize(30) .Italic() .Center();
		}
	}
}
