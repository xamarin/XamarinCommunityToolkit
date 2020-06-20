using System;

namespace XamarinCommunityToolkitSample.Models.Views
{
    public enum ViewSectionId
    {
        AvatarView
    }

    public static class ViewSectionIdIdExtensions
    {
        public static string GetTitle(this ViewSectionId id)
            => id switch
            {
                ViewSectionId.AvatarView => "AvatarView",
                _ => throw new NotImplementedException()
            };
    }
}
