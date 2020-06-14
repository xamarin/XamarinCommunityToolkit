using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace XamarinCommunityToolkit.Behaviors
{
    [Preserve(AllMembers = true)]
    public enum ComparisonCondition
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }
}
