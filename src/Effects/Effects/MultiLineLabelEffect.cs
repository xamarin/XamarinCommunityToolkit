using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    public class MultiLineLabelEffect : RoutingEffect
    {
        public int Lines { get; set; }

        public MultiLineLabelEffect() : base(EffectIds.MultiLineLabelEffect)
        {
        }
    }
}
