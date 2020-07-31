using System.Text.RegularExpressions;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class PasswordValidationBehavior : TextValidationBehavior
    {
        protected override string DefaultRegexPattern
            => @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";

        protected override RegexOptions DefaultRegexOptions => RegexOptions.IgnoreCase;
    }
}