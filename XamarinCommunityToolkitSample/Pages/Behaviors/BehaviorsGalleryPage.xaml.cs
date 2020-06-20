using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models.Behaviors;

namespace XamarinCommunityToolkitSample.Pages.Behaviors
{
    public partial class BehaviorsGalleryPage : ContentPage
    {
        ICommand navigateCommand;

        public BehaviorsGalleryPage()
            => InitializeComponent();

        public ICommand NavigateCommand => navigateCommand ??= new Command(parameter
            => Navigation.PushAsync(PreparePage((BehaviorSectionId)parameter)));

        Page PreparePage(BehaviorSectionId id)
        {
            var page = GetPage(id);
            page.Title = id.GetTitle();
            return page;
        }

        Page GetPage(BehaviorSectionId id)
            => id switch
            {
                BehaviorSectionId.EventToCommand => new EventToCommandBehaviorPage(),
                BehaviorSectionId.EmailValidation => new EmailValidationBehaviorPage(),
                BehaviorSectionId.NumericValidation => new NumericValidationBehaviorPage(),
                _ => throw new NotImplementedException()
            };
    }
}
