using System;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    [Flags]
    public enum TextDecorationFlags
    {
        None = 0,
        TrimStart = 1,
        TrimEnd = 2,
        Trim = 3,
        NullToEmpty = 4,
        ReduceWhiteSpaces = 8
    }
}
