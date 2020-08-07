using Xamarin.Forms;
namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public abstract class MatchType
    {
        public abstract string Regex { get; }
        public abstract Style Style { get; }
    }
}
