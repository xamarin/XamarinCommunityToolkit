using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    public class LabelMultiLine : RoutingEffect
    {
        public int Lines { get; set; }

        public LabelMultiLine() : base(EffectIds.LabelMultiLine)
        {
        }
    }
}
