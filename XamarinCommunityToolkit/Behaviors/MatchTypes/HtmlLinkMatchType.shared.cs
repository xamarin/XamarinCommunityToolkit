using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
namespace Xamarin.CommunityToolkit.Behaviors
{
    public class HtmlLinkMatchType : MatchType
    {
#if !NETSTANDARD1_0
        Lazy<Regex> extractValueRegex = new Lazy<Regex>(() => new Regex("(?<=href=\\\")[\\S]+(?=\\\")", RegexOptions.Compiled | RegexOptions.Singleline));

        Lazy<Regex> extractTextRegex = new Lazy<Regex>(() => new Regex("<.*?>", RegexOptions.Compiled | RegexOptions.Singleline));

        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"(<a.*?>.*?</a>)", RegexOptions.Compiled | RegexOptions.Singleline));
#else
        Lazy<Regex> extractValueRegex = new Lazy<Regex>(() => new Regex("(?<=href=\\\")[\\S]+(?=\\\")", RegexOptions.Singleline));

        Lazy<Regex> extractTextRegex = new Lazy<Regex>(() => new Regex("<.*?>", RegexOptions.Singleline));

        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"(<a.*?>.*?</a>)", RegexOptions.Singleline));
#endif
        public override string GetValue(string value) => extractValueRegex.Value.Match(value).Value;

        public override string GetText(string text) => extractTextRegex.Value.Replace(text, string.Empty);

        public override Style Style => new Style(typeof(Span))
        {
            Class = "HtmlLinkSpanStyle",
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
