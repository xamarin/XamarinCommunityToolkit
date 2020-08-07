using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class ConfirmPasswordBehavior : ValidationBehavior
    {
        public static readonly BindableProperty ComparedTextProperty =
            BindableProperty.Create(nameof(ComparedText), typeof(string),
                typeof(ConfirmPasswordBehavior));
        
        protected override bool Validate(object value)
        {
            var confirmPasswordText = value?.ToString();
            return confirmPasswordText == ComparedText;
        }
        
        public string ComparedText
        {
            get => (string)GetValue(ComparedTextProperty);
            set => SetValue(ComparedTextProperty, value);
        }
    }
}