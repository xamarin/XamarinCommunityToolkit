using System;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Views
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
                _ => id.ToString()
            };
    }
}
