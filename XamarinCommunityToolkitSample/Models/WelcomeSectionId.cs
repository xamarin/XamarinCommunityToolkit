using System;

namespace XamarinCommunityToolkitSample.Models
{
    public enum WelcomeSectionId
    {
        Behaviors,
        Converters,
        Extensions,
        Views,
        TestCases
    }

    public static class WelcomeSectionIdExtensions
    {
        public static string GetTitle(this WelcomeSectionId id)
            => id switch
            {
                WelcomeSectionId.Behaviors => "Behaviors",
                WelcomeSectionId.Converters => "Converters",
                WelcomeSectionId.Extensions => "Extensions",
                WelcomeSectionId.Views => "Views",
                WelcomeSectionId.TestCases => "Test Cases",
                _ => throw new NotImplementedException()
            } + " Gallery";
    }
}
