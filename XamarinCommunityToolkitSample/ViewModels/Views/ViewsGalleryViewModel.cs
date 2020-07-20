using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;
using XamarinCommunityToolkitSample.Pages.Views;

namespace XamarinCommunityToolkitSample.ViewModels.Views
{
    public class ViewsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(typeof(AvatarViewPage), "AvatarView", Color.FromHex("#498205"), "The AvatarView represents a user's name by using the initials and a generated background color."),
        };
    }
}
