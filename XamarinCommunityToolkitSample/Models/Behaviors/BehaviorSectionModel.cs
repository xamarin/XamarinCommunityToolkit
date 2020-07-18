using System;
namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Behaviors
{
    public class BehaviorSectionModel
    {
        public BehaviorSectionModel(BehaviorSectionId id)
        {
            Id = id;
            Title = id.GetTitle();
        }

        public BehaviorSectionId Id { get; }
        public string Title { get; }
    }
}
