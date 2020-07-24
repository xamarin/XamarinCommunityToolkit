using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Views;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Views
{
    public class ViewsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(typeof(AvatarViewPage), "AvatarView", Color.FromHex("#498205"), "The AvatarView represents a user's name by using the initials and a generated background color."),
            new SectionModel(typeof(RangeSliderPage), "RangeSlider", Color.Red, "The RangeSlider is a slider with two thumbs allowing to select numeric ranges."),
        };
    }
}
