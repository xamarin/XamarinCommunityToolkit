using System;
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
			=> ApplyToControl(Control, apply);

		bool ApplyToControl<T>(T controlType, bool apply) =>
			controlType switch
			{
				UITextField textField => ApplyToUITextField(textField, apply),
				UITextView _ => ApplyToUITextView(apply),
				_ => throw new NotSupportedException($"Control of type: {controlType.GetType().Name} is not supported by this effect.")
			};

		#region - UITextField

		bool ApplyToUITextField(UITextField textField, bool apply)
		{
			if (textField == null)
				return false;

			if (apply)
				textField.EditingDidBegin += OnEditingDidBegin;
			else
				textField.EditingDidBegin -= OnEditingDidBegin;

			return true;
		}

		void OnEditingDidBegin(object sender, EventArgs e)
		{
			var textfield = sender as UITextField;

			if (textfield == null)
				return;

			textfield.PerformSelector(new Selector("selectAll"), null, 0.0f);
		}

		#endregion - UI TextField

		#region - UI TextView

		bool ApplyToUITextView(bool apply)
		{
			var formsControl = Element as Editor;
			if (formsControl == null)
				return false;

			if (apply)
				formsControl.Focused += OnTextViewFocussed;
			else
				formsControl.Focused -= OnTextViewFocussed;

			return true;
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

		#endregion - UI TextView
	}
}