using XamarinCommunityToolkitSample.Models;

namespace XamarinCommunityToolkitSample.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public WelcomeSectionModel[] Items { get; } =
        {
            new WelcomeSectionModel(WelcomeSectionId.Behaviors),
            new WelcomeSectionModel(WelcomeSectionId.Converters),
            new WelcomeSectionModel(WelcomeSectionId.Extensions),
            new WelcomeSectionModel(WelcomeSectionId.Views),
            new WelcomeSectionModel(WelcomeSectionId.TestCases)
        };
    }
}
