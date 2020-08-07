using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class LinkMatchType : MatchType
    {
        public override string Regex => @"(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+";

        public override Style Style => new Style(typeof(Span))
        {
            Class = "LinkSpanStyle",
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
