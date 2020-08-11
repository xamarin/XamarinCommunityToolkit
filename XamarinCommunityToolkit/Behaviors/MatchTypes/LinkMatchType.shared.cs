using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class LinkMatchType : MatchType
    {
        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+", RegexOptions.Compiled | RegexOptions.Singleline));

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
