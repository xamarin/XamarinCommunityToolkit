using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class ConfirmPasswordBehavior : ValidationBehavior
    {
        public static readonly BindableProperty OriginalPasswordProperty =
            BindableProperty.Create(nameof(OriginalPassword), typeof(string),
                typeof(ConfirmPasswordBehavior));
        
        protected override bool Validate(object value)
            => value?.ToString() == OriginalPassword;
        
        public string OriginalPassword
        {
            get => (string)GetValue(OriginalPasswordProperty);
            set => SetValue(OriginalPasswordProperty, value);
        }
    }
}
