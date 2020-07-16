using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;

namespace XamarinCommunityToolkitSample.Pages
{
    public class BasePage : ContentPage
    {
        ICommand navigateCommand;

        public Color DetailColor { get; set; }

        public ICommand NavigateCommand => navigateCommand ??= new Command(parameter
            => Navigation.PushAsync(PreparePage((SectionModel)parameter)));

        Page PreparePage(SectionModel model)
        {
            var page = (BasePage)Activator.CreateInstance(model.Type);
            page.Title = model.Title;
            page.DetailColor = model.Color;
            return page;
        }
    }
}
