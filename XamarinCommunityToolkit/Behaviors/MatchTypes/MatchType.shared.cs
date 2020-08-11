using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public abstract class MatchType
    {
        public abstract Lazy<Regex> Regex { get; }
        public abstract Style Style { get; }

        public virtual string GetValue(string value) => value;

        public virtual string GetText(string text) => text;
    }
}
