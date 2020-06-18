using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinCommunityToolkitSample.Views
{
    public partial class WelcomePage : ContentPage
    {
        ICommand navigateCommand;

        public WelcomePage()
            => InitializeComponent();

        public ICommand NavigateCommand => navigateCommand ??= new Command(parameter
            => Navigation.PushAsync(GetPage((Section.SectionId)parameter)));

        public Section[] Sections { get; } =
        {
            new Section { Id = Section.SectionId.Behaviors, Text = "Behaviors Gallery" },
            new Section { Id = Section.SectionId.Converters, Text = "Converters Gallery" },
            new Section { Id = Section.SectionId.Views, Text = "Views Gallery" },
            new Section { Id = Section.SectionId.TestCases, Text = "Test Cases Gallery" },
        };

        Page GetPage(Section.SectionId id)
            => id switch
            {
                Section.SectionId.Behaviors => new BehaviorsPage(),
                Section.SectionId.Converters => new ContentPage { Title = "Not Implemented yet" },
                Section.SectionId.Views => new ContentPage { Title = "Not Implemented yet" },
                Section.SectionId.TestCases => new ContentPage { Title = "Not Implemented yet" },
                _ => throw new System.NotImplementedException()
            };

        public sealed class Section
        {
            public SectionId Id { get; set; }
            public string Text { get; set; }

            public enum SectionId
            {
                Behaviors,
                Converters,
                Views,
                TestCases
            }
        }
    }
}
