using System;
using System.Collections.Generic;
using System.Linq;
using XamarinCommunityToolkitSample.Models;

namespace XamarinCommunityToolkitSample.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public IEnumerable<WelcomeSectionModel> Items { get; } = new List<WelcomeSectionModel> {
            new WelcomeSectionModel(WelcomeSectionId.Behaviors, "Behaviors lets you add functionality to user interface controls without having to subclass them. Behaviors are written in code and added to controls in XAML or code."),
            new WelcomeSectionModel(WelcomeSectionId.Converters, "Converters let you convert bindings of a certain type to a different value, based on custom logic."),
            new WelcomeSectionModel(WelcomeSectionId.Extensions, "Extensions are used to supplement existing functionalities by making them easier to use."),
            new WelcomeSectionModel(WelcomeSectionId.TestCases, "Testing is important, ok?! So this is where all of the tests for our little project reside."),
            new WelcomeSectionModel(WelcomeSectionId.Views, "A custom view or control allows for adding custom functionality as if it came out of the Xamarin.Forms box.")
        };
    }
}
