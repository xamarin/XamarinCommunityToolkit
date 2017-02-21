using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsCommunityToolkit.Effects.Droid;
using Android.Runtime;

[assembly: ExportEffect(typeof(EntryRemoveBorder), nameof(EntryRemoveBorder))]
namespace FormsCommunityToolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class EntryRemoveBorder : PlatformEffect
    {
        protected override void OnAttached()
        {
        }

        protected override void OnDetached()
        {
        }
    }
}