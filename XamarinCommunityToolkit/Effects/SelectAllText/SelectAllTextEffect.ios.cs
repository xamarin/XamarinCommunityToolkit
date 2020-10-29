using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.SelectAllTextEffect), nameof(SelectAllTextEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
    public class SelectAllTextEffect : PlatformEffect
	{
        protected override void OnAttached() => ApplyEffect(true);

        protected override void OnDetached() => ApplyEffect(false);

        void ApplyEffect(bool apply)
		{
            var controlType = Control.GetType();

            if (!ControlIsSupported(controlType))
                return;

            if (controlType == typeof(UITextField))
			{
                var textField = Control as UITextField;
                if (textField == null)
                    return;

                if (apply)
                {
                    textField.EditingDidBegin += OnEditingDidBegin;
                }
                else
                {
                    textField.EditingDidBegin -= OnEditingDidBegin;
                }
            }
            else if (controlType.BaseType == typeof(UITextView))
			{
                var formsControl = Element as Editor;
                if (formsControl == null)
                    return;

                if (apply)
				{
					formsControl.Focused += OnTextViewFocussed;
				}
                else
				{
                    formsControl.Focused -= OnTextViewFocussed;
                }
			}
            else
			{
                throw new NotSupportedException($"Control of type: {controlType.Name} is not supported by this effect.");
			}
        }

        void OnTextViewFocussed(object sender, FocusEventArgs e)
		{
            var formsControl = Element as Editor;
            if (formsControl == null)
                return;

            var textView = Control as UITextView;
            if (textView == null)
                return;

            if (formsControl.IsFocused)
			{
                textView.SelectAll(textView);
            }
		}

        void OnEditingDidBegin(object sender, EventArgs e)
        {
            var textfield = sender as UITextField;

            if (textfield == null)
                return;

            textfield.PerformSelector(new Selector("selectAll"), null, 0.0f);
        }

        bool ControlIsSupported(Type controlType)
		{
            if (controlType == null)
                throw new ArgumentException("Type was null", nameof(controlType));

            switch (controlType.Name)
			{
                case "UITextField":
                case "FormsUITextView":
	                return true;
                default:
	                return false;
            }
		}
    }
}
