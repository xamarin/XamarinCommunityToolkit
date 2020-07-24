using System;
using System.Windows.Input;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Pages
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
