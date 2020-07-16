using System.Collections.Generic;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;
using XamarinCommunityToolkitSample.Pages.Behaviors;
using XamarinCommunityToolkitSample.Pages.Converters;
using XamarinCommunityToolkitSample.Pages.Extensions;
using XamarinCommunityToolkitSample.Pages.TestCases;
using XamarinCommunityToolkitSample.Pages.Views;

namespace XamarinCommunityToolkitSample.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new  SectionModel(typeof(BehaviorsGalleryPage), "Behaviors", Color.FromHex("#8E8CD8"), "Behaviors lets you add functionality to user interface controls without having to subclass them. Behaviors are written in code and added to controls in XAML or code."),
            new  SectionModel(typeof(ConvertersGalleryPage), "Converters", Color.FromHex("#EA005E"), "Converters let you convert bindings of a certain type to a different value, based on custom logic."),
            new  SectionModel(typeof(ExtensionsGalleryPage), "Extensions", Color.FromHex("#00CC6A"), "Extensions are used to supplement existing functionalities by making them easier to use."),
            new  SectionModel(typeof(TestCasesGalleryPage), "Test Cases", Color.FromHex("#FF8C00"), "Testing is important, ok?! So this is where all of the tests for our little project reside."),
            new  SectionModel(typeof(ViewsGalleryPage), "Views", Color.FromHex("#EF6950"), "A custom view or control allows for adding custom functionality as if it came out of the Xamarin.Forms box.")
        };
    }
}
