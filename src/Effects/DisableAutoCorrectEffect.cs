using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    /// <summary>
    /// When attached to a Xamarin.Forms.Entry control, disables auto-suggestion, auto-capitilisation and auto-correction for entered text.
    /// </summary>
    public class DisableAutoCorrectEffect : RoutingEffect
    {
        public DisableAutoCorrectEffect() : base(EffectIds.DisableAutoCorrectEffect)
        {
        }
    }
}
