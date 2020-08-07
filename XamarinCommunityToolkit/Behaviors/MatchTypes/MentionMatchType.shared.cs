using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class MentionMatchType : MatchType
    {
        public override string Regex => @"[@]\w+";

        public override Style Style => new Style(typeof(Span))
        {
            Class = "MentionSpanStyle",
            Setters =
                {
                    new Setter
                    {
                        Property = Span.TextColorProperty,
                        Value = Color.Red
                    }
                }
        };
    }
}
