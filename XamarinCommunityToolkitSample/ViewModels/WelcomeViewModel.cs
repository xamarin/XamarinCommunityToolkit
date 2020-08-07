using System.Collections.Generic;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Behaviors;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Converters;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Views;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.TestCases;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Extensions;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Resx;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new  SectionModel(typeof(BehaviorsGalleryPage), AppResources.BehaviorsTitle, Color.FromHex("#8E8CD8"), AppResources.BehaviorsDescription),
            new  SectionModel(typeof(ConvertersGalleryPage), AppResources.ConvertersTitle, Color.FromHex("#EA005E"), AppResources.ConvertersDescription),
            new  SectionModel(typeof(ExtensionsGalleryPage), AppResources.ExtensionsTitle, Color.FromHex("#00CC6A"), AppResources.ExtensionsDescription),
            new  SectionModel(typeof(TestCasesGalleryPage), AppResources.TestCasesTitle, Color.FromHex("#FF8C00"), AppResources.TestCasesDescription),
            new  SectionModel(typeof(ViewsGalleryPage), AppResources.ViewsTitle, Color.FromHex("#EF6950"), AppResources.ViewsDescription)
        };
    }
}
