using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class HtmlLinkMatchType : MatchType
    {
        Lazy<Regex> extracValueRegex = new Lazy<Regex>(() => new Regex("(?<=href=\\\")[\\S]+(?=\\\")", RegexOptions.Singleline));

        Lazy<Regex> extractTextRegex = new Lazy<Regex>(() => new Regex("<.*?>", RegexOptions.Singleline));

        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"(<a.*?>.*?</a>)", RegexOptions.Singleline));

        public override string GetValue(string value) => extracValueRegex.Value.Match(value).Value;

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
