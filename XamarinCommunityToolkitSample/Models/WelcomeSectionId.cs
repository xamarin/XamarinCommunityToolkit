using System;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Models
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
                WelcomeSectionId.TestCases => "Test Cases",
                _ => id.ToString()
            } + " Gallery";
    }
}
