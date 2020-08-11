﻿using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class HashtagMatchType : MatchType
    {
        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"#\w+", RegexOptions.Compiled | RegexOptions.Singleline));

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
