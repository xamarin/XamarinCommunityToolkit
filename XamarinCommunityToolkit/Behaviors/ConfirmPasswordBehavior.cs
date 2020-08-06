using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class ConfirmPasswordBehavior : ValidationBehavior
    {
        public static readonly BindableProperty ComparedTextProperty =
            BindableProperty.Create(nameof(ComparedText), typeof(string),
                typeof(ConfirmPasswordBehavior), string.Empty);
        protected override bool Validate(object value)
        {
            var confirmPasswordText = value as string;
            return confirmPasswordText != null && confirmPasswordText == ComparedText;
        }
        
        public string ComparedText
        {
            get => (string)base.GetValue(ComparedTextProperty);
            set => base.SetValue(ComparedTextProperty, value);
        }
    }
}