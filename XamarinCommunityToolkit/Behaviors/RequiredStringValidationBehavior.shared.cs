using System.Windows.Input;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class RequiredStringValidationBehavior : ValidationBehavior
    {
        public static readonly BindableProperty ComparedToStringProperty =
            BindableProperty.Create(nameof(ComparedToString), typeof(string),
                typeof(RequiredStringValidationBehavior));
    
        public static readonly BindableProperty ValidCommandProperty =
            BindableProperty.Create(nameof(ValidCommand), typeof(ICommand),
                typeof(RequiredStringValidationBehavior));

        protected override bool Validate(object value)
        {
            if (value?.ToString() == ComparedToString)
            {
                ValidCommand?.Execute(value);
                return true;
            }

            return false;
        }

        public string ComparedToString
        {
            get { return (string) GetValue(ComparedToStringProperty); }
            set { SetValue(ComparedToStringProperty, value); }
        }
        
        public ICommand ValidCommand
        {
            get => (ICommand)GetValue(ValidCommandProperty);
            set => SetValue(ValidCommandProperty, value);
        }
    }
}
