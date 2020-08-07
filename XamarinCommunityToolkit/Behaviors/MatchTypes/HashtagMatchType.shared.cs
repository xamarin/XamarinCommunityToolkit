using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class HashtagMatchType : MatchType
    {
        public override string Regex => @"#\w+";

        public override Style Style => new Style(typeof(Span))
        {
            Class = "HashtagSpanStyle",
            Setters =
                {
                    new Setter
                    {
                        Property = Span.TextColorProperty,
                        Value = Color.Green
                    }
                }
        };
    }
}
