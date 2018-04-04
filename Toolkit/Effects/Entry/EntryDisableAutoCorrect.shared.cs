using Xamarin.Forms;

namespace Xamarin.Toolkit.Effects
{
    /// <summary>
    /// When attached to a Xamarin.Forms.Entry control, disables auto-suggestion, auto-capitilisation and auto-correction for entered text.
    /// </summary>
    public class EntryDisableAutoCorrect : RoutingEffect
    {
        public EntryDisableAutoCorrect() : base(EffectIds.EntryDisableAutoCorrect)
        {
        }
    }
}
