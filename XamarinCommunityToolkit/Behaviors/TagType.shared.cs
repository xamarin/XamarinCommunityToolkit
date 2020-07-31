using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class TagType
    {
        public string Symbol { get; set; } = "#";
        public Style Style { get; set; } = new Style(typeof(Span))
        {
            Class = "DefaultSpanStyle",
            Setters =
                {
                    new Setter
                    {
                        Property = Span.TextColorProperty,
                        Value = Color.Blue
                    }
                }
        };
    }
}
