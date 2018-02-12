using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using ObjCRuntime;
using RoutingEffects = XamarinCommunityToolkit.Effects;
using PlatformEffects = XamarinCommunityToolkit.Effects.iOS;

[assembly: ExportEffect(typeof(PlatformEffects.EntrySelectAllText), nameof(RoutingEffects.EntrySelectAllText))]
namespace XamarinCommunityToolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public class EntrySelectAllText : PlatformEffect
    {
        protected override void OnAttached()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            editText.EditingDidBegin += (object sender, EventArgs e) =>
            {
                editText.PerformSelector(new Selector("selectAll"), null, 0.0f);
            };
        }

        protected override void OnDetached()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            editText.EditingDidBegin -= (object sender, EventArgs e) =>
            {
                editText.PerformSelector(new Selector("selectAll"), null, 0.0f);
            };
        }
    }
}
