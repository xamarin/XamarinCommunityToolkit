using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntrySelectAllText), nameof(RoutingEffects.EntrySelectAllText))]
namespace Xamarin.Toolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public class EntrySelectAllText : PlatformEffect
    {
        protected override void OnAttached()
        {
            var editText = Control as UITextField;
            if (editText == null)
            {
                return;
            }

            editText.EditingDidBegin += (object sender, EventArgs e) =>
            {
                editText.PerformSelector(new Selector("selectAll"), null, 0.0f);
            };
        }

        protected override void OnDetached()
        {
            var editText = Control as UITextField;
            if (editText == null)
            {
                return;
            }

            editText.EditingDidBegin -= (object sender, EventArgs e) =>
            {
                editText.PerformSelector(new Selector("selectAll"), null, 0.0f);
            };
        }
    }
}
