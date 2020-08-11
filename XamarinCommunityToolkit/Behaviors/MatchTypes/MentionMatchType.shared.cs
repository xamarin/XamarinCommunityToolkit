using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class MentionMatchType : MatchType
    {
        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"[@]\w+", RegexOptions.Singleline));

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
