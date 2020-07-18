
namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Models
{
    public sealed class WelcomeSectionModel
    {
        public WelcomeSectionModel(WelcomeSectionId id)
        {
            Id = id;
            Title = id.GetTitle();
        }

        public WelcomeSectionId Id { get; }
        public string Title { get; }
    }
}
