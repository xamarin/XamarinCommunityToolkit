using System.Windows.Input;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class ConfirmEqualInputsBehavior : ValidationBehavior
    {
        public static readonly BindableProperty ComparedToStringProperty =
            BindableProperty.Create(nameof(ComparedToString), typeof(string),
                typeof(ConfirmEqualInputsBehavior));
    
        public static readonly BindableProperty ValidCommandProperty =
            BindableProperty.Create(nameof(ValidCommand), typeof(ICommand),
                typeof(ConfirmEqualInputsBehavior));

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
