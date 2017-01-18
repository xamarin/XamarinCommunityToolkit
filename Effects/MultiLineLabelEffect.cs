using System.Diagnostics.Contracts;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    public class MultiLineLabelEffect : RoutingEffect
    {
        public int Lines { get; set; }

        public MultiLineLabelEffect() : base("FormsCommunityToolkit.Effects.MultiLineLabelEffect")
        {
        }
    }
}
