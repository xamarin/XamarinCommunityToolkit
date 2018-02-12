using Xamarin.Forms;

namespace XamarinCommunityToolkit.Effects
{
    public class LabelMultiLine : RoutingEffect
    {
        public int Lines { get; set; }

        public LabelMultiLine() : base(EffectIds.LabelMultiLine)
        {
        }
    }
}
