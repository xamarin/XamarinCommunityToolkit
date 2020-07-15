
namespace XamarinCommunityToolkitSample.Models
{
    public sealed class WelcomeSectionModel
    {
        public WelcomeSectionModel(WelcomeSectionId id, string description)
        {
            Id = id;
            Title = id.GetTitle();
            Description = description;
        }

        public WelcomeSectionId Id { get; }
        public string Title { get; }
        public string Description { get; }
    }
}
