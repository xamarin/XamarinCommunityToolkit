using Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Converters;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Converters
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
