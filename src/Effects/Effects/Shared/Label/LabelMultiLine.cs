using Xamarin.Forms;

namespace Xamarin.Toolkit.Effects
{
    public class LabelMultiLine : RoutingEffect
    {
        public int Lines { get; set; }

        public LabelMultiLine() : base(EffectIds.LabelMultiLine)
        {
        }
    }
}
