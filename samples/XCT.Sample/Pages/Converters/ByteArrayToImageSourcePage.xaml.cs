using Xamarin.CommunityToolkit.Sample.ViewModels.Converters;

namespace Xamarin.CommunityToolkit.Sample.Pages.Converters
{
	public partial class ByteArrayToImageSourcePage : BasePage
	{
		public ByteArrayToImageSourcePage()
			=> InitializeComponent();

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await ((ByteArrayToImageSourceViewModel)BindingContext).OnAppearing();
		}
	}
}