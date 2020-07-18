using System;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Converters;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Converters
{
    public partial class ConvertersGalleryPage : ContentPage
    {
        ICommand navigateCommand;

        public ConvertersGalleryPage()
            => InitializeComponent();

        public ICommand NavigateCommand => navigateCommand ??= new Command(parameter
            => Navigation.PushAsync(PreparePage((ConverterSectionId)parameter)));

        Page PreparePage(ConverterSectionId id)
        {
            var page = GetPage(id);
            page.Title = id.GetTitle();
            return page;
        }

        Page GetPage(ConverterSectionId id)
            => id switch
            {
                ConverterSectionId.ItemTappedEventArgs => new ItemTappedEventArgsPage(),
                ConverterSectionId.ItemSelectedEventArgs => new ItemSelectedEventArgsPage(),
                _ => throw new NotImplementedException()
            };
    }
}
