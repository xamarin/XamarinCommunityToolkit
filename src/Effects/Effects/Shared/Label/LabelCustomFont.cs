using Xamarin.Forms;

namespace XamarinCommunityToolkit.Effects
{

    public class LabelCustomFont : RoutingEffect
    {
        /// <summary>
        /// Only needed on Android, FontFamilyName can be disregarded
        /// </summary>
        /// <value>The font path.</value>
        public string FontPath { get; set; }

        /// <summary>
        /// Only needed on iOS, FontPath can be disregarded
        /// </summary>
        /// <value>The name of the font family.</value>
        public string FontFamilyName { get; set; }

        public LabelCustomFont () : base (EffectIds.LabelCustomFont)
        {
        }
    }
}